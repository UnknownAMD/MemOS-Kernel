using System;
using System.IO;
using MemOS;
using Cosmos.System.Network.IPv4;
using System.Text;
using Cosmos.System.Network.IPv4.TCP;

namespace MemOS.apps.tools
{
    public static class wget
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: wget (netrun)");
        }
        public static void Download(string curpath,string address, string port, string message)
        {
            /* This features a TCP connection
            network initialization is needed*/

            // Parse arguments
            Address add = Address.Parse(address);
            int destPort = Int32.Parse(port);

            // Base local port = 4242
            Console.WriteLine("Connecting to destination host...");
            using var xClient = new TcpClient(destPort); // Ports should be corresponding
            xClient.Connect(add, destPort);

            // Send data
            Console.WriteLine("Sending request...");
            xClient.Send(Encoding.ASCII.GetBytes(message));

            // Receive data
            var endpoint = new EndPoint(Address.Zero, 0);
            Console.WriteLine("EndPoint set");

            var data = xClient.Receive(ref endpoint);  //set endpoint to remote machine IP:port
            Console.WriteLine("Received Data!");


            string[] folders = message.Split(@"\");
            string file = message.Remove(message.Length - folders[folders.Length - 2].Length - 1);
            File.WriteAllBytes(curpath+file, data);
        }
        public static void Help() 
        {
            Console.WriteLine("Usage: wget <ip> <port> \"GET <ip>:<port>\\path\\to\\the\\file\\\"");
        }
    }
}

