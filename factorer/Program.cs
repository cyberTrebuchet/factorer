using System;

namespace factorer
{
    class Program
    {
        static void Main(string[] args)
        {
            ulong userInput;

            Console.WriteLine("Enter a positive integer up to 18,446,744,073,709,551,615 to be factored: ");

            userInput = Convert.ToUInt64(Console.ReadLine());

            Console.WriteLine(userInput);
        }
    }
}
