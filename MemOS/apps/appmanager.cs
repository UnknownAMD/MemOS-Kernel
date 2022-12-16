/* TODO:
 * make this faster
 * a lot of error checking
 * needs more optimization
 * fix diskmg
 * fix math
 * fix chmod
 * fix reboot
 * fix user
*/


using MemOS.apps.basic;
using MemOS.apps.diskaccess;
using MemOS.apps.pcinfo;
using MemOS.apps.system;
using MemOS.apps.tools;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;


namespace MemOS.apps
{
    public static class appmanager
    {
        public static void TrySee()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS System: APP MANAGER");
        }
        public static void Main(string input, string curpath)
        {
            List<string> args;
            try
            {
                args = input.Split(' ').ToList();
                args.Add("");
                input = args[0];
                Console.WriteLine(input);
                switch (input)
                {
                    case "help": if (args[1] == "") { help.Main(0); } else { help.Main(int.Parse(args[1]) - 1); } break;
                    case "debug": Console.WriteLine(input+args[1]); break;
                    case "ls": if (args[1] == "") { listoffile.Main(curpath); } else { listoffile.Main(args[1]); } break;
                    case "diskmg": diskmg.Main(args.ToArray()); break;
                    case "clear": Console.Clear(); break;
                    //case "ping": netping.Main(args[1]); break;
                    case "reboot": powercontrol.Main(args[1]); break;
                    case "shutdown": powercontrol.Main(input); break;
                    case "cat": cat.Main(args[1]); break;
                    case "echo": echoapp.Main(args[1]); break;
                    case "cd": cd.Main(args[1], curpath); break;
                    case "date": date.Main(); break;
                    case "mkdir": mkdir.Main(args[1], curpath); break;
                    case "touch": touch.Main(args[1],curpath); break;
                    case "krenelinfo": kernelinfo.Main(); break;
                    case "sysinfo": sysinfo.Main();  break;
                    case "beep": beep.Main(); break;
                    case "playbeep": playbeep.Main(); break;
                    case "cdreset": Kernel.Resetpath(); break;
                    case "notepad": Notepad.Main(args[1]); break;
                    case "rm": switch (args[1]) { case "-f": remove.fullpath(args[1]); break; case "-h": remove.help(); break; case "": remove.help(); break; default: remove.args(args[1], curpath); break; } break;
                   // case "lua": switch (args[1]) { case "-c": break; case "-h": break; case "-r": break; default: break; } break;

                    default:
                        if (File.Exists(input))
                        {
                      //     LuaInterpreter.Execute(input); break;
                        }
                        ErrorHandle.ThrowError("Syntax error: Unknown command or Lua script");
                        break;
                }
            }
            catch (Exception e) 
            { 
                ErrorHandle.ThrowError($"ERROR!: {e.Message}"); 
            }
        }
    }
}


