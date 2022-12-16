using System;
using System.IO;
using MemOS.apps.system;

namespace MemOS.apps.tools
{
    public static class touch
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: touch");
        }
        public static void Main(string input, string path)
        {
            try
            {
                File.Create(path+input);
            }
            catch (Exception e)
            {
                ErrorHandle.ThrowError($"ERROR!: {e.Message}");
            }
        }
    }
}

