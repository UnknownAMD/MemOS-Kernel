// Color app isnt finished ig ill add in the future

using System;

namespace MemOS.apps.basic
{
    public static class color
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS app: color");
        }
        public static void Main(string input)
        {
            var text = input.Substring(6);

            if (text.StartsWith("a"))
            {

            }
        }
        public static void empty()
        {
            Console.WriteLine("Color Manager 1.0");
            Console.WriteLine("0 = Black       8 = Gray");
            Console.WriteLine("1 = Blue        9 = Light Blue");
            Console.WriteLine("2 = Green       A = Light Green");
            Console.WriteLine("3 = Aqua        B = Light Aqua");
            Console.WriteLine("4 = Red         C = Light Red");
            Console.WriteLine("5 = Purple      D = Light Purple");
            Console.WriteLine("6 = Yellow      E = Light Yellow");
            Console.WriteLine("7 = White       F = Bright White");

            Console.WriteLine("Example: color a");
        }
    }
}

