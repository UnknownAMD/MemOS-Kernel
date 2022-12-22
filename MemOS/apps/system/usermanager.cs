using MemOS.services;
using System;
using System.IO;
using System.Text;

namespace MemOS.apps.system
{
    public static class usermanager
    {

        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: User Manager");
        }
        public static void Help()
        {
            Console.WriteLine("Flags:\n -c  create a new user\n -r <user>  delete a user\n -l <user>  login to a user");
        }
        public static void Create()
        {
            Login l = new Login();
            l.Register();
        }
        public static void Delete(string flag)
        {
            Login l = new Login();

        }
        public static void Login(string s)
        {
            Login l = new Login();
            Kernel k = new Kernel();
            Console.Write("Password: ");
            string p = k.maskedEntry();
            l.validateLogin(p, s);
            Console.ReadKey();
        }
        public static void EnableLogin()
        {
            string key = "memos"; //temporary key
            string text = "true";
            var result = new StringBuilder();
            for (int c = 0; c < text.Length; c++)
                result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));
            File.Create(@"0\Users\conf.mec");
            File.WriteAllText(@"0\Users\conf.mec", result.ToString());
            Create();
        }
        public static void DisableLogin()
        {
            string key = "memos"; //temporary key
            string text = "false";
            var result = new StringBuilder();
            for (int c = 0; c < text.Length; c++)
                result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));
            File.Create(@"0\Users\conf.mec");
            File.WriteAllText(@"0\Users\conf.mec", result.ToString());
        }
        public static bool CheckLoginEnabled()
        {
            if (File.Exists(@"0:\Users\conf.mec"))
            {
                string key = "memos"; //temporary key
                string text = File.ReadAllText(@"0:\Users\conf.mec");
                var result = new StringBuilder();
                for (int c = 0; c < text.Length; c++)
                    result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));
                if (result.ToString() == "true") return true;
                else return false;
            }
            return false;
        }
    }
}

