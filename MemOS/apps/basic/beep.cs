using System;

namespace MemOS.apps.basic
{
    public static class beep
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: beep");
        }
        public static void Main()
        {
            Console.Beep();
        }
    }
}

