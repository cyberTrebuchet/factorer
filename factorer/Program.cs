/*
 * Factorer in C#
 * v0.0.0101
 * by Britt Crozier
 * in Visual Studio 2019
 * 
 * distributed under
 * the terms and conditions of
 * the GNU General Public License 3.0 or later
 * 
 * This program calculates and records the prime factors
 * of the next x consecutive integers via file IO, limited
 * only by the upper bound of type ulong.
 */

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

            Console.WriteLine("How many more numbers shall we factor today, Supreme Commander? ");

            userInput = Convert.ToUInt64(Console.ReadLine());

            Console.WriteLine(userInput);

            do
            {
                Factor();
                userInput--;
            } while (userInput > 0);
        }
        // predicate delegate function for Array.FindAll in Factor()
        // from https://docs.microsoft.com/en-us/dotnet/api/system.array.findall?view=netcore-3.1
        // and https://docs.microsoft.com/en-us/dotnet/api/system.predicate-1?view=netcore-3.1
        static bool PrimeCheck(string fctd)
        {
            if (fctd == "") { return false; } // for last line of factors.txt
            int startIndex = fctd.Length - 1;
            string lastChar = fctd.Substring(startIndex, 1);
            
            if (lastChar == "\r") // trim \r - comment out for UNIX?
            {
                fctd = fctd.Substring(0, fctd.Length - 1); // trim last char
                Console.WriteLine($"Trimmed carriage return on {fctd}");
            }
            // return true if this fctd has only 1 prime factor
            return Convert.ToInt16(fctd.Split(",")[1]) == 1;
        }
        // string check delegate
        // from https://docs.microsoft.com/en-us/dotnet/api/system.action-1?view=netcore-3.1
        static void ConWr(string str)
        {
            Console.WriteLine(str);
        }
        static void Factor()
        {
            ulong fct = 2, // current factor for looping
                expn = 0, // exponent of each factor
                totalFct = 0, // total factors of each tbf, to be discovered
                tbf = 0; // next to be factored

            string toWrite = "";

            // Open the text file using a stream reader.
            // From https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
            using (StreamReader sr = new StreamReader("factors.txt"))
            {
                toWrite = sr.ReadToEnd();
            }

            // parse file contents for last entry and primes, removing trailing \n
            string[] fileFcts = toWrite.Substring(0, toWrite.Length - 1).Split("\n");
            // assign tbf to the last factored number plus one
            tbf = Convert.ToUInt64(fileFcts[fileFcts.Length - 1].Split(",")[0]) + 1;

            // get list of primes
            // from https://docs.microsoft.com/en-us/dotnet/api/system.predicate-1?view=netcore-3.1
            Predicate<string> predicate = PrimeCheck;
            string[] oldPrimes = Array.FindAll(fileFcts, predicate);

            Array.ForEach(oldPrimes, ConWr);
            
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
                    // factor only primes

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
