using System;

namespace Factorer
{
    static 
    class Program
    {
        static ulong Factor(ulong fct, ulong tbf) // fct is factor, tbf is to-be-factored user input
        {
            ulong expn = 0, // exponent of each factor
                totalFct = 0; // total factor count, to be discovered and returned
            
            if (tbf <= 4294967295) // optimize for smaller user input values
            {
                if (tbf <= 65535)
                {
                    if (tbf <= 255)
                    {
                        tbf = (byte)tbf;
                        Console.WriteLine("Switched to byte!");
                    } else {
                        tbf = (ushort)tbf;
                        Console.WriteLine("Switched to ushort!");
                    }
                } else {
                    tbf = (uint)tbf;
                    Console.WriteLine("Switched to uint!");
                }
            } else { Console.WriteLine("Sticking with ulong!"); }
            

            do
            {
                /* If fct is a factor of tbf, then reassign tbf to its divisor with fct and increment
                 * current exponent and total factor count; otherwise, increment fct and reset expn to 0. */
                if (tbf % fct == 0)
                {
                    Console.WriteLine("Just got factored down to:");
                    Console.WriteLine(tbf /= fct);
                    expn++;
                    totalFct++;
                    Console.WriteLine(expn);
                } else {
                    Console.WriteLine("Next factor to try:");
                    fct++;
                    Console.WriteLine(fct);
                    expn = 0;
                };
            } while (tbf > 1);

            return totalFct;
        }
        static void Main(string[] args)
        {
            ulong userInput;

            Console.WriteLine("Enter a positive integer up to 18,446,744,073,709,551,615 to be factored: ");

            userInput = Convert.ToUInt64(Console.ReadLine());

            Console.WriteLine(userInput);

            // Factor(userInput);

            Console.WriteLine(Factor(2, userInput));
        }
    }
}
