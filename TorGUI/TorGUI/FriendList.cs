using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TorGUI
{
    public partial class FriendList : Form
    {
        public FriendList()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                string friendUsername = listBox1.Items[index].ToString();
                string[] words = friendUsername.Split(' '); // Split to username and status
                if (words[1].Contains("Online"))
                {
                    Chat chatForm = new Chat(words[0]);
                    this.Hide();
                    chatForm.Show();
                }
                else
                    MessageBox.Show("You can't chat with offline user");

            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            setFriends();
        }

        private void setFriends()
        {
            Global.enginePipe.send("getFriends");
            string friends = Global.enginePipe.receive(); // Receive friends from backend
            MessageBox.Show(friends);
           
            if(friends != "")
            {
                string[] words = friends.Split(',');

                foreach (var word in words) if (word != "" )// Add all the friends to the listBox
                {
                    string friend = word;
                    if(char.IsWhiteSpace(word, 0)) // Check if first char is space
                    {
                        friend = word.Remove(0, 1); // remove first char
                    }
                    if (friend != "" && friend != "0") // Not adding the last comma because it's empty
                    {
                        char status = friend[friend.Length - 1]; // Get the last chars
                        friend = friend.Remove(friend.Length - 1);
                        if (status == '1')
                            friend += " (Online)";
                        else
                            friend += " (Offline)";

                        listBox1.Items.Add(friend);
                    }


                }
            }
           
        }
        private void FriendList_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Friend list");
            setFriends();
        }

        private void addFriendPic_Click(object sender, EventArgs e)
        {
            if(isFriendUsernameValid())
            {
                Global.enginePipe.send("addFriend");
                Global.enginePipe.send(tbFriendUsername.Text);
                string status = Global.enginePipe.receive();
                if (status == "0")
                {
                    MessageBox.Show("User added succefully");
                    listBox1.Items.Clear();
                    setFriends();
                }
                else if (status == "1")
                {
                    MessageBox.Show("You are already friend of this user");
                }
                else if (status == "2")
                {
                    MessageBox.Show("User not found");
                }
                else if (status == "3")
                {
                    MessageBox.Show("You can not send friend request to yourself");
                }
            }
        }
        private bool isFriendUsernameValid()
        {
            if (tbFriendUsername.Text == "" )
            {
                MessageBox.Show("You must fill the friend's username");
                return false;
            }
            if (tbFriendUsername.Text.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("Username can not contain special characters and spaces");
                return false;
            }
            
            return true;
        }
    }
}
