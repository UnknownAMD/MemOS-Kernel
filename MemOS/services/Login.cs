using Cosmos.Common.Extensions;
using MemOS.apps.system;
using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static MemOS.Kernel;

namespace MemOS.services
{
        public enum privilege
        {
            User,
            SuperUser,
            System
        }
        public class User
        {
            public string username { get; set; }
            public privilege privLevel { get; set; }
        }
    public class Login
    {
        private string password = "";
        private byte[] GetHash(string inputString)
        {
            byte[] result;
            var shaM = new SHA1Managed();
            result = shaM.ComputeHash(Encoding.ASCII.GetBytes(inputString));
            return result;
        }

        /*private static string GetHashString(string inputString)
        { 
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }*/

        public void createUser(string usern, string passw, privilege privlevel)
        {
            if (Directory.Exists(@"0:\Users\")) if (usern != "root" && !File.Exists($@"0:\Users\{usern}.usr")) { File.WriteAllText($@"0:\Users\{usern}.usr", $"$pswd:{passw}$privlevel:{privlevel}"); } else { ErrorHandle.ThrowError($"ERROR!: Cannot create user {usern} because user already exists!"); }
            else
            {
                Directory.CreateDirectory(@"0:\Users\"); File.WriteAllText($@"0:\Users\{usern}.usr", $"user:{usern}$pswd:{passw}$privlevel:{(int)privlevel}");
            }
        }

        public void list()
        {
            Console.WriteLine("Users on this computer:");
            foreach (string l in Directory.GetFiles(@"0:\Users\"))
            {
                Console.WriteLine("Username: " + parseUser(l).username);
            }
        }

        public bool validateLogin(string paswd, string usern)
        {
            Console.WriteLine(usern);
            var user = parseUser(usern);
            if (user != null)
            {
                if (password == paswd)
                {
                    var kernel = new Kernel();
                    kernel.curuser = user;
                    ErrorHandle.ThrowSuccess("Logged in successfuly");
                    return true;
                }
                else
                {
                    ErrorHandle.ThrowError("Wrong Password or Username!");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private User parseUser(string User)
        {
            Console.WriteLine(User);
            if (File.Exists($@"0\Users\{User}.usr"))
            {
                string[] token = File.ReadAllText($@"0\Users\{User}.usr").Split('$');
                Console.WriteLine(token[1] + " " + token[1].Split(':')[1]);
                if (token[1].Split(':')[1] == "#NAL#") password = ""; else password = token[1].Split(':')[1];
                return new User() { username = User, privLevel = (privilege)int.Parse(token[2].Split(':')[1]) };
            }
            ErrorHandle.ThrowError($"ERROR!: Unknown user {User}");
            return null;
        }
        public User Register()
        {
            var kernel = new Kernel();
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var passwd = kernel.maskedEntry();
            if (kernel.curuser.username != "root") { createUser(username, passwd, privilege.User); return new User() { username = username, privLevel = privilege.User }; }
            else
            {
                superuserquestion:
                Console.Write("Superuser (y/n): "); string read = Console.ReadLine();  if (read == "y") { createUser(username, passwd, privilege.SuperUser); return new User() { username = username, privLevel = privilege.SuperUser }; }
                else if (read == "n") { createUser(username, passwd, privilege.User); return new User() { username = username, privLevel = privilege.User }; }
                else ErrorHandle.ThrowError($"ERROR!: Unknown option {read}"); Console.Clear(); goto superuserquestion;
            }

        }
        string EncryptOrDecrypt(string text, string key)
        {
            var result = new StringBuilder();

            for (int c = 0; c < text.Length; c++)
                result.Append((char)((uint)text[c] ^ (uint)key[c % key.Length]));

            return result.ToString();
        }
    }
}
