using System;
using System.IO;
using MemOS.apps.system;

namespace MemOS.apps.tools
{
    public static class cd
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: CD");
        }
        public static void Main(string input, string curpath)
        {
            if (input != "..")
            {
                if (Directory.Exists(curpath+input))
                {
                    if (input.EndsWith('\\')) { input = input.Remove(input.Length - 2, 1); }
                    Kernel.PathCD($@"{curpath}{input}\");
                }
                else
                {
                    ErrorHandle.ThrowError("ERROR!: Directory doesn't exist");
                }
            }
            else
            {
                Console.WriteLine(curpath);
                string[] folders = curpath.Split(@"\");
                string stage2 = curpath.Remove(curpath.Length - folders[folders.Length-2].Length-1);
                Console.WriteLine(stage2);
                Kernel.PathCD(stage2);

            }
        }
    }
}

