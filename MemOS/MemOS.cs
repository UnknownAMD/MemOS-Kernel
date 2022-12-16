using System;
using MemOS.apps;


namespace MemOS
{
    public static class MemOS
        {
        
        public static void TryBoot()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS Global");
        }
        public static void Main(string curpath)
        {
            Console.ForegroundColor = ConsoleColor.Blue; Console.Write(curpath); Console.ForegroundColor = ConsoleColor.Green; Console.Write("> "); Console.ResetColor();
            appmanager.Main(Console.ReadLine(), curpath);
        }
        public static void Welcome()
        {
            Console.Clear();
            Console.WriteLine("MemOS Kernel V1.3.2\n(C) 2022 MemzDev\n");
        }
    }
}

