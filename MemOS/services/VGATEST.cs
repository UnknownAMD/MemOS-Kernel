// This code is just trash

using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
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
using Cosmos.System.Graphics;

namespace MemOS.services
{
    public static class vgamemos
    {
        public static void run()
        {
           
            try
            {
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

