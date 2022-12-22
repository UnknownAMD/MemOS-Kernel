using System;
using MemOS.Emulation.Threading;
using System.Collections.Generic;
using MemOS.apps.system;

namespace MemOS.Emulation
{
    public class Execute
    {
        public static void RunEXE(byte[] data, params string[] parameters)
        {
            FGMSECInstructionSet set = new FGMSECInstructionSet();
            Executable Task;
            Task = new Executable(data, set, 3);
            Task.ReadData();
            if (parameters.Length > 0)
            {
                int addr = Task.Memory.Data.Count;
                for (int i = 0; i < parameters.Length; i++)
                {
                    foreach (char c in parameters[i])
                        Task.Memory.AddChar(c);
                    Task.Memory.AddChar(0);
                }
                Task.Memory.Stack.Push(addr);
                Task.Memory.Stack.Push(parameters.Length - 1);
            }
            SystemCalls.AddSystemCalls(Task);
            while (Task.running)
                TaskManager.Next();
            int code = 0;
            Task.Memory.Stack.Try_Pop(ref code);
            if (code != 0)
            ErrorHandle.ThrowError($"ERROR!: Program exited with abnormal code {code}");
        }
    }
}
