using System;
using System.IO;
using Sys = Cosmos.System;
using MemOS.services;
using MemOS.Emulation;
using MemOS.apps.system;
using System.Collections.Generic;

namespace MemOS
{
    public class Kernel : Sys.Kernel
    {
        public bool LoginEnabled;
        public bool isConsole = true; bool firstTimeUser = true;
        public User curuser;
        public bool isLoggedIn;
        public string version = "1.3.3";
        public string tryit;
        public static string curpath = @"0:\";
        public List<string> History;
        protected override void BeforeRun()
        {
            Console.WriteLine("Loading Kernel...");
            Console.ResetColor();
            MemOS.TryBoot();
            FGMSECInstructionSet.Install();
            diskrun.autorun();
            netrun.autorun();
            Console.Clear();
            MemOS.Welcome();
            LoginEnabled = usermanager.CheckLoginEnabled();
            if (!LoginEnabled) { curuser = new User() { username = "root", privLevel = privilege.System }; isLoggedIn = true; }
        }
        protected override void Run()
        {
            if (LoginEnabled) //If logins are enabled
            {
                if (isLoggedIn) //and we are logged in
                {
                    if (isConsole)
                        consoleLoop(); //do main loop
                    //else
                    //initGUI();
                }
                else //If we're not logged in
                    displayLogin(); //Display login page
            }
            else  //If logins aren't enabled
            {
                consoleLoop(); //do main loop
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
            while (isLoggedIn == false)
            {
                Console.Clear();
                MemOS.Welcome();
                Login l = new Login();
                Console.Write("Enter your username: ");
                string s = Console.ReadLine();
                Console.Write("Enter your password: ");
                string p = maskedEntry();
                Console.WriteLine(p +" "+ s);
                isLoggedIn = l.validateLogin(p, s);
                Console.ReadKey();
                Console.Clear();
            }
            MemOS.Welcome();
        }
        public string maskedEntry()
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
