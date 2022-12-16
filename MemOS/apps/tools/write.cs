using System;
using System.IO;
using Cosmos.System.FileSystem.VFS;
using MemOS.apps.system;

namespace MemOS.apps.tools
{
    public static class write
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: write");
        }
        public static void Main(string input)
        {
            Console.WriteLine("File: " + input);
            Console.Write("Context > ");
            var data = Console.ReadLine();
            try
            {
                if (VFSManager.GetFile(input).GetFileStream().CanWrite)
                {
                    File.WriteAllText(input,data);
                }
            }
            catch (Exception e)
            {
                ErrorHandle.ThrowError($"ERROR!: {e.Message}");
            }
        }
    }
}

