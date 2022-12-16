using System;
using System.Net.NetworkInformation;
using MemOS.apps.system;
using Cosmos.System.Network;

namespace MemOS.apps.tools
{
    public static class netping
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: ping (netping)");
        }
        public static void Main(string ip)
        {
            try
            {
                Ping pinger = new Ping();
                PingReply reply = pinger.Send(ip);
                Console.WriteLine("Ping: " + ip + " Status: " + reply.Status + " time=" + reply.RoundtripTime + "ms");
            }
            catch (Exception e)
            {
                ErrorHandle.ThrowError($"ERROR!: {e.Message}");
            }
        }
    }
}

