using MemOS.Emulation;
using System;
using MemOS.services;

namespace MemOS.Emulation
{
    public class SystemCalls
    {
        public static void AddSystemCalls(Executable Task)
        {
            Task.AddSystemCall((Executable caller) =>
            {
                int addr = (int)((FGMSECInstructionSet)caller.usingInstructionSet).CPU.GetRegData(3);
                char c = (char)caller.Memory.ReadChar(addr);
                while (c != 0)
                {
                    Console.Write(c);
                    addr++;
                    c = (char)caller.Memory.ReadChar(addr);
                }
            });
            Task.AddSystemCall((Executable caller) =>
            {
                string input = Console.ReadLine();
                input += '\0';
                int addr = caller.Memory.Data.Count;
                int caddr = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    char c = input[i];
                    if (!caller.Memory.AddChar(c))
                    {
                        caller.Memory.WriteChar(caddr, c);
                        addr = 0;
                        caddr++;
                    }
                }
              ((FGMSECInstructionSet)caller.usingInstructionSet).CPU.SetRegData(3, (uint)addr);
            });
            Task.AddSystemCall((Executable caller) =>
            {
                Console.Clear();
            });
            Task.AddSystemCall((Executable caller) =>
            {
                if (caller.RunningUser.privLevel != privilege.User)
                {
                    ((FGMSECInstructionSet)caller.usingInstructionSet).CPU.SetRegData(3, 1);
                    return;
                }
                if (caller.RunningUser.privLevel > privilege.SuperUser)
                {
                    caller.RunningUser = new User() { username = "root", privLevel = privilege.System };
                    ((FGMSECInstructionSet)caller.usingInstructionSet).CPU.SetRegData(3, 1);
                }
                else
                {
                    ((FGMSECInstructionSet)caller.usingInstructionSet).CPU.SetRegData(3, 0);
                }

            });
            Task.AddSystemCall((Executable caller) =>
            {
                var username = caller.RunningUser.username;
                int addr = caller.Memory.Data.Count;
                foreach (char c in username)
                    caller.Memory.AddChar(c);
                caller.Memory.AddChar(0);
                ((FGMSECInstructionSet)caller.usingInstructionSet).CPU.SetRegData(3, (uint)addr);
            });
        }
    }
}
