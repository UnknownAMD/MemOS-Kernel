using System;
using System.IO;
using MemOS.apps.system;

namespace MemOS.apps.tools
{
    public static class chmod
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: chmod");
        }
        public static void Main(string input, string curpath)
        {
            string[] lines = File.ReadAllLines(curpath+input);
            foreach (string line in lines) appmanager.Main(line, curpath);
        }
    }
}

