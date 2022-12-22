using System;
using System.IO;
using MemOS.apps.system;

namespace MemOS.apps.tools
{
    public static class cat
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: cat");
        }
        public static void Main(string path)
        {
            try
            { 
                 Console.WriteLine(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                ErrorHandle.ThrowError($"ERROR!: {e.Message}");
            }
        }
    }
}

