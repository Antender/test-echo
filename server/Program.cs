using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace server
{
    using RoomClients = ConcurrentDictionary<string, Client>;

    public class Message<T> where T : class
    {
        public Message(T payload)
        {
            Payload = payload;
        }

        public Message<T> Case<T2>(Action<T2> action) where T2: class
        {
            if (Payload is T2)
            {
                action.Invoke(Payload as T2);
                return null;
            }
            return this;
        }
        public T Payload;
    }

    public interface IMessageType
    {
        
    }
        
    public class Received : IMessageType
    {
        public Received(string roomId, byte[] message)
        {
            RoomId = roomId;
            Message = message;
        }
        public string RoomId;
        public byte[] Message;
    }

    public class Connected : IMessageType
    {
        public Connected(Client client)
        {
            Client = client;
        }
        public Client Client;
    }

    public class Disconnected : IMessageType
    {
        public Disconnected(Client client)
        {
            Client = client;
        }

        public Client Client;
    }

    public class RoomDestroyed : IMessageType
    {
        public RoomDestroyed(string roomId)
        {
            RoomId = roomId;
        }
        public string RoomId;
    }

    
    class Server
    {
        private static Dictionary<string, RoomClients> Rooms = new Dictionary<string, RoomClients>();
        public static ConcurrentQueue<Message<IMessageType>> Messages = new ConcurrentQueue<Message<IMessageType>>();
        public static ConcurrentDictionary<string, Timer> RoomTimers = new ConcurrentDictionary<string, Timer>();

        const int Port = 50000;
        private static Listener _listener;

        static readonly Action<Connected> ConnectedHandler = delegate (Connected m)
        {
            Console.Out.WriteLine(m.Client.ClientId + " connected");
            var roomId = m.Client.RoomId;
            var clientId = m.Client.ClientId;
            if (!Rooms.ContainsKey(roomId))
            {
                Console.Out.WriteLine("room" + m.Client.RoomId + " created");
                Rooms[roomId] = new RoomClients();
                RoomTimers[roomId] = new Timer(RoomExpired, roomId, 60000, Timeout.Infinite);
            }
            if (Rooms[roomId].ContainsKey(clientId))
            {
                var oldClient = Rooms[roomId][clientId];
                Rooms[roomId][clientId] = m.Client;
            }
            else
            {
                Rooms[roomId][clientId] = m.Client;
            }
        };

        private static readonly Action<Received> ReceivedHandler = delegate(Received m)
        {
            Console.Out.WriteLine("room " + m.RoomId + " received:" + Encoding.UTF8.GetString(m.Message));
            RoomTimers[m.RoomId].Change(60000, Timeout.Infinite);
            foreach (var client in Rooms[m.RoomId].Values)
            {
                client.NetworkStream.WriteAsync(m.Message, 0, m.Message.Length);
            }
        };

        private static readonly Action<Disconnected> DisconnectedHandler = delegate(Disconnected m)
        {
            Console.Out.WriteLine(m.Client.ClientId + " disconnected");
            var roomId = m.Client.RoomId;
            var clientId = m.Client.ClientId;
            while (Rooms[roomId].ContainsKey(clientId))
            {
                Client oldClient;
                Rooms[roomId].TryRemove(clientId, out oldClient);
            }
        };

        private static readonly Action<RoomDestroyed> RoomDestroyedHandler = delegate(RoomDestroyed m)
        {
            Console.Out.WriteLine(m.RoomId + " destroyed");
            Rooms.Remove(m.RoomId);
        };

        static void Main()
        {

            try
            {
                _listener = new Listener(Port);
                for(;;)
                {
                    while (!Messages.IsEmpty)
                    {
                        Message<IMessageType> message;
                        if (Messages.TryDequeue(out message))
                        {
                            message
                            ?.Case(ConnectedHandler)
                            ?.Case(ReceivedHandler)
                            ?.Case(DisconnectedHandler)
                            ?.Case(RoomDestroyedHandler);
                        }
                    }
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Out.WriteLine("Main thread is dead");
                Console.ReadLine();
            }
        }

        static void RoomExpired(object oRoomId)
        {
            var roomId = (string) oRoomId;
            while (RoomTimers.ContainsKey(roomId))
            {
                Timer oldTimer;
                RoomTimers.TryRemove(roomId, out oldTimer);
            }
            Messages.Enqueue(new Message<IMessageType>(new RoomDestroyed (roomId)));
        }
    }

    public class Listener
    {
        public readonly int Port;
        private readonly TcpListener _tcpListener;

        public Listener(int port)
        {
            Port = port;
            _tcpListener = new TcpListener(IPAddress.Any, Port);
            _tcpListener.Start();
            Console.Out.WriteLine("Listening on port " + port);
            Task.Factory.StartNew(ListenLoop);
        }

        private async void ListenLoop()
        {
            for (;;)
            {
                var socket = await _tcpListener.AcceptSocketAsync();
                if (socket == null)
                {
                    break;
                }
                Task.Factory.StartNew(new Client(socket).Receive);
            }
        }
    }

    public class Client
    {
        private readonly Socket _socket;
        public readonly NetworkStream NetworkStream;
        public string ClientId;
        public string RoomId;

        public Client(Socket socket)
        {
            _socket = socket;
            NetworkStream = new NetworkStream(socket, true);
        }

        public async void Receive()
        {
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            do
            {
                if (bytesRead >= buffer.Length)
                {
                    Console.Out.WriteLine("ClientID and RoomID length exceeded");
                    var message = Encoding.UTF8.GetBytes("ClientID and RoomID length exceeded");
                    NetworkStream.Write(message, 0, message.Length);
                    NetworkStream.Dispose();
                    return;
                }
                bytesRead += await NetworkStream.ReadAsync(buffer, bytesRead, buffer.Length - bytesRead);
            } while (Encoding.UTF8.GetString(buffer).Count(el => el == '\n') < 2);
            var temp = Encoding.UTF8.GetString(buffer).Split('\n');
            ClientId = temp[0];
            RoomId = temp[1];
            Server.Messages.Enqueue(new Message<IMessageType>(new Connected(this)));
            for (;;)
            {
                try
                {
                    bytesRead = await NetworkStream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        Server.Messages.Enqueue(new Message<IMessageType>(new Disconnected(this)));
                        return;
                    }
                    Server.Messages.Enqueue(new Message<IMessageType>(new Received(
                        RoomId,
                        buffer.Take(bytesRead).ToArray()
                    )));
                }
                catch (Exception e)
                {
                    Server.Messages.Enqueue(new Message<IMessageType>(new Disconnected(this)));
                    return;
                }
            }
        }
    }
}
