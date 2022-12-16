using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
// Network Manager source code
// Network Access is coming soon.

using Cosmos.System.Graphics;
using System.Drawing;
using Cosmos.Core.IOGroup;
using System.Threading;
using Cosmos.HAL;
using Cosmos.Core;
using Cosmos.Common;
using System.Net;
using Cosmos.System.Network;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using System.IO;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;
using Cosmos.System.Network.Config;

namespace MemOS.services
{
    public static class netrun
    {
        public static void autorun()
        {
            try
            {
                IPConfig.Enable(NetworkDevice.GetDeviceByName("eth0"), Address.Zero, Address.Broadcast, Address.Parse("192.168.1.1"));
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

