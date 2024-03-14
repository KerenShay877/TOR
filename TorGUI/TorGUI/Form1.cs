using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TorGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void initForm()
        {
            Global.enginePipe.connect();
            Global.engineRecievePipe.connect();
            Invoke((MethodInvoker)delegate
            {
                // string s = enginePipe.getEngineMessage();
                // MessageBox.Show(s);
            });
        }

        Thread connectionThread;
        private void Form1_Load(object sender, EventArgs e)
        {
            Global.enginePipe = new pipe();
            Global.engineRecievePipe = new recievePipe();
            //this.Show();

            //MessageBox.Show("Press OK to start waiting for engine to connect...");
            connectionThread = new Thread(initForm);
            connectionThread.Start();
            connectionThread.IsBackground = true;
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Global.enginePipe.close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        

        private void lblSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register registerForm = new Register();
            this.Hide();
            registerForm.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            
            if (allFieldValid())
            {
                Global.enginePipe.send("Login");
                Global.enginePipe.send(tbUsername.Text);
                Global.enginePipe.send(tbPassword.Text);
                string status = Global.enginePipe.receive();
                if (status == "1")
                {
                    Global.username = tbUsername.Text;
                    FriendList friendListForm = new FriendList();
                    this.Hide();
                    friendListForm.Show();

                }
                else if (status == "2")
                    MessageBox.Show("The username or password is incorrect");
                else
                    MessageBox.Show("unknown error occurred");
            }
            
        }
        private bool allFieldValid()
        {

            if (tbUsername.Text == "" || tbPassword.Text == "")
            {
                MessageBox.Show("You must fill all the fields");
                return false;
            }
            if (tbUsername.Text.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("Username can not contain special characters and spaces");
                return false;
            }
            if (tbPassword.Text.Contains('(') || tbPassword.Text.Contains(')'))
            {
                MessageBox.Show("Password can not contain '(' or ')'");
                return false;
            }
            if(tbPassword.Text.Contains(' '))
            {
                MessageBox.Show("Password can not contain spaces");
                return false;
            }
            return true;
        }



    }
}
