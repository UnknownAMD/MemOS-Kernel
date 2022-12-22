using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;
using System.Text;

namespace MemOS.FtpServer
{
    // FtpClient class.
    internal class FtpClient
    {
        // Client IP Address.
        internal Address Address { get; set; }

        // Client TCP Port.
        internal int Port { get; set; }

        // TCP Control Client. Used to send commands.
        internal TcpClient Control { get; set; }

        // TCP Data Transfer Client. Used to transfer data.
        internal TcpClient Data { get; set; }

        // TCP Data Transfer Listener. Used in PASV mode.
        internal TcpListener DataListener { get; set; }

        // FTP client data transfer mode.
        internal TransferMode Mode { get; set; }

        // FTP client username.
        internal string Username { get; set; }

        // FTP client password.
        internal string Password { get; set; }

        // Is user connected.
        internal bool Connected { get; set; }

      
        // Create new instance of the FtpClient class
        internal FtpClient(TcpClient client)
        {
            Control = client;
            Connected = false;
            Mode = TransferMode.NONE;
        }

        // Is user connected.
        internal bool IsConnected()
        {
            if (Connected == false)
            {
                SendReply(530, "Login incorrect.");
                return Connected;
            }
            else
            {
                return Connected;
            }
        }

        // Send text to control socket (usually port 21)
        internal void SendReply(int code, string message)
        {
            message = message.Replace('\\', '/');
            Control.Send(Encoding.ASCII.GetBytes(code + " " + message + "\r\n"));
        }
    }
}
