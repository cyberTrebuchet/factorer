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
            ulong totalFct = 0, // total factors of each tbf, to be discovered
                tbf = 0, // next to be factored
                tbf0 = 0; // to hold original value of tbf in the while loop

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
            tbf0 = tbf;

            // get list of primes
            // from https://docs.microsoft.com/en-us/dotnet/api/system.predicate-1?view=netcore-3.1
            Predicate<string> predicate = PrimeCheck;
            string[] oldPrimes = Array.FindAll(fileFcts, predicate);

            Array.ForEach(oldPrimes, ConWr);
            
            // update file output
            toWrite = toWrite + $"{tbf},";

            Console.WriteLine("Last factored from file:");
            Console.WriteLine(tbf - 1);

            if (tbf <= 4294967295) // crudely optimize for smaller values by recasting
            {
                if (tbf <= 65535)
                {
                    if (tbf <= 255)
                    {
                        tbf = (byte)tbf;
                        tbf0 = (byte)tbf0;
                        totalFct = (byte)totalFct;
                    } else {
                        tbf = (ushort)tbf;
                        tbf0 = (ushort)tbf0;
                        totalFct = (ushort)totalFct;
                    }
                } else {
                    tbf = (uint)tbf;
                    tbf0 = (uint)tbf0;
                    totalFct = (uint)totalFct;
                }
            }

            Console.WriteLine("Beginning with a factor of 2:");

            do // factor tbf
            {
                Array.ForEach(oldPrimes, delegate(string prime)
                {
                    ulong fct = Convert.ToUInt64(prime.Split(",")[0]),
                        expn = 0;

                    if (fct <= 4294967295) // crudely optimize for smaller values by recasting
                    {
                        if (fct <= 65535)
                        {
                            if (fct <= 255)
                            {
                                fct = (byte)fct;
                                expn = (byte)expn;
                            } else {
                                fct = (ushort)fct;
                                expn = (ushort)expn;
                            }
                        } else {
                            fct = (uint)fct;
                            expn = (uint)expn;
                        }
                    }

                    Console.WriteLine("Next factor to try:");
                    Console.WriteLine(fct);

                    /* If fct is a factor of tbf, then reassign tbf to its divisor with fct 
                     * and increment current exponent and total factor count */
                    if (tbf % fct == 0)
                    {
                        Console.WriteLine("Now factored down to:");
                        Console.WriteLine(tbf /= fct);
                        expn++;
                        totalFct++;
                        Console.WriteLine(expn);
                    }
                    else if (fct * fct > tbf0 && totalFct == 0)
                    {
                        // If, however, fct is already at least root tbf0
                        // and no prime factor has been found
                        totalFct = 1; // then tbf0 is prime
                    } else { expn = 0; } // otherwise, try next fct and reset expn to 0.
                });
                
            } while (totalFct < 1); /* changed from (tbf > 1) to break if tbf
             * isn't factored by the time fct is above root tbf0. The addition of
             * the ForEach loop on the primes necessitated a break solution in case
             * tbf itself is a new prime, otherwise infinite loop. */

            Console.WriteLine("Total factors:"); // of tbf
            Console.WriteLine(totalFct);

            toWrite = toWrite + $"{totalFct}\r\n";

            File.WriteAllText("factors.txt", toWrite);
        }
    }
}
