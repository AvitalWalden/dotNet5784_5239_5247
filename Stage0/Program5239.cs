using System;
namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome5239();
            Welcome5247();
            Console.ReadKey();
        }

        static partial void Welcome5247();
        private static void Welcome5239()
        {
            Console.Write("Enter your name: ");
            string userName = Console.ReadLine()!;
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }
    }
}