using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace client_code
{
    public partial class userpage : Form
    {
        private TcpClient _client;
        private NetworkStream _netStream;

        public userpage(TcpClient client, NetworkStream netStream)
        {
            InitializeComponent();
            _client = client;
            _netStream = netStream;
        }

        public string _username
        {
            set { usernamelb.Text = usernamelb.Text + value; }
        }


        private void gamecodepanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void newgamebt_Click(object sender, EventArgs e)
        {
            string message = "CREATE|NUL|NUL";
            SendMessageToServer(message);
            string ans = ReceiveMessageFromServer();
            gamecodelb.Text = ans;
            gamecodepanel.Visible = true;

        }

        private void SendMessageToServer(string message)
        {
            if (_client == null || !_client.Connected || _netStream == null || !_netStream.CanWrite)
            {
                Console.WriteLine("[CLIENT] Reconnecting...");
                _client = new TcpClient("127.0.0.1", 8888);
                _netStream = _client.GetStream();
            }
            if (_netStream != null)
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                _netStream.Write(data, 0, data.Length);
                MessageBox.Show(message);
            }
        }


        // Receive a message from the server
        private string ReceiveMessageFromServer()
        {

            if (_netStream != null)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = _netStream.Read(buffer, 0, buffer.Length);
                return Encoding.ASCII.GetString(buffer, 0, bytesRead);
            }
            else
            {
                return string.Empty;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void gamecodeinfolb_Click(object sender, EventArgs e)
        {

        }

        private void gamecodelb_Click(object sender, EventArgs e)
        {

        }

        private void joingamebt_Click(object sender, EventArgs e)
        {
            String gamecode = gamecodetb.Text;
            string message = $"JOIN|{gamecode}|NULL";
            if (gamecode.Length == 6)
            {
                string ans = "";
                while (ans == "")
                {
                    SendMessageToServer(message);
                    ans = ReceiveMessageFromServer();
                }
                MessageBox.Show(ans);
                if (ans == "TRUE")
                {
                    string username = usernamelb.Text;
                    username = username.Substring(6);
                    gamebase mainpage = new gamebase(_client, _netStream, username, gamecode);
                    mainpage.Show();
                    this.Close();
                }
                if (ans == "FALSE")
                {
                    MessageBox.Show("game does not exist");
                }
                if (ans == "FULL")
                {
                    MessageBox.Show("Game is full, please try again later, or alternatively join a different game/ new game");
                }
            }
            else
            {
                MessageBox.Show("invallid gamecode");
            }

        }

        private void gamecodetb_TextChanged(object sender, EventArgs e)
        {

        }

        private void logout_Click(object sender, EventArgs e)
        {
            string username = usernamelb.Text;
            username = username.Substring(6);
            String message = $"LOGOUT|{username}|NUL";
            SendMessageToServer(message);
            string ans = ReceiveMessageFromServer();
            MessageBox.Show(ans);
            startpage mainpage = new startpage(); // instantiates startpage form
            mainpage.Show();  // shows form
            this.Close(); // closes userpage form
        }
    }
}
