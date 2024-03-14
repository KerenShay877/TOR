using System.Windows.Forms;

namespace TorGUI
{
    partial class FriendList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FriendList));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.reloadPic = new System.Windows.Forms.PictureBox();
            this.addFriendPic = new System.Windows.Forms.PictureBox();
            this.tbFriendUsername = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.reloadPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addFriendPic)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(234, 304);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // reloadPic
            // 
            this.reloadPic.Image = ((System.Drawing.Image)(resources.GetObject("reloadPic.Image")));
            this.reloadPic.Location = new System.Drawing.Point(217, 320);
            this.reloadPic.Name = "reloadPic";
            this.reloadPic.Size = new System.Drawing.Size(29, 26);
            this.reloadPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.reloadPic.TabIndex = 1;
            this.reloadPic.TabStop = false;
            this.reloadPic.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // addFriendPic
            // 
            this.addFriendPic.Image = ((System.Drawing.Image)(resources.GetObject("addFriendPic.Image")));
            this.addFriendPic.Location = new System.Drawing.Point(12, 324);
            this.addFriendPic.Name = "addFriendPic";
            this.addFriendPic.Size = new System.Drawing.Size(62, 50);
            this.addFriendPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.addFriendPic.TabIndex = 2;
            this.addFriendPic.TabStop = false;
            this.addFriendPic.Click += new System.EventHandler(this.addFriendPic_Click);
            // 
            // tbFriendUsername
            // 
            this.tbFriendUsername.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.tbFriendUsername.Location = new System.Drawing.Point(80, 345);
            this.tbFriendUsername.Name = "tbFriendUsername";
            this.tbFriendUsername.Size = new System.Drawing.Size(166, 29);
            this.tbFriendUsername.TabIndex = 3;
            // 
            // FriendList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 386);
            this.Controls.Add(this.tbFriendUsername);
            this.Controls.Add(this.addFriendPic);
            this.Controls.Add(this.reloadPic);
            this.Controls.Add(this.listBox1);
            this.Name = "FriendList";
            this.Text = "FriendList";
            this.Load += new System.EventHandler(this.FriendList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.reloadPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addFriendPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.PictureBox reloadPic;
        private PictureBox addFriendPic;
        private TextBox tbFriendUsername;
    }
}