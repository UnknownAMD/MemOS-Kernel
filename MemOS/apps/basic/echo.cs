using System;

namespace MemOS.apps.basic
{
    public static class echoapp
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: echo");
        }
        public static void Main(string input)
        {
            Console.WriteLine(input);
        }
    }
}

