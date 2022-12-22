using System;
using System.IO;
using System.Collections.Generic;
using MSE.Assembly;
using MSE;
using MSE.instructionSets;
using MemOS.apps.system;

namespace ASMToMSE32
{
    public class ASMCompiler
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: cat");
        }
        public static void Compile(string ASMFILE)
        {
            if (ASMFILE.EndsWith(".asm"))
            {
                Converter converter = new Converter(ASMFILE);
                File.WriteAllBytes($"{ASMFILE.Split('.')[0]}.mem", converter.Compile());
                Console.WriteLine("Compile complete!");
            }
            else Help();
        }
        public static void CompileDir(string path)
        {
            foreach (string ASMFILE in Directory.GetFiles(path))
            {
                if (ASMFILE.EndsWith(".asm"))
                {
                    Converter converter = new Converter(ASMFILE);
                    File.WriteAllBytes($"{ASMFILE.Split('.')[0]}.mem", converter.Compile());
                    Console.WriteLine("Compile complete!");
                }
            }
        }
        public static void Help()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("compile -h\ncompile <ASMFILE.asm>\ncompile -d <PATH>");
        }
    }
}
