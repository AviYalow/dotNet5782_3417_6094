using System;

namespace Targil0
{
   partial class Program
    {
        static void Main(string[] args)
        {
            Welcome3417();
            Welcome6094();
           
            Console.ReadKey();
        }

        static partial void Welcome6094();

        private static void Welcome3417()
        {
            Console.WriteLine("Enter your name: ");
            string userName = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", userName);
        }
   
    }
}
