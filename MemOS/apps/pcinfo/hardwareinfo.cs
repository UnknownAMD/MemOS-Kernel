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
    public static class hardwareinfo
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
            Console.WriteLine("Operating System: MemOS OS 1.2.0");
            Console.WriteLine("Kernel: MemOS-Kernel 1.5.0");
            Console.WriteLine("CPU: " + CPU.GetCPUBrandString());
            Console.WriteLine("RAM: " + CPU.GetAmountOfRAM() + "MB");
            try
            {
                Console.WriteLine("CPU Uptime: " + CPU.GetCPUUptime());
            }
            catch (Exception e)
            {
                Console.WriteLine("CPU Uptime: " + e.Message);
            }
        }
    }
}

