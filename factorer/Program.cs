/*
 * Factorer in C#
 * v0.1.0001
 * by Britt Crozier
 * in Visual Studio 2019
 * 
 * distributed under
 * the terms and conditions of
 * the GNU General Public License 3.0 or later
 * 
 * This program calculates and records the prime factors
 * of the next x consecutive integers via file IO, limited
 * only by the upper bound of type ulong, with optimizations
 * for values accomodable by smaller types.
 */

using System;
using System.IO;

namespace Factorer
{
    static
    class Program
    {
        static ulong tbf0; // to hold original value of tbf, declared here
        // to make available within both PrimeCheck and Factor()
        static void Main(string[] args)
        {
            ulong userInput;

            Console.WriteLine("How many more numbers shall we factor today, Supreme Commander? ");

            userInput = Convert.ToUInt64(Console.ReadLine());

            Console.WriteLine($"\n{userInput} more number(s) coming right up!\n" +
                "\n\t~ Enjoy Your Knowledge ~\n");

            while (userInput > 0)
            {
                Factor();
                userInput--;
            }
        }
        // predicate delegate function for Array.FindAll in Factor()
        // from https://docs.microsoft.com/en-us/dotnet/api/system.array.findall?view=netcore-3.1
        // and https://docs.microsoft.com/en-us/dotnet/api/system.predicate-1?view=netcore-3.1
        static bool PrimeCheck(string fctd)
        {
            if (fctd == "") { return false; } // for last line of factors.txt
            int startIndex = fctd.Length - 1;
            string lastChar = fctd.Substring(startIndex, 1);
            
            if (lastChar == "\r") // trim \r ~ comment out for UNIX?
            {
                fctd = fctd.Substring(0, fctd.Length - 1); // trim last char
            }

            // return true if this fctd is prime (has only 1 prime factor)
            // and is no greater than half of tbf0
            string[] fctdA = fctd.Split(",");
            return Convert.ToUInt64(fctdA[1]) == 1 
                && Convert.ToUInt64(fctdA[0]) * 2 <= tbf0;
        }
        // string check delegate method for debugging
        // from https://docs.microsoft.com/en-us/dotnet/api/system.action-1?view=netcore-3.1
        static void ConWr(string str)
        {
            Console.WriteLine(str);
        }
        static void Factor()
        {
            ulong tbf, // next to be factored
                totalFct = 0; // total prime factors of tbf, to be discovered

            string toWrite = "";

            /* 
             * Open the text file using a stream reader.
             * For Windows Visual Studio 2019 build, file must be
             * in directory Factorer\bin\Debug\netcoreapp3.1\
             * A different path may be combined with the filename string
             * using Path.Combine() as argument to StreamReader()
             * From https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
             */
            using (StreamReader sr = new StreamReader("factors.txt"))
            {
                toWrite = sr.ReadToEnd();
            }

            // parse file contents into array of factored numbers, removing trailing \n
            string[] fileFcts = toWrite.Substring(0, toWrite.Length - 1).Split("\n");
            // assign tbf to the last factored number plus one
            tbf = Convert.ToUInt64(fileFcts[fileFcts.Length - 1].Split(",")[0]) + 1;
            tbf0 = tbf;

            // Crudely optimize for smaller values by recasting.
            // Omit any level of the following if tree,
            // if only greater numbers will be used:
            if (tbf <= 4294967295)
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

            // update file output
            toWrite = toWrite + $"{tbf},";

            Console.WriteLine("Next to factor:");
            Console.WriteLine(tbf);

            // get list of primes
            // from https://docs.microsoft.com/en-us/dotnet/api/system.predicate-1?view=netcore-3.1
            Predicate<string> predicate = PrimeCheck;
            string[] oldPrimes = Array.FindAll(fileFcts, predicate);

            // attempt to factor each prime fct into tbf
            foreach(string prime in oldPrimes)
            {
                if (tbf == 1) { break; } // break if it's already factored
                ulong fct = Convert.ToUInt64(prime.Split(",")[0]); // current prime factor

                // Crudely optimize for smaller values by recasting.
                // Omit any level of the following if tree,
                // if only greater numbers will be used:
                if (fct <= 4294967295)
                {
                    if (fct <= 65535)
                    {
                        if (fct <= 255)
                        {
                            fct = (byte)fct;
                        } else { fct = (ushort)fct; }
                    } else { fct = (uint)fct; }
                }

                Console.WriteLine("Next factor to try:");
                Console.WriteLine(fct);

                /* For as many times as fct is a factor of tbf,
                 * reassign tbf to its divisor with fct and increment
                 * current exponent and total factor count */
                while (tbf % fct == 0)
                {
                    Console.WriteLine("Now factored down to:");
                    Console.WriteLine(tbf /= fct);
                    totalFct++;
                }
            }

            // if no prime was a factor, then tbf was prime
            if (totalFct == 0) { totalFct++; }

            Console.WriteLine("Total factors:"); // of tbf
            Console.WriteLine(totalFct);

            toWrite = toWrite + $"{totalFct}\r\n";

            File.WriteAllText("factors.txt", toWrite);
        }
    }
}
