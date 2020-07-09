using System;

namespace Factorer
{
    static 
    class Program
    {
        static ulong Factor(ulong fct, ulong tbf) // fct is factor, tbf is to-be-factored user input
        {
            ulong expn = 0, totalFct = 0; // exponent of fct, to be discovered and returned

            do
            {
                /* If fct is a factor of tbf, then reassign tbf to its divisor with fct; otherwise,
                 * increment fct. */
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
