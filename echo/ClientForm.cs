using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace echo
{
    internal struct ConnectionData
    {
        public string RoomId;
        public string ClientId;
        public string Url;
        public string Message;

        public ConnectionData(string roomId, string clientId, string url, string message)
        {
            RoomId = roomId;
            ClientId = clientId;
            Url = url;
            Message = message;
        }
    }

    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private bool _connected;

        private delegate void DisconnectionDelegate(bool value);
        private void SetConnected(bool value)
        {
            if (InvokeRequired)
            {
                var disconnectedDelegate = new DisconnectionDelegate(SetConnected);
                Invoke(disconnectedDelegate,value);
            }
            else
            {
                _connected = value;
                if (value)
                {
                    Connect();
                }
                else
                {
                    Disconnect();
                }
            }
        }

        private delegate void AppendDelegate(string value);
        private void AppendOutput(string value)
        {
            if (InvokeRequired)
            {
                var appendDelegate = new AppendDelegate(AppendOutput);
                Invoke(appendDelegate, value);
            }
            else
            {
                Output.AppendText(value);
            }
        }

        private Thread _connectionThread;
        private void Connectbutton_Click(object sender, EventArgs e)
        {
            SetConnected(!_connected);
        }

        private void Connect()
        {
            ClientIDtext.Enabled = false;
            RoomIDtext.Enabled = false;
            URLtext.Enabled = false;
            Messagetext.Enabled = false;
            Connectbutton.Text = @"Disconnect";
            _connectionThread = new Thread(ConnectionThreadLoop);
            _connectionThread.Start(new ConnectionData(RoomIDtext.Text + "\n", ClientIDtext.Text + "\n", URLtext.Text, Messagetext.Text + "\n"));
        }

        private void Disconnect()
        {
            ClientIDtext.Enabled = true;
            RoomIDtext.Enabled = true;
            URLtext.Enabled = true;
            Messagetext.Enabled = true;
            Connectbutton.Text = @"Connect";
            Output.Text = "";
            if (_connectionThread.IsAlive)
            {
                _connectionThread.Abort();
            }
        }

        private readonly byte[] _buffer = new byte[4096];
        private NetworkStream _networkstream;
        private void ConnectionThreadLoop(object obj)
        {
            ConnectionData data = (ConnectionData)obj;
            try
            {
                var host = data.Url.Split(':')[0];
                var port = Int32.Parse(data.Url.Split(':')[1]);
                var client = new TcpClient(host, port);
                _networkstream = client.GetStream(); 
                _networkstream.ReadAsync(_buffer, 0, 2048).ContinueWith(AddToOutput);
                SendString(_networkstream,data.ClientId);
                SendString(_networkstream, data.RoomId);
                for(;;)
                {
                    var timestamp = DateTime.Now;
                    SendString(_networkstream, data.Message);
                    _networkstream.Flush();
                    var diff = (DateTime.Now - timestamp).TotalMilliseconds;
                    if (diff < 100)
                    {
                        Thread.Sleep(100 - (int)diff);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                _networkstream.Dispose();
                _networkstream = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                SetConnected(false);
                Thread.CurrentThread.Abort();
            }
        }

        private void AddToOutput(Task<int> t)
        {
            try
            {
                string output = System.Text.Encoding.UTF8.GetString(_buffer.Take(t.Result).ToArray());
                AppendOutput(output);
                _networkstream.ReadAsync(_buffer, 0, 2048).ContinueWith(AddToOutput);
            }
            catch (Exception)
            {
                
            }
        }

        private static void SendString(NetworkStream writer, string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            writer.Write(bytes, 0, bytes.Length);
        }
    }
}
