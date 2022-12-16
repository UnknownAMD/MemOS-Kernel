using System;
using Cosmos.System.FileSystem.VFS;
using System.IO;

namespace MemOS.apps.tools
{
    public static class remove
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: remove");
        }
        public static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR!: missing operand");
            Console.ResetColor();
            Console.WriteLine("$ rm <FileName> -- Delete files");
            Console.WriteLine("$ rm <FolderName> -- Delete files on full path");
            Console.WriteLine("$ rm -f <FileName> -- Delete files on full path");
            Console.ResetColor();
        }
        public static void args(string input, string curpath)
        {
            {
                string filepath = curpath + "\\" + input;
                Console.WriteLine("Deleting File...");
                try
                {
                    VFSManager.DeleteFile(filepath);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR!: {e.Message}");
                    Console.ResetColor();
                }
            }
        }
        public static void fullpath(string input)
        {
            Console.WriteLine("Deleting File...");
            try
            {
                File.Delete(input);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR!: {e.Message}");
                Console.ResetColor();
            }
        }
        public static void help()
        {
            Console.WriteLine("$ Usage:");
            Console.WriteLine("$ rm <File>    -- Delete files");
            Console.WriteLine("$ rm <Folder>  -- Delete files on full path");
            Console.WriteLine("$ rm -f <File> -- Delete files on full path");
        }
    }
}


