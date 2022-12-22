using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MemOS.apps.tools
{
    public static class Notepad
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: Notepad");
        }
        public static void Main(string Path)
        {
            string[] commands = { "<line> <data>", "massdel <startline> <count>", "del <line>", "page <page>", "help" };
            int curpage = 1;
            int pages = 1;
            int lastlines;
            if (!File.Exists(Path))
            {
                File.Create(Path);
            }
        WRITE_EDITOR:
            try
            {
                string[] lines2 = File.ReadAllLines(Path);
                pages = lines2.Length / 5;
                if (lines2.Length % 5 != 0) pages++;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Loaded File: " + Path + "\n");
                if (lines2.Length == 0) lineChanger("", Path, 5);
                if (curpage != pages)
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        Console.WriteLine(5 * (curpage - 1) + i + ". " + lines2[5 * (curpage - 1) + i - 1]);
                    }
                    Console.WriteLine();
                }
                else if (lines2.Length % 5 != 0)
                {
                    for (int i = 1; i != lines2.Length % 5 + 1; i++)
                    {
                        Console.WriteLine(5 * (curpage - 1) + i + ". " + lines2[5 * (curpage - 1) + i - 1]);
                    }
                    Console.WriteLine();
                }
                else
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        Console.WriteLine(5 * (curpage - 1) + i + ". " + lines2[5 * (curpage - 1) + i - 1]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("page: " + curpage + "/" + pages);
                Console.Write("Editor> ");
                string command = Console.ReadLine();
                string[] commandarray = command.Trim().Split(" ");
                switch (commandarray[0])
                {
                    case "page":
                        try
                        {
                            if (int.Parse(commandarray[1]) <= pages)
                            {
                                curpage = int.Parse(commandarray[1]);
                                goto WRITE_EDITOR;
                            }
                            else
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException is FormatException)
                            {
                                goto WRITE_EDITOR;
                            }
                        }
                        break;
                    case "del":
                        lineEraser(Path, int.Parse(commandarray[1]));
                        goto WRITE_EDITOR;

                    case "massdel":
                        MASSlineEraser(Path, int.Parse(commandarray[1]), int.Parse(commandarray[2]));
                        goto WRITE_EDITOR;

                    case "help":
                        Console.Clear();
                        foreach (string sus in commands)
                        {
                            Console.WriteLine(sus);
                        }
                        Console.ReadKey();
                        goto WRITE_EDITOR;

                    case "exit":
                        return;
                        
                    default:
                        lineChanger(command.Remove(0, commandarray[0].Length), Path, int.Parse(commandarray[0]));
                        goto WRITE_EDITOR;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CRITICAL ERROR!: LINE CANNOT BE A STRING");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Press any key to continue");
                    Console.ResetColor();
                    Console.ReadKey();
                    goto WRITE_EDITOR;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CRITICAL ERROR!\n" + ex);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Press any key to continue");
                    Console.ResetColor();
                    Console.ReadKey();
                    goto WRITE_EDITOR;
                }
            }
            goto WRITE_EDITOR;
        }
        static void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            if (arrLine.Length < line_to_edit)
            {
                string[] sus = new String[line_to_edit - arrLine.Length];
                Array.Copy(arrLine, sus, arrLine.Length);
                arrLine = sus;
                _ = sus;
            }
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }
        static void MASSlineEraser(string fileName, int startline, int endline)
        {
            List<string> arrline = File.ReadAllLines(fileName).ToList();
            Console.WriteLine(arrline.Count);
            arrline.RemoveRange(startline-1, endline-startline);
            arrline.RemoveRange(startline - 1, endline);
            File.WriteAllLines(fileName, arrline.ToArray());
            _ = arrline;
        }
        static void lineEraser(string fileName, int line)
        {
            List<string> arrline = File.ReadAllLines(fileName).ToList();
            arrline.RemoveAt(line - 1);
            File.WriteAllLines(fileName, arrline.ToArray());
            _ = arrline;
        }
    }
}