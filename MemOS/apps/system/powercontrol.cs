using Cosmos.Core;
using System;
using Sys = Cosmos.System;

namespace MemOS.apps.system
{
   public static class powercontrol
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: powercontrol");
        }
        public static void Reboot()
        {
            Console.WriteLine("Rebooting...");
            Cosmos.HAL.Global.PIT.Wait(1000);
            Sys.Power.Reboot();
        }
        public static void Shutdown()
        {
            Console.WriteLine("Shutting Down...");
            Cosmos.HAL.Global.PIT.Wait(1000);
            Sys.Power.Shutdown();
        }
    }
}

