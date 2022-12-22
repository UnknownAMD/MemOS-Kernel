using System;
using System.IO;
using System.Text;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;

namespace MemOS.FtpServer
{
    // FTP data transfer mode.
    public enum TransferMode
    {

        // No mode set.
        NONE,


        // Active mode.
        ACTV,


        // Passive Mode.
        PASV
    }

    // FTPCommand class.
    internal class FtpCommand
    {

        // FTP Command Type.
        public string Command { get; set; }


        // FTP Command Content.
        public string Content { get; set; }
    }

    // FtpServer class. Used to handle FTP client connections.
    public class FSFtpServer : IDisposable
    {

        // Command Manager.
        internal FtpCommandManager CommandManager { get; set; }


        // TCP Listener used to handle new FTP client connection.
        internal TcpListener tcpListener;


        // Is FTP server listening for new FTP clients.
        internal bool Listening;


        // Are debug logs enabled.
        internal bool Debug;


        // Create new instance of the FtpServer class.

        // Exception Thrown if directory does not exists.
        // fs Initialized Cosmos Virtual Filesystem.
        // directory FTP Server root directory path.
        // debug Is debug logging enabled
        public FSFtpServer(CosmosVFS fs, string directory, bool debug = false)
        {
            if (Directory.Exists(directory) == false)
            {
                throw new Exception("FTP server can't open specified directory.");
            }

            CommandManager = new FtpCommandManager(fs, directory);

            Listening = true;
            Debug = debug;
        }


        // Listen for new FTP clients.
        public void Listen()
        {
            System.Console.WriteLine("Starting new FTPServer on: "+NetworkConfiguration.CurrentAddress.ToString());
            while (Listening)
            {
                tcpListener = new TcpListener(21);
                tcpListener.Start();
                var client = tcpListener.AcceptTcpClient();

                Log("Client : New connection from " + client.RemoteEndPoint.Address.ToString()); ;

                ReceiveNewClient(client);
            }
        }


        // Handle new FTP client.
        private void ReceiveNewClient(TcpClient client)
        {
            var ftpClient = new FtpClient(client);

            ftpClient.SendReply(220, "Service ready for new user.");

            while (ftpClient.Control.IsConnected())
            {
                ReceiveRequest(ftpClient);
            }

            ftpClient.Control.Close();

            //TODO: multiple FTP client connections
            Close();
        }


        // Parse and execute FTP command.
        private void ReceiveRequest(FtpClient ftpClient)
        {
            var ep = new EndPoint(Address.Zero, 0);

            try
            {
                var data = Encoding.ASCII.GetString(ftpClient.Control.Receive(ref ep));
                data = data.Remove(data.Length - 2, 2);

                Log("Client : " + data);

                var splitted = data.Split(' ');

                var command = new FtpCommand();
                command.Command = splitted[0];

                if (splitted.Length > 1)
                {
                    //Handle command content containing spaces
                    int i = data.IndexOf(" ") + 1;
                    command.Content = data.Substring(i);

                    command.Content = command.Content.Replace('/', '\\');
                }

                CommandManager.ProcessRequest(ftpClient, command);
            }
            catch (Exception ex)
            {
                Global.mDebugger.Send("Exception: " + ex.Message);
            }
        }


        // Write logs to console
        private void Log(string str)
        {
            if (Debug)
            {
                global::System.Console.WriteLine(str);
            }
        }


        // Close FTP server.
        public void Close()
        {
            Listening = false;
            tcpListener.Stop();
        }


        // Dispose
        public void Dispose()
        {
            Close();
        }
    }
}
