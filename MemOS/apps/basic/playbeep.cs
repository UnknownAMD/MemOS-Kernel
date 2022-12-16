using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using System.Drawing;
using Cosmos.Core.IOGroup;
using System.Threading;
using Cosmos.HAL;
using Cosmos.Core;
using Cosmos.Common;
using System.Net;
using Cosmos.System.Network;


namespace MemOS.apps.basic
{
    public static class playbeep
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: playbeep");
        }
        public static void Main()
        {
            Console.WriteLine("BEEP!");
            Console.Beep();
            Console.WriteLine("BEEP!");
            Console.Beep();
            Console.WriteLine("BEEP!");
            Console.Beep();
            Console.WriteLine("BEEP!");
            Console.Beep();
            Console.WriteLine("BEEP!");
            Console.Beep();
        }
    }
}

