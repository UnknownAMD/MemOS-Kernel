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
        public static void Main(string input)
        {
            var text = input;
            if (text == "")
            {
                Console.WriteLine("ERROR!: Power Methods is null");
            }
            else if (text == "reboot")
            {
                Console.WriteLine("Rebooting...");
                Cosmos.HAL.Global.PIT.Wait(1000);
                Sys.Power.Reboot();
            }
            else if (text == "shutdown")
            {
                Console.WriteLine("Shutting Down...");
                Cosmos.HAL.Global.PIT.Wait(1000);
                Sys.Power.Shutdown();
            }
        }
    }
}

