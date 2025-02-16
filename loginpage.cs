using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using Newtonsoft.Json.Linq;

namespace client_code
{
    public partial class startpage : Form
    {
        private TcpClient _client;
        private NetworkStream _netStream;
        private int correctindex;
        private const string AccessKey = "N9JTx1BAXrw6flhnPIxBeyK7GVR6-YcmLAhiT84mdwU"; //access key for API used in captcha generation 
        int captchaimageposition;
        public startpage()
        {
            InitializeComponent();
            connectionstatuslb.Text = "Disconnected";
            passwordtb.PasswordChar = '*'; //makes password entered appear as asteriks for better security.

        }

        //loads the captcha images for the captcha panel
        private async void LoadCaptchaImages() //async used here because nothing is being returned and then doesnt interrupt main thread
        {
            string[] allquerys = { "", "", "", "" };
            Random rnd = new Random();
            correctindex = rnd.Next(0, 4);
            PictureBox[] captchaimages = { captchaimage1, captchaimage2, captchaimage3, captchaimage4 };
            for (int count = 0; count < 4; count++)
            {
                if (count != correctindex)
                {
                    allquerys[count] = ("pink");
                }
                else
                {
                    allquerys[count] = ("blue");
                    captchaimageposition = count;
                }
            }
            //the above loop randomly positions the 'valid' query in the array of all querys
            for (int count = 0; count < captchaimages.Length; count++)
            {
                string imageurl = await GetImageUrl(allquerys[count]); //await used to pause program execution until this has occured 
                if (imageurl != null)
                {
                    using (HttpClient client = new HttpClient())
                    using (var response = await client.GetStreamAsync(imageurl))
                    {
                        captchaimages[count].Image = Image.FromStream(response);
                    }
                }
            }

        }

        private async Task<string> GetImageUrl(string query) //returns a Task object that holds a string
        {

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Client-ID {AccessKey}");
                string Url = $"https://api.unsplash.com/search/photos?query={query}&per_page=1"; //ensures only one image matching query is returned

                HttpResponseMessage response = await client.GetAsync(Url);
                if (response.IsSuccessStatusCode) //refers to response from server to see if request has been sucessfully processed
                {
                    string jsonresponse = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(jsonresponse);

                    // Ensure that the 'results' field exists and has items
                    if (json["results"] != null && json["results"].Any())
                    {
                        return json["results"][0]["urls"]["regular"].ToString();
                        //return line explanation = accesses results array from json response, 
                    }
                }
                return null;
            }
        }

        // Handle Start Connection button click
        private void startconnectionbt_Click(object sender, EventArgs e)
        {
            string serverIP = ServerIPtb.Text;

            MessageBox.Show("Attempting to connect to " + serverIP);

            try
            {
                _client = new TcpClient(serverIP, 8888);
                _netStream = _client.GetStream();
                connectionstatuslb.Text = "Connected";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message);
                connectionstatuslb.Text = "Disconnected";
            }
        }

        // Handle Login button click
        private void loginbt_Click(object sender, EventArgs e)
        {
            if (_client != null && _client.Connected)
            {
                //execute captcha first.
                LoadCaptchaImages();
                Captcha.Visible = true;


            }
            else
            {
                MessageBox.Show("Not connected to the server.");
            }
        }


        private void ProceedWithLogin()
        {
            // Get the username and password from the textboxes
            string username = usernametb.Text;
            string password = passwordtb.Text;

            // Create the message to send to the server in the format "LOGIN|username|password"
            string message = $"LOGIN|{username}|{password}";
            SendMessageToServer(message);

            // Receive and display the response from the server
            string response = ReceiveMessageFromServer();
            MessageBox.Show(response);
            if (response.Contains("SUCCESS"))
            {
                userpage mainPage = new userpage(_client, _netStream); // instantiates userpage form
                mainPage.Show();  // shows form
                mainPage._username = username;
                this.Hide();  // hide the current form (startpage)
                //mainPage.FormClosed += (s, args) => this.Close(); //closes startpage i might need to change this since 
                                                                  //data transfer needs to occur before start page is closed.
            }
        }

        // Handle Create Account button click
        private void createaccountbt_Click(object sender, EventArgs e)
        {
            if (_client != null && _client.Connected)
            {
                // Get the username and password from the textboxes
                string username = usernametb.Text;
                string password = passwordtb.Text;

                // Create the message to send to the server in the format "REGISTER|username|password"
                string message = $"REGISTER|{username}|{password}";
                SendMessageToServer(message);

                // Receive and display the response from the server
                string response = ReceiveMessageFromServer();
                MessageBox.Show(response);
                
            }
            else
            {
                MessageBox.Show("Not connected to the server.");
            }
        }

        // Send a message to the server
        private void SendMessageToServer(string message)
        {
            if (_netStream != null)
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                _netStream.Write(data, 0, data.Length);
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
            return string.Empty;
        }

        // Clean up resources when disposing the form
        //protected override void Dispose(bool disposing)
        //{
            //if (disposing)
            //{
               // if (_netStream != null)
                 //   _netStream.Close();
                //if (_client != null)
                    //_client.Close();
            //}
            //base.Dispose(disposing);
       // }

        private void captchalb_Click(object sender, EventArgs e)
        {

        }

        private void captchainfo_Click(object sender, EventArgs e)
        {

        }

        private void captchaimage1_Click(object sender, EventArgs e)
        {
            if (captchaimageposition == 0)
            {
                MessageBox.Show("Captcha correct");
                Captcha.Visible = false;
                ProceedWithLogin();
            }
            else
            {
                MessageBox.Show("Captcha incorrect, try again.");
                LoadCaptchaImages();
            }
        }

        private void captchaimage2_Click(object sender, EventArgs e)
        {
            if (captchaimageposition == 1)
            {
                MessageBox.Show("Captcha correct");
                Captcha.Visible = false;
                ProceedWithLogin();
            }
            else
            {
                MessageBox.Show("Captcha incorrect, try again.");
                LoadCaptchaImages();
            }
        }

        private void captchaimage3_Click(object sender, EventArgs e)
        {
            if (captchaimageposition == 2)
            {
                MessageBox.Show("Captcha correct");
                Captcha.Visible = false;
                ProceedWithLogin();
            }
            else
            {
                MessageBox.Show("Captcha incorrect, try again.");
                LoadCaptchaImages();
            }
        }

        private void captchaimage4_Click(object sender, EventArgs e)
        {
            if (captchaimageposition == 3)
            {
                MessageBox.Show("Captcha correct");
                Captcha.Visible = false;
                ProceedWithLogin();
            }
            else
            {
                MessageBox.Show("Captcha incorrect, try again.");
                LoadCaptchaImages();
            }
        }

        private void Captcha_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
