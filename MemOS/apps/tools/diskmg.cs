using System;
using Cosmos.System.FileSystem.VFS;
using MemOS.apps.system;

namespace MemOS.apps.tools
{
    public static class diskmg
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: diskmg");
        }
        public static void Main(string[] args)
        {
            if (args[1] == "-t")
            {
                Console.WriteLine("Disk ID: " + args[2]);
                try
                {
                    Console.WriteLine("FileSystem Type: " + VFSManager.GetFileSystemType(args[2]));
                }
                catch (Exception e)
                {
                    ErrorHandle.ThrowError($"ERROR!: {e.Message}");
                }
            }
            else if (args[1] == "-s")
            {
                Console.WriteLine("Disk ID: " + args[2]);
                try
                {
                    string SizeSymbol = "bytes";
                    long available_space = VFSManager.GetAvailableFreeSpace(args[2]);
                    if (available_space >= 0 && available_space < 1000) SizeSymbol = "BYTES";
                    else if (available_space >= 1000 && available_space < 1000000) SizeSymbol = "KB";
                    else if (available_space >= 1000000 && available_space < 1000000000) SizeSymbol = "MB";
                    else if (available_space >= 1000000000 && available_space < 1000000000000) SizeSymbol = "GB";
                    else if (available_space >= 1000000000000 && available_space < 1000000000000000) SizeSymbol = "TB";
                    else if (available_space >= 1000000000000000 && available_space < 1000000000000000000) SizeSymbol = "PB";
                    Console.WriteLine("Available Space: " + available_space + SizeSymbol);
                }
                catch (Exception e)
                {
                    ErrorHandle.ThrowError($"ERROR!: {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("DiskManager 1.0");
                Console.WriteLine("-t <DISK> -- Disk Type");
                Console.WriteLine("-s <DISK> -- Available Space");
            }
        }
    }
}

