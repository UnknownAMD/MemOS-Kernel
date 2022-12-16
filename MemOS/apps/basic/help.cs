using System;

namespace MemOS.apps.basic
{
    public static class help
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: help");
        }
        public static void Main(int i)
        {
            switch (i)
            {
                case 0:
                    Console.WriteLine("help:   Shows this list again. Usage help [pg#]");
                    Console.WriteLine("--------------------------FILE SYSTEM--------------------------");
                    Console.WriteLine("ls:     Lists files and dirs. Usage: ls");
                    Console.WriteLine("diskmg: Drive Manager. Usage: diskmg");
                    Console.WriteLine("rm:     Remove File. Usage: rm [path]");
                    Console.WriteLine("cd:     Move to Dir. Usage: cd [path]");
                    Console.WriteLine("mkdir:  Create Dir. Usage: mkdir [path]");
                    Console.WriteLine("touch:  Create File. Usage: touch [path]");
                    Console.WriteLine("cat:    View File content. Usage: cat [path]");
                    Console.WriteLine("write:  Write to File. Usage: write [path]");
                    Console.WriteLine("miv:    Vim-like Editor. Usage: miv");
                    Console.WriteLine("-----------------------------BASIC-----------------------------");
                    Console.WriteLine("clear:  Clear Screen.");
                    Console.WriteLine("echo:   Echoes an input. Usage: echo [text to echo]");

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Page 1/2");
                    Console.WriteLine("For more commands, type help [page number]");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case 1:
                    Console.WriteLine("-----------------------------BASIC-----------------------------");
                    Console.WriteLine("beep: Beep once. Usage: beep");
                    Console.WriteLine("playbeep: Beep five times. Usage: playbeep");
                    Console.WriteLine("chmod: Executes a batch file. Usage: chmod [path]");
                    Console.WriteLine("math: Computes the input. Usage: math -h");
                    Console.WriteLine("user: User operations. Usage: user -h");
                    Console.WriteLine("-----------------------------SYSTEM----------------------------");
                    Console.WriteLine("sysinfo: System Information. Usage: sysinfo");
                    Console.WriteLine("kernelinfo: Kernel Information. Usage: kernelinfo");
                    Console.WriteLine("date:  unix style time. Usage: date");
                    Console.WriteLine("reboot: Reboots the computer. Usage: reboot [delay ms]");
                    Console.WriteLine("shutdown: Shutdowns the PC. Usage: shutdown [delay ms]\n");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Page 2/2");
                    Console.WriteLine("For more commands, type help [page number]");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}

