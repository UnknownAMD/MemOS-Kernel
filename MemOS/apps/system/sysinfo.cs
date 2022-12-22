using System;
using Cosmos.Core;

namespace MemOS.apps.system
{
    public static class sysinfo
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: sysinfo");
        }
        public static void Main()
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Beep();
                Kernel k = new Kernel();
                Console.WriteLine($"MemOS-Kernel: {k.version}");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("CPU: " + CPU.GetCPUBrandString());
                Console.WriteLine("Cycle Speed: " + CPU.GetCPUCycleSpeed() / 10000000000);
                Console.WriteLine("Total RAM: " + CPU.GetAmountOfRAM() + "MB");
                Console.WriteLine("Available RAM: " + CPU.GetEndOfKernel() + 1024 / 1048576 + "KB");
                Console.WriteLine("Uptime: " + CPU.GetCPUUptime());
            }
            catch (Exception e)
            {
                ErrorHandle.ThrowError($"ERROR!: {e.Message}");
            }
            Console.Beep();
            Console.ResetColor();
        }
    }
}

