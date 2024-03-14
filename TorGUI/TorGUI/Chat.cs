using System;
using System.Windows.Forms;
using System.Media;
using System.IO; // needed for filing

namespace TorGUI
{
    public partial class Chat : Form
    {
        string friendUsername;
        private string logFileName;

        public Chat(string friendUsername)
        {
            this.friendUsername = friendUsername;
            this.logFileName = @"chat_" + friendUsername + ".log";
            InitializeComponent();
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            Global.enginePipe.send("setDestIp");
            Global.enginePipe.send(friendUsername);
            // Sets Position for the first bubble on the top
            bbl_old.Top = 0 - bbl_old.Height;

            // Load Chat from the log file
            if (File.Exists(logFileName))
            {
                using (StreamReader sr = File.OpenText(logFileName))
                {
                    int i = 0; // to count lines
                    while (sr.Peek() >= 0) // loop till the file ends
                    {                        
                        string[] words = sr.ReadLine().Split('~');
                        if(words[0].Equals(Global.username))
                        {
                            addInMessage(words[1]);
                        }
                        else
                        {
                            addOutMessage(words[1]);
                        }
                        i++;
                    }
                    // scroll to the bottom once finished loading.
                    panel2.VerticalScroll.Value = panel2.VerticalScroll.Maximum;
                    panel2.PerformLayout();
                }
            }
        }

        private void showOutput()
        {
            if (!(string.IsNullOrWhiteSpace(InputTxt.Text))) // Make sure the textbox isnt empty
            {
                Global.enginePipe.send("msg");
                Global.enginePipe.send(InputTxt.Text);
                // Show the user message
                addInMessage(InputTxt.Text);           
                //=========== Creates backup of chat from user and bot to the given location ============
                FileStream fs = new FileStream(logFileName, FileMode.Append, FileAccess.Write);
                if (fs.CanWrite)
                {
                    string log_in = Global.username + "~" + InputTxt.Text;
                    byte[] write = System.Text.Encoding.ASCII.GetBytes(log_in + Environment.NewLine);
                    fs.Write(write, 0, write.Length);
                }
                fs.Flush();
                fs.Close();
                //=======================================================================================

                InputTxt.Text = ""; // Reset textbox
            }
        }

        // Call the Output method when the send button is clicked.
        private void button1_Click(object sender, EventArgs e)
        {
            showOutput();
        }

        // Call the Output method when the enter key is pressed.
        private void InputTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                showOutput();
                e.SuppressKeyPress = true; // Disable windows error sound
            }
        }

        // Call the Output method when the enter key is pressed.
        private void InputTxt_Tilde(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Oemtilde)
            {
               
                MessageBox.Show("You can't type '~'");
                InputTxt.Text = InputTxt.Text.Remove(InputTxt.Text.Length - 1);
                e.SuppressKeyPress = true; // Disable windows error sound
            }
        }
        // Dummy Bubble created to store the previous bubble data.
        bubble bbl_old = new bubble();

        // User Message Bubble Creation
        public void addInMessage(string message)
        {
            // Create new chat bubble
            bubble bbl = new bubble(message, msgtype.In);
            bbl.Location = bubble1.Location; // Set the new bubble location from the bubble sample.
            bbl.Left += 50; // Indent the bubble to the right side.
            bbl.Size = bubble1.Size; // Set the new bubble size from the bubble sample.
            bbl.Top = bbl_old.Bottom + 10; // Position the bubble below the previous one with some extra space.
            
            // Add the new bubble to the panel.
            panel2.Controls.Add(bbl);

            // Force Scroll to the latest bubble
            bbl.Focus();

            // save the last added object to the dummy bubble
            bbl_old = bbl;
        }

        // Message Bubble Creation
        public void addOutMessage(string message)
        {   

            // Create new chat bubble
            bubble bbl = new bubble(message, msgtype.Out);
            bbl.Location = bubble2.Location; // Set the new bubble location from the bubble sample.
            bbl.Size = bubble2.Size; // Set the new bubble size from the bubble sample.
            bbl.Top = bbl_old.Bottom + 10; // Position the bubble below the previous one with some extra space.
            
            // Add the new bubble to the panel.
            panel2.Controls.Add(bbl);

            // Force Scroll to the latest bubble
            bbl.Focus();

            // save the last added object to the dummy bubble
            bbl_old = bbl;
        }

        // Custom close button to close the program when clicked.
        private void close_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        
        // Clear all the bubbles and chat.log
        private void clearChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Delete the log file
            File.Delete(@"chat.log");

            // Clear the chat Bubbles
            panel2.Controls.Clear();

            // This reset the position for the next bubble to come back to the top.
            bbl_old.Top = 0 - bbl_old.Height;
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void recieveMsg()
        {
            string msg = "";
            while(true)
            {
                msg = Global.engineRecievePipe.receive();
                //=========== Creates backup of chat from user and bot to the given location ============
                FileStream fs = new FileStream(logFileName, FileMode.Append, FileAccess.Write);
                if (fs.CanWrite)
                {

                    string log_out = friendUsername + "~" + msg;
                    byte[] write = System.Text.Encoding.ASCII.GetBytes(log_out + Environment.NewLine);
                    fs.Write(write, 0, write.Length);
                }
                fs.Flush();
                fs.Close();
                //=======================================================================================
                addOutMessage(msg);
            }
        }
    }
}