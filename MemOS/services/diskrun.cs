using System;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.FileSystem;
using MemOS.apps.system;

namespace MemOS.services
{
    public static class diskrun
    {
        public static CosmosVFS fs = new CosmosVFS();
        public static void autorun()
        {
            try
            {
                VFSManager.RegisterVFS(fs, aShowInfo:true);
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  OK  ");
                Console.ResetColor();
                Console.WriteLine("] MemOS System: disk services (diskrun)");
            }
            catch (Exception e)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("FAILED");
                Console.ResetColor();
                Console.WriteLine("] MemOS System: disk services (diskrun)");
                Console.ForegroundColor = ConsoleColor.Red;
                ErrorHandle.ThrowError($"ERROR!: {e.Message}");
                Console.ResetColor();
            }
        }
    }
}

