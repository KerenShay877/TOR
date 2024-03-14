namespace TorGUI
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblSignUp = new System.Windows.Forms.LinkLabel();
            this.picLogoTor = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoTor)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Casual", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.Teal;
            this.btnLogin.Location = new System.Drawing.Point(56, 269);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(114, 59);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbUsername
            // 
            this.tbUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbUsername.ForeColor = System.Drawing.Color.Black;
            this.tbUsername.Location = new System.Drawing.Point(103, 150);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(114, 20);
            this.tbUsername.TabIndex = 4;
            // 
            // tbPassword
            // 
            this.tbPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPassword.Location = new System.Drawing.Point(103, 192);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(114, 20);
            this.tbPassword.TabIndex = 5;
            this.tbPassword.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Casual", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsername.ForeColor = System.Drawing.Color.Teal;
            this.lblUsername.Location = new System.Drawing.Point(10, 148);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(83, 19);
            this.lblUsername.TabIndex = 6;
            this.lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Casual", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPassword.ForeColor = System.Drawing.Color.Teal;
            this.lblPassword.Location = new System.Drawing.Point(10, 190);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(81, 19);
            this.lblPassword.TabIndex = 7;
            this.lblPassword.Text = "Password";
            // 
            // lblSignUp
            // 
            this.lblSignUp.ActiveLinkColor = System.Drawing.Color.Teal;
            this.lblSignUp.AutoSize = true;
            this.lblSignUp.Font = new System.Drawing.Font("Casual", 9.25F);
            this.lblSignUp.LinkColor = System.Drawing.Color.Teal;
            this.lblSignUp.Location = new System.Drawing.Point(9, 224);
            this.lblSignUp.Name = "lblSignUp";
            this.lblSignUp.Size = new System.Drawing.Size(198, 16);
            this.lblSignUp.TabIndex = 8;
            this.lblSignUp.TabStop = true;
            this.lblSignUp.Text = "Don\'t have an account? Sign up";
            this.lblSignUp.VisitedLinkColor = System.Drawing.Color.Teal;
            this.lblSignUp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblSignUp_LinkClicked);
            // 
            // picLogoTor
            // 
            this.picLogoTor.BackColor = System.Drawing.Color.Transparent;
            this.picLogoTor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picLogoTor.Image = global::TorGUI.Properties.Resources.tor3;
            this.picLogoTor.Location = new System.Drawing.Point(12, 12);
            this.picLogoTor.Name = "picLogoTor";
            this.picLogoTor.Size = new System.Drawing.Size(205, 140);
            this.picLogoTor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogoTor.TabIndex = 3;
            this.picLogoTor.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(233, 345);
            this.Controls.Add(this.lblSignUp);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUsername);
            this.Controls.Add(this.picLogoTor);
            this.Controls.Add(this.btnLogin);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Tor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLogoTor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.PictureBox picLogoTor;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.LinkLabel lblSignUp;
    }
}

