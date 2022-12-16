using System;

namespace MemOS.apps.system
{
    public static class ErrorHandle
    {
        public static void TrySee()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS System: Error Handler");
        }
        public static void ThrowError(string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void ThrowWarn(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void ThrowSuccess(string input)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
