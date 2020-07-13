using System;
using System.IO;

namespace Factorer
{
    static 
    class Program
    {
        static void Main(string[] args)
        {
            ulong userInput;

            // Console.WriteLine("Enter a positive integer up to 18,446,744,073,709,551,615 to be factored: ");

            Console.WriteLine("How many more numbers shall we factor today, Supreme Commander? ");

            userInput = Convert.ToUInt64(Console.ReadLine());

            Console.WriteLine(userInput);

            do
            {
                Factor();
                userInput--;
            } while (userInput > 0);
        }
        
        static void Factor()
        {
            ulong fct = 2,
                expn = 0, // exponent of each factor
                totalFct = 0, // total factor count, to be discovered
                tbf = 0; // next to be factored

            string toWrite = "";

            // Open the text file using a stream reader.
            // From https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
            using (StreamReader sr = new StreamReader("factors.txt"))
            {
                toWrite = sr.ReadToEnd();
            }

            // parse file contents for last entry
            string[] fileFcts = toWrite.Split("\n");
            tbf = Convert.ToUInt64(fileFcts[fileFcts.Length - 2].Split(",")[0]) + 1;
            
            // update file output
            toWrite = toWrite + $"{tbf},";

            Console.WriteLine("Last factored from file:");
            Console.WriteLine(tbf - 1);

            if (tbf <= 4294967295) // crudely optimize for smaller user input values
            {
                if (tbf <= 65535)
                {
                    if (tbf <= 255)
                    {
                        tbf = (byte)tbf;
                        fct = (byte)fct;
                        expn = (byte)expn;
                        totalFct = (byte)totalFct;
                        Console.WriteLine("Switched to byte!");
                    } else {
                        tbf = (ushort)tbf;
                        fct = (ushort)fct;
                        expn = (ushort)expn;
                        totalFct = (ushort)totalFct;
                        Console.WriteLine("Switched to ushort!");
                    }
                } else {
                    tbf = (uint)tbf;
                    fct = (uint)fct;
                    expn = (uint)expn;
                    totalFct = (uint)totalFct;
                    Console.WriteLine("Switched to uint!");
                }
            } else {
                Console.WriteLine("Sticking with ulong!"); 
            }

            Console.WriteLine("Beginning with a factor of 2:");

            do
            {
                /* If fct is a factor of tbf, then reassign tbf to its divisor with fct 
                 * and increment current exponent and total factor count; otherwise, 
                 * try next fct and reset expn to 0. */
                if (tbf % fct == 0)
                {
                    Console.WriteLine("Now factored down to:");
                    Console.WriteLine(tbf /= fct);
                    expn++;
                    totalFct++;
                    Console.WriteLine(expn);
                } else {
                    Console.WriteLine("Next factor to try:");
                    fct++; // extremely wasteful as only primes need be tried; need list of primes
                    Console.WriteLine(fct);
                    expn = 0;
                };
            } while (tbf > 1);

            Console.WriteLine("Total factors:");
            Console.WriteLine(totalFct);

            toWrite = toWrite + $"{totalFct}\r\n";

            File.WriteAllText("factors.txt", toWrite);
        }
    }
}
