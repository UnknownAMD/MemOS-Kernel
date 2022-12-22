using MSE.Assembly;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSE.instructionSets
{
    class FGMSECInstruction: Instruction
    {
        public delegate void FGMSECInstructionDelegate(Assembler assembler, DataContainer[] args);
        FGMSECInstructionDelegate CompileDelegate;
        public FGMSECInstruction(int len, FGMSECInstructionDelegate CompileDelegate = null): base(len)
        {
            if (CompileDelegate is null)
                this.CompileDelegate = new FGMSECInstructionDelegate((Assembler assembler, DataContainer[] args) => { });
            else
                this.CompileDelegate = CompileDelegate;
        }
        public override void Compile(Assembler assembler, DataContainer[] args)
        {
            CompileDelegate(assembler, args);
        }
    }
    class FGMSECInstructionSet: ISetBase
    {
        public FGMSECInstructionSet() : base(new Dictionary<string, Instruction>() {
            { "NOP", new FGMSECInstruction(0, (Assembler assembler, DataContainer[] args) => { assembler.NonOP(); }) },

            { "MOV", new FGMSECInstruction(2, (Assembler assembler, DataContainer[] args) => { assembler.Move(args[0], args[1]); }) },
            { "SET", new FGMSECInstruction(2, (Assembler assembler, DataContainer[] args) => { assembler.Set(args[0], args[1]); }) },
            { "ADD", new FGMSECInstruction(2, (Assembler assembler, DataContainer[] args) => { assembler.Sum(args[0], args[1]); }) },
            { "SUB", new FGMSECInstruction(2, (Assembler assembler, DataContainer[] args) => { assembler.Subtract(args[0], args[1]); }) },
            { "DIV", new FGMSECInstruction(2, (Assembler assembler, DataContainer[] args) => { assembler.Divide(args[0], args[1]); }) },
            { "MUL", new FGMSECInstruction(2, (Assembler assembler, DataContainer[] args) => { assembler.Multiply(args[0], args[1]); }) },

            { "JMP", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.JumpToSegment(args[0]); }) },
            { "CALL", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.CallSegment(args[0]); }) },
            { "RET", new FGMSECInstruction(0, (Assembler assembler, DataContainer[] args) => { assembler.Return(); }) },
            { "SCALL", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.SystemCall(args[0]); }) },

            { "END", new FGMSECInstruction(0, (Assembler assembler, DataContainer[] args) => { assembler.End(); }) },

            { "CMP", new FGMSECInstruction(2, (Assembler assembler, DataContainer[] args) => { assembler.Compare(args[0], args[1]); }) },
            { "EQU", new FGMSECInstruction(2, (Assembler assembler, DataContainer[] args) => { assembler.Equal(args[0], args[1]); }) },

            { "JE", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.JumpIf(args[0]); }) },
            { "JNE", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.JumpIfNot(args[0]); }) },

            { "CE", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.CallIf(args[0]); }) },
            { "CNE", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.CallIfNot(args[0]); }) },

            { "SCE", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.SystemCallIf(args[0]); }) },
            { "SCNE", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.SystemCallIfNot(args[0]); }) },

            { "RE", new FGMSECInstruction(0, (Assembler assembler, DataContainer[] args) => { assembler.ReturnIf(); }) },
            { "RNE", new FGMSECInstruction(0, (Assembler assembler, DataContainer[] args) => { assembler.ReturnIfNot(); }) },

            { "EE", new FGMSECInstruction(0, (Assembler assembler, DataContainer[] args) => { assembler.EndIf(); }) },
            { "ENE", new FGMSECInstruction(0, (Assembler assembler, DataContainer[] args) => { assembler.EndIfNot(); }) },

            { "PUSH", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.Push(args[0]); }) },
            { "POP", new FGMSECInstruction(1, (Assembler assembler, DataContainer[] args) => { assembler.Pop(args[0]); }) },
        })
        {

        }
    }
}
