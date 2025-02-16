namespace client_code
{
    partial class userpage
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
            usernamelb = new Label();
            newgamebt = new Button();
            joingamebt = new Button();
            historybt = new Button();
            gamecodetb = new TextBox();
            gamecodepanel = new Panel();
            gamecodelb = new Label();
            gamecodeinfolb = new Label();
            logout = new Button();
            gamecodepanel.SuspendLayout();
            SuspendLayout();
            // 
            // usernamelb
            // 
            usernamelb.AutoSize = true;
            usernamelb.Font = new Font("Comic Sans MS", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            usernamelb.ForeColor = Color.RosyBrown;
            usernamelb.Location = new Point(615, 230);
            usernamelb.Name = "usernamelb";
            usernamelb.Size = new Size(61, 23);
            usernamelb.TabIndex = 0;
            usernamelb.Text = "User: ";
            // 
            // newgamebt
            // 
            newgamebt.FlatStyle = FlatStyle.Popup;
            newgamebt.Font = new Font("Comic Sans MS", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            newgamebt.ForeColor = Color.RosyBrown;
            newgamebt.Location = new Point(615, 279);
            newgamebt.Name = "newgamebt";
            newgamebt.Size = new Size(117, 23);
            newgamebt.TabIndex = 1;
            newgamebt.Text = "New Game";
            newgamebt.UseVisualStyleBackColor = true;
            newgamebt.Click += newgamebt_Click;
            // 
            // joingamebt
            // 
            joingamebt.FlatStyle = FlatStyle.Popup;
            joingamebt.Font = new Font("Comic Sans MS", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            joingamebt.ForeColor = Color.RosyBrown;
            joingamebt.Location = new Point(615, 328);
            joingamebt.Name = "joingamebt";
            joingamebt.Size = new Size(117, 23);
            joingamebt.TabIndex = 2;
            joingamebt.Text = "Join Game";
            joingamebt.UseVisualStyleBackColor = true;
            joingamebt.Click += joingamebt_Click;
            // 
            // historybt
            // 
            historybt.FlatStyle = FlatStyle.Popup;
            historybt.Font = new Font("Comic Sans MS", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            historybt.ForeColor = Color.RosyBrown;
            historybt.Location = new Point(769, 279);
            historybt.Name = "historybt";
            historybt.Size = new Size(117, 23);
            historybt.TabIndex = 3;
            historybt.Text = "Game History";
            historybt.UseVisualStyleBackColor = true;
            // 
            // gamecodetb
            // 
            gamecodetb.Font = new Font("Comic Sans MS", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            gamecodetb.Location = new Point(769, 327);
            gamecodetb.Name = "gamecodetb";
            gamecodetb.Size = new Size(117, 24);
            gamecodetb.TabIndex = 4;
            gamecodetb.Text = "Enter Game Code:";
            gamecodetb.TextChanged += gamecodetb_TextChanged;
            // 
            // gamecodepanel
            // 
            gamecodepanel.BackColor = Color.Gainsboro;
            gamecodepanel.Controls.Add(gamecodelb);
            gamecodepanel.Controls.Add(gamecodeinfolb);
            gamecodepanel.Location = new Point(607, 397);
            gamecodepanel.Name = "gamecodepanel";
            gamecodepanel.Size = new Size(279, 63);
            gamecodepanel.TabIndex = 5;
            gamecodepanel.Visible = false;
            gamecodepanel.Paint += gamecodepanel_Paint;
            // 
            // gamecodelb
            // 
            gamecodelb.BackColor = Color.White;
            gamecodelb.Location = new Point(162, 23);
            gamecodelb.Name = "gamecodelb";
            gamecodelb.Size = new Size(100, 16);
            gamecodelb.TabIndex = 1;
            gamecodelb.Click += gamecodelb_Click;
            // 
            // gamecodeinfolb
            // 
            gamecodeinfolb.AutoSize = true;
            gamecodeinfolb.BackColor = Color.Transparent;
            gamecodeinfolb.Font = new Font("Comic Sans MS", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            gamecodeinfolb.ForeColor = Color.RosyBrown;
            gamecodeinfolb.Location = new Point(8, 22);
            gamecodeinfolb.Name = "gamecodeinfolb";
            gamecodeinfolb.Size = new Size(141, 16);
            gamecodeinfolb.TabIndex = 0;
            gamecodeinfolb.Text = "Game Code Generated:";
            gamecodeinfolb.Click += gamecodeinfolb_Click;
            // 
            // logout
            // 
            logout.BackColor = Color.Transparent;
            logout.FlatStyle = FlatStyle.Popup;
            logout.Font = new Font("Comic Sans MS", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            logout.ForeColor = Color.Firebrick;
            logout.Location = new Point(686, 195);
            logout.Name = "logout";
            logout.Size = new Size(131, 23);
            logout.TabIndex = 6;
            logout.Text = "Log Out";
            logout.UseVisualStyleBackColor = false;
            logout.Click += logout_Click;
            // 
            // userpage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1495, 709);
            Controls.Add(logout);
            Controls.Add(gamecodepanel);
            Controls.Add(gamecodetb);
            Controls.Add(historybt);
            Controls.Add(joingamebt);
            Controls.Add(newgamebt);
            Controls.Add(usernamelb);
            Name = "userpage";
            Text = "userpage";
            gamecodepanel.ResumeLayout(false);
            gamecodepanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label usernamelb;
        private Button creategamebt;
        private Button newgamebt;
        private Button joingamebt;
        private Button historybt;
        private TextBox gamecodetb;
        private Panel gamecodepanel;
        private Label gamecodeinfolb;
        private Label gamecodelb;
        private Button logout;
    }
}