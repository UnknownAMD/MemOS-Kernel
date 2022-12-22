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
            Kernel k = new Kernel();
            Console.ForegroundColor = ConsoleColor.Blue; Console.Write(curpath); Console.ForegroundColor = ConsoleColor.Green; Console.Write("> "); Console.ResetColor();
            string command = Console.ReadLine();
            k.History.Add(command);
            //Console.CancelKeyPress += new ConsoleCancelEventHandler(exit);
            appmanager.Main(command, curpath);
        }
        public static void Welcome()
        {
            Kernel k = new Kernel();
            Console.Clear();
            Console.WriteLine($"MemOS Kernel V{k.version}\n(C) 2022 MemzDev\n");
        }
        public static void exit(object sender, ConsoleCancelEventArgs args)
        {
        //    Main(Kernel.curpath);
        }        
    }
}

