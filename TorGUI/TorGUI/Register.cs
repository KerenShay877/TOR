using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace TorGUI
{
    public partial class Register : Form
    {
        
        public Register()
        {
            InitializeComponent();
        }

        private void picLogoTor_Click(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if(allFieldCorrect())
            {
                Global.enginePipe.send("Register");
                Global.enginePipe.send(tbUsername.Text);
                Global.enginePipe.send(tbPassword.Text);
                Global.enginePipe.send(tbEmail.Text);
                string status = Global.enginePipe.receive();
                if (status == "1")
                {
                    FriendList friendListForm = new FriendList();
                    this.Hide();
                    friendListForm.Show();
                }
                else if (status == "2")
                    MessageBox.Show("Username already exists!");
                else if (status == "3")
                    MessageBox.Show("Email already exists!");
                else
                    MessageBox.Show("unknown error occurred");
            }

        }

        private void tbUsername_TextChanged(object sender, EventArgs e)
        {}

        private void Register_Load(object sender, EventArgs e)
        {}

        private bool allFieldCorrect()
        {

            if(tbUsername.Text == "" || tbPassword.Text == "" || tbEmail.Text == "")
            {
                MessageBox.Show("You must fill all the fields");
                return false;
            }
            if(tbUsername.Text == tbPassword.Text)
            {
                MessageBox.Show("Username and password can not be same");
                return false;
            }
            if (tbUsername.Text.Any(ch => !Char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("Username can not contain special characters and spaces");
                return false;
            }
            if(tbPassword.Text.Contains('(') || tbPassword.Text.Contains(')'))
            {
                MessageBox.Show("Password can not contain '(' or ')'");
                return false;
            }
            if(tbPassword.Text.Contains(' '))
            {
                MessageBox.Show("Password can not contain spaces");
                return false;
            }
            if(!IsValidEmail(tbEmail.Text))
            {
                MessageBox.Show("Bad email format");
                return false;
            }
            return true;
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}


