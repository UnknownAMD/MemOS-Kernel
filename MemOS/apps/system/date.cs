using System;

namespace MemOS.apps.system
{
    public static class date
    {
        public static void TryCheck()
        {
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  OK  ");
            Console.ResetColor();
            Console.WriteLine("] MemOS System Date:");
        }
        public static void Main()
        {
            Console.WriteLine("Date: " + Cosmos.HAL.RTC.DayOfTheMonth + "/" + Cosmos.HAL.RTC.Month + "/" + "20" + Cosmos.HAL.RTC.Year);
            Console.WriteLine("Time: " + Cosmos.HAL.RTC.Hour + ":" + Cosmos.HAL.RTC.Minute + ":" + Cosmos.HAL.RTC.Second);
            Console.WriteLine("Notice: Date Format is dd/mm/yyyy");
            Console.WriteLine("Notice: Time is 24h format");
        }
        public static void shortforsys()
        {
            Console.WriteLine("Kernel Date:");
            Console.WriteLine("Date: " + Cosmos.HAL.RTC.DayOfTheMonth + "/" + Cosmos.HAL.RTC.Month + "/" + "20" + Cosmos.HAL.RTC.Year);
            Console.WriteLine("Time: " + Cosmos.HAL.RTC.Hour + ":" + Cosmos.HAL.RTC.Minute + ":" + Cosmos.HAL.RTC.Second);
        }
    }
}

