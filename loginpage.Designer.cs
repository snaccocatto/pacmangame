namespace client_code
{
    partial class startpage
    {
        ///  Required designer variables and method for designer support below - note to self to not modify this!!
        private System.ComponentModel.IContainer components = null;
        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(startpage));
            welcomelb = new Label();
            loginbt = new Button();
            createaccountbt = new Button();
            usernametb = new TextBox();
            passwordtb = new TextBox();
            startconnectionbt = new Button();
            connectionstatuslb = new Label();
            passwordlb = new Label();
            Captcha = new Panel();
            captchainfo = new Label();
            captchaimage4 = new PictureBox();
            captchaimage3 = new PictureBox();
            captchaimage2 = new PictureBox();
            captchaimage1 = new PictureBox();
            captchalb = new Label();
            ServerIPtb = new TextBox();
            Captcha.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)captchaimage4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)captchaimage3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)captchaimage2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)captchaimage1).BeginInit();
            SuspendLayout();
            // 
            // welcomelb
            // 
            welcomelb.AutoSize = true;
            welcomelb.Font = new Font("Comic Sans MS", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            welcomelb.Location = new Point(709, 230);
            welcomelb.Name = "welcomelb";
            welcomelb.Size = new Size(53, 16);
            welcomelb.TabIndex = 0;
            welcomelb.Text = "welcome";
            // 
            // loginbt
            // 
            loginbt.FlatStyle = FlatStyle.Popup;
            loginbt.Font = new Font("Comic Sans MS", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            loginbt.ForeColor = Color.RosyBrown;
            loginbt.Location = new Point(686, 417);
            loginbt.Name = "loginbt";
            loginbt.Size = new Size(100, 23);
            loginbt.TabIndex = 1;
            loginbt.Text = "log in";
            loginbt.UseVisualStyleBackColor = true;
            loginbt.Click += loginbt_Click;
            // 
            // createaccountbt
            // 
            createaccountbt.BackColor = SystemColors.Control;
            createaccountbt.FlatStyle = FlatStyle.Popup;
            createaccountbt.Font = new Font("Comic Sans MS", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            createaccountbt.ForeColor = Color.RosyBrown;
            createaccountbt.Location = new Point(672, 456);
            createaccountbt.Name = "createaccountbt";
            createaccountbt.Size = new Size(133, 23);
            createaccountbt.TabIndex = 2;
            createaccountbt.Text = "create account";
            createaccountbt.UseVisualStyleBackColor = false;
            createaccountbt.Click += createaccountbt_Click;
            // 
            // usernametb
            // 
            usernametb.Location = new Point(686, 258);
            usernametb.Name = "usernametb";
            usernametb.Size = new Size(100, 23);
            usernametb.TabIndex = 3;
            usernametb.Text = "username:";
            // 
            // passwordtb
            // 
            passwordtb.Location = new Point(686, 299);
            passwordtb.Name = "passwordtb";
            passwordtb.Size = new Size(100, 23);
            passwordtb.TabIndex = 4;
            passwordtb.Text = "password:";
            // 
            // startconnectionbt
            // 
            startconnectionbt.BackColor = Color.Transparent;
            startconnectionbt.Cursor = Cursors.AppStarting;
            startconnectionbt.FlatStyle = FlatStyle.Popup;
            startconnectionbt.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            startconnectionbt.ForeColor = Color.RosyBrown;
            startconnectionbt.Location = new Point(643, 150);
            startconnectionbt.Name = "startconnectionbt";
            startconnectionbt.Size = new Size(180, 37);
            startconnectionbt.TabIndex = 6;
            startconnectionbt.Text = "Start Connection";
            startconnectionbt.UseVisualStyleBackColor = false;
            startconnectionbt.Click += startconnectionbt_Click;
            // 
            // connectionstatuslb
            // 
            connectionstatuslb.AutoSize = true;
            connectionstatuslb.Font = new Font("Comic Sans MS", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            connectionstatuslb.ForeColor = Color.RosyBrown;
            connectionstatuslb.Location = new Point(686, 199);
            connectionstatuslb.Name = "connectionstatuslb";
            connectionstatuslb.RightToLeft = RightToLeft.No;
            connectionstatuslb.Size = new Size(91, 19);
            connectionstatuslb.TabIndex = 7;
            connectionstatuslb.Text = "Disconnected";
            // 
            // passwordlb
            // 
            passwordlb.AutoSize = true;
            passwordlb.Font = new Font("Comic Sans MS", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            passwordlb.Location = new Point(569, 337);
            passwordlb.Name = "passwordlb";
            passwordlb.Size = new Size(334, 60);
            passwordlb.TabIndex = 8;
            passwordlb.Text = resources.GetString("passwordlb.Text");
            passwordlb.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Captcha
            // 
            Captcha.BackColor = Color.Gainsboro;
            Captcha.Controls.Add(captchainfo);
            Captcha.Controls.Add(captchaimage4);
            Captcha.Controls.Add(captchaimage3);
            Captcha.Controls.Add(captchaimage2);
            Captcha.Controls.Add(captchaimage1);
            Captcha.Controls.Add(captchalb);
            Captcha.Location = new Point(569, 499);
            Captcha.Name = "Captcha";
            Captcha.Size = new Size(313, 202);
            Captcha.TabIndex = 9;
            Captcha.Visible = false;
            Captcha.Paint += Captcha_Paint;
            // 
            // captchainfo
            // 
            captchainfo.AutoSize = true;
            captchainfo.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            captchainfo.Location = new Point(197, 48);
            captchainfo.Name = "captchainfo";
            captchainfo.Size = new Size(100, 51);
            captchainfo.TabIndex = 5;
            captchainfo.Text = "please select \r\nthe blue \r\ncoloured image :)";
            captchainfo.Click += captchainfo_Click;
            // 
            // captchaimage4
            // 
            captchaimage4.Location = new Point(101, 101);
            captchaimage4.Name = "captchaimage4";
            captchaimage4.Size = new Size(79, 50);
            captchaimage4.TabIndex = 4;
            captchaimage4.TabStop = false;
            captchaimage4.Click += captchaimage4_Click;
            // 
            // captchaimage3
            // 
            captchaimage3.Location = new Point(16, 101);
            captchaimage3.Name = "captchaimage3";
            captchaimage3.Size = new Size(79, 50);
            captchaimage3.TabIndex = 3;
            captchaimage3.TabStop = false;
            captchaimage3.Click += captchaimage3_Click;
            // 
            // captchaimage2
            // 
            captchaimage2.Location = new Point(101, 48);
            captchaimage2.Name = "captchaimage2";
            captchaimage2.Size = new Size(79, 47);
            captchaimage2.TabIndex = 2;
            captchaimage2.TabStop = false;
            captchaimage2.Click += captchaimage2_Click;
            // 
            // captchaimage1
            // 
            captchaimage1.Location = new Point(16, 48);
            captchaimage1.Name = "captchaimage1";
            captchaimage1.Size = new Size(79, 47);
            captchaimage1.TabIndex = 1;
            captchaimage1.TabStop = false;
            captchaimage1.Click += captchaimage1_Click;
            // 
            // captchalb
            // 
            captchalb.AutoSize = true;
            captchalb.BackColor = Color.Transparent;
            captchalb.Font = new Font("Comic Sans MS", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            captchalb.ForeColor = Color.RosyBrown;
            captchalb.Location = new Point(14, 11);
            captchalb.Name = "captchalb";
            captchalb.Size = new Size(121, 16);
            captchalb.TabIndex = 0;
            captchalb.Text = "Human Verification:";
            captchalb.Click += captchalb_Click;
            // 
            // ServerIPtb
            // 
            ServerIPtb.Location = new Point(664, 121);
            ServerIPtb.Name = "ServerIPtb";
            ServerIPtb.Size = new Size(141, 23);
            ServerIPtb.TabIndex = 10;
            ServerIPtb.Text = "Enter the Server's IP:";
            // 
            // startpage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1481, 713);
            Controls.Add(ServerIPtb);
            Controls.Add(Captcha);
            Controls.Add(passwordlb);
            Controls.Add(connectionstatuslb);
            Controls.Add(startconnectionbt);
            Controls.Add(passwordtb);
            Controls.Add(usernametb);
            Controls.Add(createaccountbt);
            Controls.Add(loginbt);
            Controls.Add(welcomelb);
            ForeColor = Color.RosyBrown;
            Name = "startpage";
            Text = "loginpage";
            Captcha.ResumeLayout(false);
            Captcha.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)captchaimage4).EndInit();
            ((System.ComponentModel.ISupportInitialize)captchaimage3).EndInit();
            ((System.ComponentModel.ISupportInitialize)captchaimage2).EndInit();
            ((System.ComponentModel.ISupportInitialize)captchaimage1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }


        #endregion

        private Label welcomelb;
        private Button loginbt;
        private Button createaccountbt;
        private TextBox usernametb;
        private TextBox passwordtb;
        private Button startconnectionbt;
        private Label connectionstatuslb;
        private Label passwordlb;
        private Panel Captcha;
        private Label captchalb;
        private PictureBox captchaimage2;
        private PictureBox captchaimage1;
        private PictureBox captchaimage3;
        private Label captchainfo;
        private PictureBox captchaimage4;
        private TextBox ServerIPtb;
    }
}
