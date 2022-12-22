using System.Collections.Generic;

namespace MSE.instructionSets
{
    public abstract class Instruction
    {
        public readonly int len;

        public Instruction(int len)
        {
            this.len = len;
        }
        public abstract void Compile(Assembly.Assembler assembler, Assembly.DataContainer[] args);

    }
}