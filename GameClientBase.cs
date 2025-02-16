using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using static System.Collections.Specialized.BitVector32;

class tcpserver
{
    public static List<string> gameids = new List<string>();
    public static List<string> activeusers = new List<string>(); //used to ensure same user isnt logged in across multiple devices
    //(only one log in per account)
    private static Dictionary<string, List<TcpClient>> hostgame = new Dictionary<string, List<TcpClient>>();
    //hostgame is a list of all the active games, where the gameid is the key and the data items are clients on that game 
    private static Dictionary<string, List<bool>> playstatus = new Dictionary<string, List<bool>>();

    private static Dictionary<string, List<int>> linkedxcoords = new Dictionary<string, List<int>>();
    private static Dictionary<string, List<int>> linkedycoords = new Dictionary<string, List<int>>();
    //these will ensure the same maze generation is used for players playing the same game. 

    private static object placeholder = new object();
    private static Dictionary<string, string> userCredentials = new Dictionary<string, string>(); //this will store the users username and password




    static void Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 8888);
        server.Start();
        Console.WriteLine("Server started");
        Console.WriteLine("Waiting for connections...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient(); // Waits for client connection
            Thread clientthread = new Thread(() => ClientCom(client)); //new thread for each client, smthn to separate each bit.
            clientthread.Start();
            Console.WriteLine("Client connected");
        }
    }

    static void ClientCom(TcpClient client)
    {
        bool connectedstatus = true; // Set to true initially so the connection loop works forever. since main com pathway here
        //i plan to use this format for each client server tranfer.
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (connectedstatus)
        {
            try
            {
                // reads incoming message from client
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                while (bytesRead == 0)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                }
                // parse the message (should be "ACTION|username|password")

                Console.WriteLine(message);

                string[] messageParts = message.Split('|');
                string action = messageParts[0];
                string taskone = messageParts[1];
                string tasktwo = messageParts[2];
                //this code basically prevents the need of passing three messages between the client and server
                //instead sending it all as one and then separating it via the | and storing the separates strings individually
                //this is also useful since i can use this for all transfers, and only a few lines of code needs to be changed on each side
                //if sending more items.

                // processes the log in or registration requests.
                if (action == "REGISTER")
                {
                    Console.WriteLine("trying to register");
                    HandleRegister(client, taskone, tasktwo);
                }
                if (action == "LOGIN")
                {
                    Console.WriteLine("trying to log in");
                    HandleLogin(client, taskone, tasktwo);
                }
                if (action == "CREATE")
                {
                    Console.WriteLine("trying to create");
                    HandleCreateGame(client);
                }
                if (action == "JOIN")
                {
                    Console.WriteLine("trying to join");
                    HandleJoinGame(client, taskone);
                }
                if (action == "LOGOUT")
                {
                    Console.WriteLine("trying to log out");
                    HandleLogOut(client, taskone);
                }
                if (action == "MAKE")
                {
                    Console.WriteLine("trying to make maze");
                    Console.WriteLine($"Action: {action}, TaskOne: {taskone}, TaskTwo: {tasktwo}");
                    MakeMaze(client, taskone);
                }
                if (action == "MOVE")
                {
                    string taskthree = messageParts[3];
                    string taskfour = messageParts[4];
                    string taskfive = messageParts[5];
                    Console.WriteLine($"Action: {action}, TaskOne: {taskone}, TaskTwo: {tasktwo}");
                }
            }
            catch (Exception ex) //prevents crashing by returning an error messages to the client.
            {
                Console.WriteLine("Error: " + ex.Message);
                connectedstatus = false;
            }

        }
    }
    static void HandleMove(string direction, string xcoord, string ycoord, string player, string gamecode)
    {
        if (player == "PLAYER1")
        {
            if (hostgame.ContainsKey(gamecode))
            {
                List<TcpClient> temp = hostgame[gamecode];
                TcpClient PLAYER2 = temp[1];
                string message = $"MOVE|{direction}|{xcoord}|{ycoord}|PLAYER2";
                NetworkStream stream = PLAYER2.GetStream();
                byte[] responseBytes = Encoding.ASCII.GetBytes(message);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
        }
        else
        {
            if (hostgame.ContainsKey(gamecode))
            {
                List<TcpClient> temp = hostgame[gamecode];
                TcpClient PLAYER1 = temp[0];
                string message = $"MOVE|{direction}|{xcoord}|{ycoord}|PLAYER1";
                NetworkStream stream = PLAYER1.GetStream();
                byte[] responseBytes = Encoding.ASCII.GetBytes(message);
                stream.Write(responseBytes, 0, responseBytes.Length);


            }
        }
    }
    static void HandleRegister(TcpClient client, string username, string password)
    {
        NetworkStream stream = client.GetStream();
        string response;

        lock (placeholder) // locks onto one thread  
        {
            Regex regex = new Regex((@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!.\-?]).{6,10}$"));
            //compares password with regex expression (isnt needed for log in since to create account would have already passed these paramters.
            if (regex.IsMatch(password))
            {
                if (!((username.Length < 6) | (username.Length > 10)))
                {
                    if (!userCredentials.ContainsKey(username))
                    {
                        // Register new user
                        userCredentials[username] = password; // Link username and password
                        response = "SUCCESS: User registered.";
                        Console.WriteLine($"New user registered: {username}");
                    }
                    else
                    {
                        // Username already exists
                        response = "ERROR: Username already taken.";
                    }
                }
                else
                {
                    response = "ERROR: Username does not fulfil required parameters";
                }
            }
            else
            {
                response = "ERROR: Password does not fulfil required parameters";
            }
        }

        // Send response to the client
        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
        stream.Write(responseBytes, 0, responseBytes.Length);
    }

    // Handle user login
    static void HandleLogin(TcpClient client, string username, string password)
    {
        NetworkStream stream = client.GetStream();
        string response;

        lock (placeholder) // Ensure thread safety when accessing userCredentials
        {
            // Check if the username exists and if the password matches
            if (userCredentials.TryGetValue(username, out string storedPassword) && storedPassword == password)
            {
                if (!(activeusers.Contains(username)))
                {
                    // Successful login
                    response = "SUCCESS";
                    Console.WriteLine($"User logged in: {username}");
                    activeusers.Add(username);
                }
                else
                {
                    response = "ERROR: User already logged in, please log out and try again.";
                }
            }
            else
            {
                // Invalid username or password
                response = "ERROR: Invalid username or password.";
            }
        }

        // Send response to the client
        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
        stream.Write(responseBytes, 0, responseBytes.Length);
    }


    // Handle game creation
    static void HandleCreateGame(TcpClient client)
    {
        string gamecode = GenerateGameCode();
        lock (placeholder)
        {
            hostgame[gamecode] = new List<TcpClient> { client };
            playstatus[gamecode] = new List<bool> { false };
        }
        Console.WriteLine($"Game Created. Game Code: {gamecode}");

        // Send the game code to the client
        NetworkStream stream = client.GetStream();
        string response = gamecode;
        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
        stream.Write(responseBytes, 0, responseBytes.Length);
    }

    // Handle game joining
    static void HandleJoinGame(TcpClient client, string gamecode)
    {
        lock (placeholder)
        {
            string response = "FALSE";
            if (!(hostgame.ContainsKey(gamecode)))
            {
                Console.WriteLine($"Game code {gamecode} does not exist.");
                response = "FALSE"; 
            }
            else
            {
                if (!(hostgame[gamecode].Contains(client)))
                {
                    if ((hostgame.ContainsKey(gamecode)) & !(hostgame[gamecode].Count > 2)) //only two clients per game. 
                    {
                        hostgame[gamecode].Add(client);
                        response = "TRUE";

                    }
                    else if (hostgame[gamecode].Count > 2)
                    {
                        response = "FULL";
                    }
                }
                else
                {
                    response = "TRUE"; //This returns true since the client is already registered as joined to the game, and 
                }
            }
            // Send the response back to the client
            NetworkStream stream = client.GetStream();
            byte[] responseBytes = Encoding.ASCII.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);

            if (hostgame[gamecode][0] == client)
            {
                Thread.Sleep(2000); //made it 'pause' here and below so client has time to change pages before data is sent 
                response = "ASSIGN|PLAYER1|NULL";
                Console.WriteLine(response);
                stream = client.GetStream();
                responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
            else
            {
                Thread.Sleep(2000);
                response = "ASSIGN|PLAYER2|NULL";
                Console.WriteLine(response);
                stream = client.GetStream();
                responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
        }
        
    }

    public static string GenerateGameCode()
    {
        string guid = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
        gameids.Add(guid);
        return guid;
    }

    static void HandleLogOut(TcpClient client, string username)
    {
        activeusers.Remove(username);
        Console.WriteLine(username + " is no longer active");
        string response = $"{username} is now logged out";
        NetworkStream stream = client.GetStream();
        byte[] responseBytes = Encoding.ASCII.GetBytes(response);
        stream.Write(responseBytes, 0, responseBytes.Length);
    }

    public static void MakeMaze(TcpClient client, string gamecode)
    {

        if (linkedxcoords.ContainsKey(gamecode))
        {
            for (int i = 0; i < 20; i++)
            {
                int ans1 = linkedxcoords[gamecode][i];
                int ans2 = linkedycoords[gamecode][i];
                string sans1 = Convert.ToString(ans1);
                string sans2 = Convert.ToString(ans2);
                NetworkStream stream = client.GetStream();
                string response = $"WALL|{sans1}|{sans2}|{i}";
                Console.WriteLine(response);
                stream = client.GetStream();
                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
                //this will send wall coordinates one block at a time. 

                string message = "NOT BUILT";

                ///this means the server can wait for the wall to be built by the client 
                ///before sending the data for the next wall to be built
                while (message != "BUILT")
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                }

            }
        }
        else
        {
            //below are for maze generation
            List<int> xcoords = new List<int>();
            List<int> ycoords = new List<int>();
            int count = 0;
            int xco = 1;
            int yco = 1;
            bool invalid = false;
            int[,] mazeGrid = new int[11, 11]; // 11x11 grid (for 50 to 550 by step 50)
            Random r = new Random();

            while (count != 20)
            {
                invalid = false;
                xco = 1;
                yco = 1;
                while ((xco % 50 != 0))
                {
                    xco = r.Next(50, 551);
                }
                while ((yco % 50 != 0))
                {
                    yco = r.Next(50, 551);
                }

                int row = (yco - 50) / 50; // convert y-coordinate to grid row
                int col = (xco - 50) / 50; // convert x-coordinate to grid col

                // Checks if the wall already exists
                if (mazeGrid[row, col] == 1)
                {
                    continue; // #skipped *insert sunglasses emoji hohoho im so cool.*
                }

                mazeGrid[row, col] = 1;
                //adds wall to the maze

                // Check if the maze is still connected
                if (!PathFinderP1(mazeGrid))
                {
                    mazeGrid[row, col] = 0; // Remove the wall if it breaks connectivity
                    continue;
                }
                if ((xco == 50) && (yco == 50))
                {
                    mazeGrid[row, col] = 0; // Remove the wall if it breaks connectivity
                    continue;
                }
                if ((xco == 550) && (yco == 50))
                {
                    mazeGrid[row, col] = 0; // Remove the wall if it breaks connectivity
                    continue;
                }

                xcoords.Add(xco);
                ycoords.Add(yco);
                count++;
            }

            linkedxcoords.Add(gamecode, xcoords);
            linkedycoords.Add(gamecode, ycoords);

            for (int i = 0; i < 20; i++) //done after checks and all so wall can be built.
            {
                int ans1 = linkedxcoords[gamecode][i];
                int ans2 = linkedycoords[gamecode][i];
                string sans1 = Convert.ToString(ans1);
                string sans2 = Convert.ToString(ans2);
                NetworkStream stream = client.GetStream();
                string response = $"WALL|{sans1}|{sans2}|{i}";
                Console.WriteLine(response);
                stream = client.GetStream();
                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
                //this will send wall coordinates one block at a time. 

                string message = "NOT BUILT";

                ///this means the server can wait for the wall to be built by the client 
                ///before sending the data for the next wall to be built
                while (message != "BUILT")
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                }
            }

        }
    }

        public static bool PathFinderP1(int[,] mazeGrid) //checks valid path from Player1 start position
        //I was going to also do this from player2's position but it seems to crash, due to the low ammount of blocks it wont really make a difference. 
        {
            bool[,] visited = new bool[11, 11];
            Queue<(int row, int col)> queue = new Queue<(int, int)>();

            // Start from Pac-Man's starting position (row, col) = (0, 0) for (50, 50)
            //this would be for player1
            queue.Enqueue((0, 0));
            visited[0, 0] = true;

            int[] dRow = { -1, 1, 0, 0 }; // Movement offsets (up, down, left, right)
            int[] dCol = { 0, 0, -1, 1 };

            while (queue.Count > 0)
            {
                var (row, col) = queue.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int newRow = row + dRow[i];
                    int newCol = col + dCol[i];

                    // Check if the new position is valid and not visited
                    if (newRow >= 0 && newRow < 11 && newCol >= 0 && newCol < 11 &&
                        !visited[newRow, newCol] && mazeGrid[newRow, newCol] == 0)
                    {
                        visited[newRow, newCol] = true;
                        queue.Enqueue((newRow, newCol)); //quickly adds it onto que.
                    }
                }
            }

            // Check if all non-wall spaces were visited
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (mazeGrid[i, j] == 0 && !visited[i, j])
                    {
                        return false; // There is a part of the maze that is unreachable
                    }
                }
            }
            return true;
        }


        public static bool PathFinderP2(int[,] mazeGrid) //checks valid path from Player2 start position
        {
            bool[,] visited = new bool[11, 11];
            Queue<(int row, int col)> queue = new Queue<(int, int)>();

            // Start from Pac-Man's starting position (row, col) = (0, 0) for (50, 50)
            //this would be for player1
            queue.Enqueue((11, 0));
            visited[11, 0] = true;

            int[] dRow = { -1, 1, 0, 0 }; // Movement offsets (up, down, left, right)
            int[] dCol = { 0, 0, -1, 1 };

            while (queue.Count > 0)
            {
                var (row, col) = queue.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int newRow = row + dRow[i];
                    int newCol = col + dCol[i];

                    // Check if the new position is valid and not visited
                    if (newRow >= 0 && newRow < 11 && newCol >= 0 && newCol < 11 &&
                        !visited[newRow, newCol] && mazeGrid[newRow, newCol] == 0)
                    {
                        visited[newRow, newCol] = true;
                        queue.Enqueue((newRow, newCol)); //quickly adds it onto que.
                    }
                }
            }

            // Check if all non-wall spaces were visited
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (mazeGrid[i, j] == 0 && !visited[i, j])
                    {
                        return false; // There is a part of the maze that is unreachable
                    }
                }
            }
            return true;
        }

    
}


