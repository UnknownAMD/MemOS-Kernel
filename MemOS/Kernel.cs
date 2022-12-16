using System;
using System.IO;
using Sys = Cosmos.System;
using MemOS.apps;
using MemOS.services;
using MemOS.apps.pcinfo;
using MemOS.apps.system;
using MemOS.apps.basic;
//using Cosmos.System.Emulation;

namespace MemOS
{
    public class Kernel : Sys.Kernel
    {
        bool isConsole = true; bool firstTImeUser = true;
        int usergroup = 1;public bool isLoggedIn = false; string user = "", ps = ""; // Might be a security issue
        public string tryit;
        public static string curpath = @"0:\";
        protected override void BeforeRun()
        {
            Console.WriteLine("Loading Kernel...");
            //FGMSECInstructionSet.Install();
            Console.ResetColor();
            MemOS.TryBoot();
            appmanager.TrySee();
            echoapp.TryCheck();
            powercontrol.TryCheck();
            sysinfo.TryCheck();
            date.TryCheck();
            diskrun.autorun();
            Console.Clear();
            MemOS.Welcome();
            firstTImeUser = !File.Exists("0:\\users.dat");
        }

        protected override void Run()
        {
            if (!firstTImeUser) //If we aren't a first time user
            {
                if (isLoggedIn)
                {
                    if (isConsole)
                        consoleLoop();
                    //else
                    //initGUI(); // sussy gui
                }
                else
                    displayLogin();
            }
            else  //Else init the install procedure
            {
                Console.Clear();
                MemOS.Welcome();
                Console.WriteLine("Setting up MemOS");
                try
                {
                    File.Create("0:\\users.dat");
                    File.WriteAllText("0:\\users.dat", "user:root$pswd:root$date:#NAL#$group:01$name:root");
                    Console.WriteLine(File.ReadAllText("0:\\users.dat"));
                }
                catch (Exception e) { Console.WriteLine(e); }
                Console.WriteLine("Created users.dat! Press any key to reboot...");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("Rebooting PC...");
                Sys.Power.Reboot();
            }
        }
        public static void PathCD(string curpath)
        {
        antidownwithoutreason:
            MemOS.Main(curpath);
            goto antidownwithoutreason;
        }
        public static void Resetpath()
        {
            string curpath = @"0:\";
        antidownwithoutreason:
            MemOS.Main(curpath);
            goto antidownwithoutreason;
        }
        void consoleLoop()
        {
            antidownwithoutreason:
            MemOS.Main(curpath);
            goto antidownwithoutreason;
        }
        public void displayLogin()
        {
            Console.Clear();
            MemOS.Welcome();
            string[] logins = File.ReadAllLines(@"0:\users.dat");
            Login l = new Login();
            Console.Write("Enter your username: ");
            string s = Console.ReadLine();
            Console.Write("Enter your password: ");
            Console.ResetColor();
            string p = maskedEntry();
            if (l.validateLogin(logins, p, s))
            {
                usergroup = l.group; user = s; ps = p;
                isLoggedIn = true;
            }
            Console.ReadKey();
            Console.Clear();
            Run();
            
        }
        private string maskedEntry()
        {
            string pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    //Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        //Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return pass;
        }
    }
}
