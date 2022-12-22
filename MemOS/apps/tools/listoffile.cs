using System;
using System.IO;
using Sys = Cosmos.System;
using MemOS.apps.system;

namespace MemOS.apps.tools
{
    public static class ls
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: ls (ls)");
        }
        public static void Main(string input)
        {
            foreach (var directoryEntry in Directory.GetDirectories(input))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(directoryEntry);
            }
            foreach (var directoryEntry in Directory.GetFiles(input))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(directoryEntry);
            }
        }
        public static void Default(string curpath)
        {
            string path = curpath;
           // Console.WriteLine("List of Files in path: " + path);
            try
            {
                foreach (var directoryEntry in Directory.GetFiles(path))
                {
                    Console.WriteLine(directoryEntry);
                }
            }
            catch (Exception e)
            {
                ErrorHandle.ThrowError($"ERROR!: {e.Message}");
            }
        }
    }
}

