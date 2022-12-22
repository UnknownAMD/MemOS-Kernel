using System;
// Network Manager source code
// Network Access is coming soon.
using Cosmos.HAL;
using Cosmos.System.Network.IPv4.UDP.DHCP;

namespace MemOS.services
{
    public static class netrun
    {
        public static NetworkDevice Adapter;
        public static void autorun()
        {
            try
            {
                Adapter = NetworkDevice.GetDeviceByName("eth0"); //get network device by
                new DHCPClient().SendDiscoverPacket();
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  OK  ");
                Console.ResetColor();
                Console.WriteLine("] MemOS System: network services (netrun)");
            }
            catch (Exception e)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FAILED");
                Console.ResetColor();
                Console.WriteLine("] MemOS System: network services (netrun)");
                Console.WriteLine($"ERROR!: {e.Message}");
                Console.ResetColor();
            }
        }
    }
}

