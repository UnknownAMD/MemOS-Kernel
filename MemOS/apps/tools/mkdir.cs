using System;
using System.IO;
using MemOS.apps.system;

namespace MemOS.apps.tools
{
    public static class mkdir
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: mkdir");
        }
        public static void Main(string input, string curpath)
        {
            try
            {
               // input = Regex.Replace(input, @"[^0-9a-zA-Z^\\]+", "-");
                Directory.CreateDirectory($"{curpath}\\{input}");
            }
            catch (Exception e)
            {
                ErrorHandle.ThrowError($"ERROR!: {e.Message}");
            }
        }
    }
}

