using Cosmos.Common.Extensions;
using MemOS.apps.system;
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
    public class Login
    {
        private string password = "", user = ""; public int group = 0;
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

        public void createUser(string[] logins, string usern, string passw, int group)
        { 
            File.AppendAllText(@"0:\users.dat", "user: " + usern + "$pswd: " + passw + "$group: " + group);
        }

        public void list(string[] logins)
        {
            Console.WriteLine("Users on the computer:");
            foreach (string l in logins)
            {
                parseToken(l);
                Console.WriteLine("Username: " + user);
            }
        }

        public bool validateLogin(string[] logins, string paswd, string usern)
        {
            var kernel = new Kernel();
            Console.WriteLine(user + " " + password); 
            parseToken(logins[0]);
            if (user == usern && password == paswd)
            {
                ErrorHandle.ThrowSuccess("Logged in successfuly");
                return true;
            }
            else
            {
                ErrorHandle.ThrowWarn("Wrong Password or Username!");
                return false;
            }
        }

        private void parseToken(string entry)
        {
            string[] token = entry.Split('$');
            user = token[0].Split(':')[1];
            if (token[1].Split(':')[1] == "#NAL#") password = ""; else password = token[1].Split(':')[1];
            group = getGroup(token[3].Split(':')[1]); 
        }

        private int getGroup(string group)
        {
            switch (group)
            {
                case "00": return 0;
                case "01": return 1;
                default: return 1;
            }
        }
    }
}
