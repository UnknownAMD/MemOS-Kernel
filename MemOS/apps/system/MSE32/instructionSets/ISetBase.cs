using System;
using System.Collections.Generic;
using System.Text;

namespace MSE.instructionSets
{
    public delegate void InstructionDelegate(Assembly.Assembler assembler, uint[] args);
    public abstract class ISetBase
    {
        Dictionary<string, Instruction> Instructions = null;
        public Instruction Decoder(string inst)
        {
            if (Instructions is null)
                throw new Exception("Instruction not handled");
            if (inst == "") return Instructions["NOP"];
            return Instructions[inst];
        }
        public ISetBase(Dictionary<string, Instruction> Instructions)
        {
            this.Instructions = Instructions;
        }
    }
}
