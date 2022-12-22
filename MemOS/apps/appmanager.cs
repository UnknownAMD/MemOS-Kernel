/* TODO:
 * make this faster
 * a lot of error checking
 * needs more optimization
 * fix diskmg
 * fix math
 * fix ping
 * fix lua
*/


using MemOS.apps.basic;
using MemOS.apps.system;
using MemOS.apps.tools;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using MemOS.Emulation;
using System.Text;
using Neo.IronLua;
using ASMToMSE32;
using MemOS.FtpServer;
using MemOS.services;
using Cosmos.System.Network.Config;

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
            Kernel k = new Kernel();
            List<string> args;
            try
            {
                args = input.Split(' ').ToList();
                args.Add("");
                input = args[0];
                switch (input)
                {
                    case "help": if (args[1] == "") { help.Main(0); } else { help.Main(int.Parse(args[1]) - 1); } break;
                    case "ls": if (args[1] == "") { ls.Main(curpath); } else { ls.Main(args[1]); } break;
                    case "diskmg": diskmg.Main(args.ToArray()); break;
                    case "clear": Console.Clear(); break;
                    //case "ping": netping.Main(args[1]); break;
                    case "wget": if (args[1] == "") { wget.Help(); } else { wget.Download(curpath, args[1], args[2], args[3]); } break;
                    case "reboot": powercontrol.Reboot(); break;
                    case "shutdown": powercontrol.Shutdown(); break;
                    case "cat": if (args[1] == "") { ErrorHandle.ThrowError("ERROR!: Path cannot be null!"); } else { cat.Main(args[1]); } break;
                    case "echo": Console.WriteLine(args[1]); break;
                    case "cd": if (args[1] == "") { Kernel.Resetpath(); } else { cd.Main(args[1], curpath); } break;
                    case "date": date.Main(); break;
                    case "mkdir": if (args[1] == "") { ErrorHandle.ThrowError("ERROR!: Path cannot be null!"); } else { mkdir.Main(args[1], curpath); } break;
                    case "touch": if (args[1] == "") { ErrorHandle.ThrowError("ERROR!: Path cannot be null!"); } else { touch.Main(args[1], curpath); } break;
                    case "sysinfo": sysinfo.Main(); break;
                    case "beep": Console.Beep(); break;
                    case "notepad": if (args[1] == "") { ErrorHandle.ThrowError("ERROR!: Path cannot be null!"); } else { Notepad.Main(curpath + args[1]); } break;
                    case "user": switch (args[1]) { case "-c": usermanager.Create(); break; case "-d": usermanager.Delete(args[2]); break; case "-l": usermanager.Login(args[2]); break; case "-h": usermanager.Help(); break; default: usermanager.Help(); break; } break;
                    case "rm": switch (args[1]) { case "-f": remove.fullpath(args[1]); break; case "-h": remove.help(); break; case "": remove.help(); break; default: remove.args(args[1], curpath); break; } break;
                    case "miv": MIV.StartMIV(); break;
                    //case "lua": if (args[1].EndsWith(".lua")) { LuaInterpreter.Execute(args[1].Split('.')[0]); } else { LuaInterpreter.Execute(args[1]); } break;
                    case "compile": if (args[1] == "" || args[1] == "-h") { ASMCompiler.Help(); } else if (args[1] == "-d") { ASMCompiler.CompileDir(curpath + args[2]); } else { ASMCompiler.Compile(args[1]); } break;
                    case "chmod": chmod.Main(args[1], curpath); break;
                    case "ftpserver": if (args[1] == "start") { var xServer = new FSFtpServer(diskrun.fs, "0:\\"); xServer.Listen(); } break;
                    case "myip": Console.WriteLine(NetworkConfiguration.CurrentAddress.ToString()); break;
                    case "loginconf": if (args[1] == "enable") { usermanager.EnableLogin(); } else if (args[1] == "disable") { usermanager.DisableLogin(); } else { Console.WriteLine(k.LoginEnabled); } break;

                    default:
                        if (File.Exists(curpath+input))
                        {
                            try
                            {
                                Execute.RunEXE(Encoding.ASCII.GetBytes(input.Remove(input.Length - 5)));
                            }
                            catch (Exception e)
                            {
                                ErrorHandle.ThrowError($"ERROR!: {e.Message}");
                            }
                        }
                        else ErrorHandle.ThrowError("Syntax error: Unknown command or Executable");
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


