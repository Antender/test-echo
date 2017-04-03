namespace echo
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ClientIDlabel = new System.Windows.Forms.Label();
            this.ClientIDtext = new System.Windows.Forms.TextBox();
            this.RoomIDtext = new System.Windows.Forms.TextBox();
            this.RoomIDlabel = new System.Windows.Forms.Label();
            this.URLtext = new System.Windows.Forms.TextBox();
            this.URLlabel = new System.Windows.Forms.Label();
            this.Connectbutton = new System.Windows.Forms.Button();
            this.Messagetext = new System.Windows.Forms.TextBox();
            this.Messagelabel = new System.Windows.Forms.Label();
            this.Output = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ClientIDlabel
            // 
            this.ClientIDlabel.AutoSize = true;
            this.ClientIDlabel.Location = new System.Drawing.Point(8, 9);
            this.ClientIDlabel.Name = "ClientIDlabel";
            this.ClientIDlabel.Size = new System.Drawing.Size(47, 13);
            this.ClientIDlabel.TabIndex = 0;
            this.ClientIDlabel.Text = "Client ID";
            // 
            // ClientIDtext
            // 
            this.ClientIDtext.Location = new System.Drawing.Point(10, 25);
            this.ClientIDtext.Name = "ClientIDtext";
            this.ClientIDtext.Size = new System.Drawing.Size(215, 20);
            this.ClientIDtext.TabIndex = 1;
            // 
            // RoomIDtext
            // 
            this.RoomIDtext.Location = new System.Drawing.Point(10, 73);
            this.RoomIDtext.Name = "RoomIDtext";
            this.RoomIDtext.Size = new System.Drawing.Size(215, 20);
            this.RoomIDtext.TabIndex = 3;
            // 
            // RoomIDlabel
            // 
            this.RoomIDlabel.AutoSize = true;
            this.RoomIDlabel.Location = new System.Drawing.Point(8, 54);
            this.RoomIDlabel.Name = "RoomIDlabel";
            this.RoomIDlabel.Size = new System.Drawing.Size(49, 13);
            this.RoomIDlabel.TabIndex = 2;
            this.RoomIDlabel.Text = "Room ID";
            // 
            // URLtext
            // 
            this.URLtext.Location = new System.Drawing.Point(10, 124);
            this.URLtext.Name = "URLtext";
            this.URLtext.Size = new System.Drawing.Size(215, 20);
            this.URLtext.TabIndex = 5;
            // 
            // URLlabel
            // 
            this.URLlabel.AutoSize = true;
            this.URLlabel.Location = new System.Drawing.Point(8, 105);
            this.URLlabel.Name = "URLlabel";
            this.URLlabel.Size = new System.Drawing.Size(50, 13);
            this.URLlabel.TabIndex = 4;
            this.URLlabel.Text = "URL:port";
            // 
            // Connectbutton
            // 
            this.Connectbutton.Location = new System.Drawing.Point(10, 195);
            this.Connectbutton.Name = "Connectbutton";
            this.Connectbutton.Size = new System.Drawing.Size(215, 26);
            this.Connectbutton.TabIndex = 7;
            this.Connectbutton.Text = "Connect";
            this.Connectbutton.UseVisualStyleBackColor = true;
            this.Connectbutton.Click += new System.EventHandler(this.Connectbutton_Click);
            // 
            // Messagetext
            // 
            this.Messagetext.Location = new System.Drawing.Point(10, 169);
            this.Messagetext.Name = "Messagetext";
            this.Messagetext.Size = new System.Drawing.Size(215, 20);
            this.Messagetext.TabIndex = 6;
            // 
            // Messagelabel
            // 
            this.Messagelabel.AutoSize = true;
            this.Messagelabel.Location = new System.Drawing.Point(8, 150);
            this.Messagelabel.Name = "Messagelabel";
            this.Messagelabel.Size = new System.Drawing.Size(50, 13);
            this.Messagelabel.TabIndex = 7;
            this.Messagelabel.Text = "Message";
            // 
            // Output
            // 
            this.Output.Location = new System.Drawing.Point(10, 237);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.ReadOnly = true;
            this.Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output.Size = new System.Drawing.Size(211, 184);
            this.Output.TabIndex = 8;
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 429);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Messagetext);
            this.Controls.Add(this.Messagelabel);
            this.Controls.Add(this.Connectbutton);
            this.Controls.Add(this.URLtext);
            this.Controls.Add(this.URLlabel);
            this.Controls.Add(this.RoomIDtext);
            this.Controls.Add(this.RoomIDlabel);
            this.Controls.Add(this.ClientIDtext);
            this.Controls.Add(this.ClientIDlabel);
            this.Name = "Client";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ClientIDlabel;
        private System.Windows.Forms.TextBox ClientIDtext;
        private System.Windows.Forms.TextBox RoomIDtext;
        private System.Windows.Forms.Label RoomIDlabel;
        private System.Windows.Forms.TextBox URLtext;
        private System.Windows.Forms.Label URLlabel;
        private System.Windows.Forms.Button Connectbutton;
        private System.Windows.Forms.TextBox Messagetext;
        private System.Windows.Forms.Label Messagelabel;
        private System.Windows.Forms.TextBox Output;
    }
}

