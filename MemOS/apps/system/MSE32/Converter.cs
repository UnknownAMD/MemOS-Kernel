using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MSE.Assembly;
using MSE.instructionSets;

namespace ASMToMSE32
{
    public class Converter
    {
        public string[] rawCode;
        private readonly List<string> includedFiles = new List<string>();
        readonly string filePath;
        string includePath;
        ISetBase InstructionSet;
        int functionsCount = 0;
        Dictionary<SegmentPlaceHolder, string> functionsPlaceholders = new Dictionary<SegmentPlaceHolder, string>();
        Dictionary<string, int> definedFunctions = new Dictionary<string, int>();

        public enum Registers
        {
            G0 = 0,
            G1 = 1,
            G2 = 2,
            G3 = 3,
            G4 = 4,
            G5 = 5,
            G6 = 6,
            G7 = 7,
            G8 = 8,
            G9 = 9,
            C1 = 10,
            C2 = 11,
            SP = 12,
            BP = 13,
            IP = 14
        }
        public Dictionary<string, byte> reg32 = new Dictionary<string, byte>()
        {
            { "G0", 0 },
            { "G1", 1 },
            { "G2", 2 },
            { "G3", 3 },
            { "G4", 4 },
            { "G5", 5 },
            { "G6", 6 },
            { "G7", 7 },
            { "G8", 8 },
            { "G9", 9 },
            { "C1", 10 },
            { "C2", 11 },
            { "SP", 12 },
            { "BP", 13 },
            { "IP", 14 }
        };
        public Dictionary<string, byte> reg16 = new Dictionary<string, byte>()
        {
            { "G0", 15 },
            { "G1", 16 },
            { "G2", 17 },
            { "G3", 18 },
            { "G4", 19 },
            { "G5", 20 },
            { "G6", 21 },
            { "G7", 22 },
            { "G8", 23 },
            { "G9", 24 },
        }; public Dictionary<string, byte> reg8 = new Dictionary<string, byte>()
        {
            { "G0", 25 },
            { "G1", 26 },
            { "G2", 27 },
            { "G3", 28 },
            { "G4", 29 },
            { "G5", 30 },
            { "G6", 31 },
            { "G7", 32 },
            { "G8", 33 },
            { "G9", 34 }
        };
        private string[] purify(string[] code)
        {
            for (int i = 0; i < code.Length; i++)
            {
                List<char> temp = new List<char>();
                temp.AddRange(code[i].Replace("\t", "").ToCharArray());
                for (int c = 0; c < temp.Count && temp[c] == ' ';)
                {
                    temp.RemoveAt(c);
                }
                if (temp.Count == 0)
                {
                    code[i] = "nop";
                }
                code[i] = new string(temp.ToArray());
            }
            return code;
        }
        DataContainer GetData(string placeholder, Assembler assembler)
        {
            DataContainer dataResult = null;
            if (placeholder.StartsWith("3$"))
            {
                dataResult = new Register(reg32[placeholder.Replace("3$", "").Replace(" ", "")]);
            }
            else if (placeholder.StartsWith("0x") || placeholder.EndsWith("h"))
            {
                placeholder = placeholder.Replace("0x", "").Replace("h", "");
                dataResult = new Data(int.Parse(placeholder, System.Globalization.NumberStyles.HexNumber));
            }
            else if (placeholder.StartsWith("[") && placeholder.EndsWith("]"))
            {
                placeholder = placeholder.Remove(0, 1).Remove(placeholder.Length - 2, 1);
                dataResult = new Pointer(GetData(placeholder, assembler));
            }
            else if (placeholder.StartsWith("%"))
            {
                string func = placeholder.Remove(0, 1);
                SegmentPlaceHolder placeHolder = new SegmentPlaceHolder(func, functionsCount);
                if (!functionsPlaceholders.ContainsKey(placeHolder))
                {
                    functionsPlaceholders.Add(new SegmentPlaceHolder(func, functionsCount), func);
                    functionsCount++;
                }
                dataResult = placeHolder;
            }
            else if (placeholder.StartsWith("\"") && placeholder.EndsWith("\""))
            {
                string STR = placeholder.Remove(0, 1).Remove(placeholder.Length - 2, 1).Replace("\\n", "\n");
                STR += '\0';
                dataResult = new Data(assembler.AddOnRAM(Encoding.UTF8.GetBytes(STR)));
            }
            return dataResult;
        }
        public string getCompilerVariable(string label)
        {
            string result = label;
            if (label.StartsWith("$"))
            {
                label = label.Remove(0, 1);
                switch (label.ToLower())
                {
                    case "filepath":
                        result = filePath;
                        break;
                    case "architecture":
                        result = "32bit";
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        public void CompilerCommand(string command)
        {
            if (command.StartsWith("include"))
            {
                List<string> code = new List<string>();
                code.AddRange(rawCode);
                string file = command.Split(' ')[1];
                if (file.StartsWith(":\\"))
                {
                    if (includedFiles.Contains(file)) return;
                    code.AddRange(purify(File.ReadAllLines(file)));
                    includedFiles.Add(file);
                }
                else
                {
                    if (includedFiles.Contains($"{includePath}\\{file}")) return;
                    code.AddRange(purify(File.ReadAllLines($"{includePath}\\{file}")));
                    includedFiles.Add($"{includePath}\\{file}");
                }
                rawCode = code.ToArray();
            }
            else if (command.StartsWith("cd"))
            {
                includePath = getCompilerVariable(command.Split(' ')[1]);
            }
        }
        public byte[] Compile()
        {
            Assembler assembler = new Assembler(Arch._32bit);
            for (int i1 = 0; i1 < rawCode.Length; i1++)
            {
                string instr = rawCode[i1];
                string temp = "";
                List<DataContainer> args = new List<DataContainer>();
                char c;
                int i = 0;
                if (instr.StartsWith("!"))
                {
                    CompilerCommand(instr.Remove(0, 1));
                    continue;
                }
                if (instr.EndsWith(":"))
                {
                    instr = instr.Remove(instr.Length - 1, 1);
                    if (!definedFunctions.ContainsKey(instr))
                        definedFunctions.Add(instr, assembler.opCode.Count);
                    else
                        definedFunctions[instr] = assembler.opCode.Count;
                    assembler.NonOP();
                    continue;
                }
                for (; i < instr.Length; i++)
                {
                    char s = instr[i];
                    if (s == ' ') { i++; break; }
                    else temp += s;
                }
                Instruction InstrInstance = InstructionSet.Decoder(temp.ToUpper());
                int skip = 0;
                bool quotes = false;
                for (int l = 0; l <= InstrInstance.len; l++)
                {
                    temp = "";
                    for (; i < instr.Length; i++)
                    {
                        char s = instr[i];
                        if (s == '"' && quotes is false) { skip++; quotes = true; }
                        else if (s == '"' && quotes is true) { skip--; quotes = false; }
                        if (s == ',' && skip == 0) { i += 2; break; }
                        else temp += s;
                    }
                    args.Add(GetData(temp, assembler));
                }
                if (args.Count < InstrInstance.len)
                {
                    Console.WriteLine($"One or more instructions require more arguments");
                    return new byte[] { 0x00 };
                }
                else if (args.Count - 1 > InstrInstance.len)
                {
                    Console.WriteLine($"One or more instructions require less arguments");
                    return new byte[] { 0x00 };
                }
                InstrInstance.Compile(assembler, args.ToArray());
            }
            uint prefix = 0xFF001AFA;
            for (int i = assembler.opCode.IndexOf((int)prefix); i != -1; i = assembler.opCode.IndexOf((int)prefix))
            {
                uint pref = 0xFE001AFB;
                assembler.opCode[i] = (int)pref;
                foreach (KeyValuePair<SegmentPlaceHolder, string> holder in functionsPlaceholders)
                {
                    if (holder.Key.GetOpInInts()[1] == assembler.opCode[i + 1])
                    {
                        assembler.opCode[i + 1] = definedFunctions[holder.Key.name];
                    }
                }
            }
            return assembler.Compile();
        }
        public Converter(string fileName, ISetBase InstructionSet = null)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Could not locate given file!");
                throw new Exception();
            }
            filePath = Directory.GetParent(fileName).FullName;
            rawCode = purify(File.ReadAllLines(fileName));
            if (InstructionSet is null)
            {
                this.InstructionSet = new FGMSECInstructionSet();
            }
            else
            {
                this.InstructionSet = InstructionSet;
            }    
        }
    }
}
