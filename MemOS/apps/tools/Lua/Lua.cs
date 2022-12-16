using Neo.IronLua;
using System;

namespace MemOS.apps.tools
{
    public static class LuaInterpreter
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS Tool: Lua Interpreter");
        }
        public static void Execute(string path)
        {
            Lua lua = new Lua();
            lua.CreateEnvironment().DoChunk("",path); // Create a environment and execute
        }
        public static void Compile(string path,LuaCompileOptions options)
        {
            Lua lua = new Lua();
            lua.CompileChunk(path, options);
        }
    }
}
