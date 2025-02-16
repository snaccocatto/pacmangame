using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static client_code.gamebase;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;


namespace client_code
{

    public partial class gamebase : Form
    {
        private TcpClient _client;
        private NetworkStream _netStream;
        public string _username;
        public string _gamecode;
        public bool fastghost = false;

        private MovablePictureBox MovingPacman;

        string userrole;

        //public class MovingGhosts : MoveableSprites

        //public class EnemyPacman : MoveableSprites



        public gamebase(TcpClient client, NetworkStream netStream, string username, string gamecode)
        {
            InitializeComponent();
            _client = client;
            _netStream = netStream;
            _username = username;
            _gamecode = gamecode;
            Task.Run(() => ListenToServer()); //this ensures loop is constantly listening to server in the background
            string message = $"MAKE|{gamecode}|NULL";
            SendMessageToServer(message);

            MovingPacman = new MovablePictureBox(Pacman, GetWalls); //Assigning of oop - for pacman


        }

        private IEnumerable<Control> GetWalls() //creates a method called GetWalls, IEnumerable allows each wall to be collected one at a time, where control specifies each wall.
        {
            return this.Controls.Cast<Control>()
                .Where(wall => wall.Name.StartsWith("M") || wall.Name.Equals("leftwall") || wall.Name.Equals("topwall") || wall.Name.Equals("bottomwall") || wall.Name.Equals("rightwall"));
            //for every instance of these controlls, it will be called wall
        }
        //this basically just gets all the walls and returns them, it converts them into an enumerable collection of objects


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            string message;
            switch (keyData)
            {
                case Keys.Up:
                    MovingPacman.Move(MovablePictureBox.Direction.Up);
                    message = MovingPacman.Move(MovablePictureBox.Direction.Up);
                    message = $"{message}|{userrole}|{_gamecode}";
                    SendMessageToServer(message);
                    return true;
                case Keys.Down:
                    MovingPacman.Move(MovablePictureBox.Direction.Down);
                    message = MovingPacman.Move(MovablePictureBox.Direction.Down);
                    message = $"{message}|{userrole}|{_gamecode}";
                    SendMessageToServer(message);
                    return true;
                case Keys.Left:
                    MovingPacman.Move(MovablePictureBox.Direction.Left);
                    message = MovingPacman.Move(MovablePictureBox.Direction.Left);
                    message = $"{message}|{userrole}|{_gamecode}";
                    SendMessageToServer(message);
                    return true;
                case Keys.Right:
                    MovingPacman.Move(MovablePictureBox.Direction.Right);
                    message = MovingPacman.Move(MovablePictureBox.Direction.Right);
                    message = $"{message}|{userrole}|{_gamecode}";
                    SendMessageToServer(message);
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public class BaseMovable
        {
            public enum Direction //needs to declare the direction 
            {
                Up,
                Down,
                Left,
                Right
            }

        }
        public class MovablePictureBox : BaseMovable
        {
            private PictureBox pacman;
            private int stepSize;
            private Image originalImage;
            public Func<IEnumerable<Control>> getWalls;


            public MovablePictureBox(PictureBox pacman, Func<IEnumerable<Control>> GetWalls, int StepSize = 10)
            {
                this.pacman = pacman ?? throw new ArgumentNullException(nameof(pacman));
                this.stepSize = StepSize;
                this.getWalls = GetWalls ?? throw new ArgumentNullException(nameof(GetWalls));

                originalImage = (Image)pacman.Image.Clone();
            }

            public string Move(Direction direction)
            {

                int newX = pacman.Left;
                int newY = pacman.Top;
                string message = "";

                switch (direction)
                {
                    case Direction.Up:
                        pacman.Image = Properties.Resources.UP_YELLOW;
                        newY -= stepSize;
                        message = $"MOVE|UP|{newX}|{newY}";
                        break;
                    case Direction.Down:
                        pacman.Image = Properties.Resources.DOWN_YELLOW;
                        newY += stepSize;
                        message = $"MOVE|DOWN|{newX}|{newY}";
                        break;
                    case Direction.Left:
                        pacman.Image = Properties.Resources.LEFT_YELLOW;
                        newX -= stepSize;
                        message = $"MOVE|LEFT|{newX}|{newY}";
                        break;
                    case Direction.Right:
                        pacman.Image = Properties.Resources.RIGHT_YELLOW;
                        newX += stepSize;
                        message = $"MOVE|RIGHT|{newX}|{newY}";
                        break;
                }


                //checks fora collision at the new position
                if (!IsCollidingWithWall(newX, newY))
                {
                    pacman.Left = newX;
                    pacman.Top = newY;
                }

                pacman.Refresh();
                return message;


            }

            private bool IsCollidingWithWall(int newX, int newY)
            {
                Rectangle futureBounds = new Rectangle(newX, newY, pacman.Width, pacman.Height);

                var walls = getWalls();

                foreach (Control wall in walls)
                {
                    if (futureBounds.IntersectsWith(wall.Bounds))
                    {
                        return true;
                    }
                }

                return false;
            }
        }





        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ListenToServer()
        {
            //could be improved via exception handling. 
            string response = string.Empty;
            while (true)
            {
                if (_netStream != null)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = _netStream.Read(buffer, 0, buffer.Length);
                    response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                }

                string[] responseparts = response.Split('|'); //creates an array of the parts of the response
                //same method that is used in server side code
                string action = responseparts[0];
                string taskone = responseparts[1];
                string tasktwo = responseparts[2];

                if (action == "ASSIGN")
                {
                    MessageBox.Show("you are " + taskone);
                    userrole = taskone;

                    if (taskone == "PLAYER1")
                    {
                        Pacman.Invoke(new Action(() => Pacman.Location = new Point(50, 50))); //sets the start point of pacman
                        opponant.Invoke(new Action(() => opponant.Location = new Point(550, 50))); //sets the start point of other player
                    }
                    if (taskone == "PLAYER2")
                    {
                        Pacman.Invoke(new Action(() => Pacman.Location = new Point(550, 50))); //sets the start point of pacman
                        opponant.Invoke(new Action(() => opponant.Location = new Point(50, 50))); //sets the start point of the other player
                    }

                }
                if (action == "WALL")
                {
                    string wallnum = responseparts[3];
                    BuildWall(wallnum, taskone, tasktwo);

                }
                if (action == "MOVE")
                {
                    string taskthree = responseparts[3];
                    string taskfour = responseparts[4];
                    MoveOtherPlayer(taskone, tasktwo, taskthree, taskfour);
                }
                if (action == "COLLIDE")
                {
                    ProcessCollission();
                }
                if (action == "GHOSTPOS")
                {
                    string gamecode = responseparts[1];
                    string[] position = responseparts[2].Split(',');

                    int ghostX = int.Parse(position[0]);
                    int ghostY = int.Parse(position[1]);

                    // Update ghost position in the game UI
                    UpdateGhostPosition(ghostX, ghostY);
                }
            }
        }

        void UpdateGhostPosition(int x, int y)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateGhostPosition(x, y)));
                return;
            }

            Ghost1.Location = new Point(x, y);
        }


        private void MoveOtherPlayer(string direction, string xcoord, string ycoord, string Player)
        {
            int newx = Convert.ToInt32(xcoord);
            int newy = Convert.ToInt32(ycoord);

            if (direction == "UP")
            {
                opponant.Image = Properties.Resources.UP_YELLOW;
                opponant.Invoke(new Action(() => opponant.Location = new Point(newx, newy)));
                CheckPacmanCollission();
            }
            if (direction == "DOWN")
            {
                opponant.Image = Properties.Resources.DOWN_YELLOW;
                opponant.Invoke(new Action(() => opponant.Location = new Point(newx, newy)));
                CheckPacmanCollission();
            }
            if (direction == "RIGHT")
            {
                opponant.Image = Properties.Resources.RIGHT_YELLOW;
                opponant.Invoke(new Action(() => opponant.Location = new Point(newx, newy)));
                CheckPacmanCollission();
            }
            if (direction == "LEFT")
            {
                opponant.Image = Properties.Resources.LEFT_YELLOW;
                opponant.Invoke(new Action(() => opponant.Location = new Point(newx, newy)));
                CheckPacmanCollission();
            }
        }

        private void CheckPacmanCollission()
        {
            if (Pacman.Bounds.IntersectsWith(opponant.Bounds))
            {
                string message = $"NULL|NULL|NULL";
                if (userrole == "PLAYER1")
                {
                    MessageBox.Show("you have won!");
                    message = $"COLLIDE|1|{_gamecode}";
                }
                else
                {
                    MessageBox.Show("you have lost!");
                    message = $"COLLIDE|0|{_gamecode}";
                }
                //message has use of 0 and 1 as it would be index of hostgame in server
                //they are reversed as it needs to be sent to other player, eg player1 sends to 1, player2 = 1              
                SendMessageToServer(message);
            }
        }

        private void ProcessCollission()
        {
            if (userrole == "PLAYER1")
            {
                MessageBox.Show("you have won!");
            }
            else
            {
                MessageBox.Show("you have lost!");
            }
        }
        private void BuildWall(string i, string taskone, string tasktwo)
        {

            int xco = Convert.ToInt32(taskone);
            int yco = Convert.ToInt32(tasktwo);
            var control = this.Controls["M" + i]; //dynamically gets the block we're moving bam
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => control.Location = new Point(xco, yco)));
            }
            else
            {
                control.Location = new Point(xco, yco);
            }
            string message = "BUILT";
            SendMessageToServer(message);
        }


        private void SendMessageToServer(string message)
        {
            if (_netStream != null)
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                _netStream.Write(data, 0, data.Length);
            }
        }

        private void M0_Click(object sender, EventArgs e)
        {

        } //wanted to remove these as they were unnecissary but it caused errors

        private void M10_Click(object sender, EventArgs e)
        {

        }

        private void Pacman_Click(object sender, EventArgs e)
        {

        }

        private void Cover_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chooseghost_Click(object sender, EventArgs e)
        {

        }

        private void ghostchoicetb_Click(object sender, EventArgs e)
        {

        }




        private void pinkghostbt_Click(object sender, EventArgs e)
        {
            string message = $"START|{_gamecode}|{userrole}";
            SendMessageToServer(message);
        }

        private void blueghostbt_Click(object sender, EventArgs e)
        {
            
        }

        private void Ghost_LocationChanged(object sender, EventArgs e)
        {
           
        }
    }
}

