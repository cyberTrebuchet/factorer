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

            Console.WriteLine("Enter a positive integer up to 18,446,744,073,709,551,615 to be factored: ");

            userInput = Convert.ToUInt64(Console.ReadLine());

            Console.WriteLine(userInput);

            // Factor(userInput);

            Console.WriteLine(Factor(2, userInput));
        }
        // fct is factor, tbf is to-be-factored user input
        static ulong Factor(ulong fct, ulong tbf) 
        {
            ulong expn = 0, // exponent of each factor
                totalFct = 0; // total factor count, to be discovered and returned

            /* From https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-write-text-to-a-file#example-write-and-append-text-with-the-file-class
             * Note that docPath is assigned to ~\OneDrive\Documents\ */
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                toWrite = ""; // for LATER

            /*
            do // prompt to save to file
            {
                Console.WriteLine("Save to file? y/n ");
                saveFile = Console.ReadLine().ToLower();
            } while (saveFile != "y" && saveFile != "n");

            if (saveFile == "y")
            {
                /* From https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-write-text-to-a-file#example-write-and-append-text-with-the-file-class
                 * (need string for text)
                
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                File.WriteAllText(Path.Combine(docPath, "factors.txt"), text);
                
            }*/

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
            } else {
                Console.WriteLine("Sticking with ulong!"); 
            }

            Console.WriteLine("Beginning with a factor of 2:");

            do
            {
                /* If fct is a factor of tbf, then reassign tbf to its divisor with fct 
                 * and increment current exponent and total factor count; otherwise, 
                 * increment fct and reset expn to 0. */
                if (tbf % fct == 0)
                {
                    Console.WriteLine("Just got factored down to:");
                    Console.WriteLine(tbf /= fct);
                    expn++;
                    totalFct++;
                    Console.WriteLine(expn);
                } else {
                    // expn > 0 ? 
                    
                    Console.WriteLine("Next factor to try:");
                    fct++;
                    Console.WriteLine(fct);
                    expn = 0;
                };
            } while (tbf > 1);

            Console.WriteLine("Total factors:");
            Console.WriteLine(totalFct);



            // Open the text file using a stream reader.
            // From https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
            using (StreamReader sr = new StreamReader("factors.txt"))
            {
                // Read the stream to a string, and write the string to the console
                toWrite = sr.ReadToEnd();
                Console.WriteLine("toWrite within SR:");
                Console.WriteLine(toWrite);
            }

            Console.WriteLine("toWrite outside SR:");
            Console.WriteLine(toWrite);

            // File.WriteAllText(Path.Combine(docPath, "factors.txt"), toWrite);

            return totalFct;
        }
    }
}
