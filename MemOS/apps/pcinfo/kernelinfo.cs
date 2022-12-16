using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using System.Drawing;
using Cosmos.Core.IOGroup;
using System.Threading;
using Cosmos.HAL;
using Cosmos.Core;
using Cosmos.Common;
using System.Net;
using Cosmos.System.Network;


namespace MemOS.apps.pcinfo
{
    public static class kernelinfo
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: kernelinfo");
        }
        public static void Main()
        {
            Console.WriteLine("Kernel Info: MemOS-Kernel");
            Console.WriteLine("Version: 1.5.0");
            Console.WriteLine("Based on: CosmosOS (Userkit 2022)");
        }
    }
}

