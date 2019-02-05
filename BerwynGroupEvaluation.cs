using System;
using System.IO;
using System.Collections.Generic;

namespace TheBerwynGroupProject
{

    /*
     * Reads through an input spreadsheat with columns containing GUID values, 
     * Val1 values, Val2 values, and Val3 values.  Then outputs a spreasheet
     * with columns containing the GUID values, the sum of Val1 and Val2, Y or N
     * depending on whether the GUID value is a duplicate, and Y or N depending 
     * on whether the length of the Val3 string is longer than the average
     * length.   
     */
     class TheBerwynGroupProject
     {
     
        /*
         * Takes in lists to hold the GUID values, Val1 values, Val2 values, and 
         * Val3 values and reads through the input file, adding appropriate 
         * values to the lists.
         */
         private static void readInputFile(List<String> guid, List<int> val1, List<int> val2, List<String> val3)
         {
         
            using (StreamReader input = new StreamReader(@"test.csv"))
            {
            
                /*
                 * Gets the first row containing the column headers.
                 */
                String line = input.ReadLine();
        
                /*
                 * While there are rows remaining, reads each through each row 
                 * and gets the values of GUID, Val1, Val2, and Val3, and puts 
                 * them in the appropiate list.
                 */
                while (!input.EndOfStream)
                {
                    int i = 0;
                    line = input.ReadLine();
                    i = line.IndexOf(',');
                    String val = line.Substring(1, i - 2);
                    line = line.Substring(i + 3);
                    guid.Add(val);
                    i = line.IndexOf(',');
                    val = line.Substring(0, i - 1);
                    line = line.Substring(i + 3);
                    val1.Add(int.Parse(val));
                    i = line.IndexOf(',');
                    val = line.Substring(0, i - 1);
                    line = line.Substring(i + 3);
                    val2.Add(int.Parse(val));
                    val = line.Substring(0, line.Length - 1);
                    val3.Add(line);
                }

                /*
                 * Closes the input file stream.
                 */
                input.Close();
            }
        }

        /*
         * Main method
         */
        static void Main(string[] args)
        {
        
            /*
             * Creates lists to hold GUID values, Val1 values, Val2 values, and 
             * Val3 values and calls method to read through input file and and 
             * add appropiate values to the lists. 
             */
            List<String> guid = new List<String>();
            List<int> val1 = new List<int>();
            List<int> val2 = new List<int>();
            List<String> val3 = new List<String>();
            readInputFile(guid, val1, val2, val3);
 
            /*
             * Gets the number of elements of the GUID list, which is the same 
             * as the number of total records in the file.  Then outputs this 
             * value to the console.
             */
            int records = guid.Count;
            Console.WriteLine("Total number of records in file: " + records);
 
            /*
             * Calculates the average length of the Val3 values.
             */
            int val3Average = 0;
            for(int i = 0; i<val3.Count; i++)
            {
                val3Average += val3[i].Length;
            }
            val3Average /= val3.Count;
 
            /*
             * Initializes a largestSum value to keep track of the largest sum 
             * of Val1 and Val2 when all the records are iterated through.
             */
            int largestSum = val1[0] + val2[0];
 
            /*
             * Creates a list to put duplicate GUID values in.
             */
            List<String> duplicates = new List<String>();
 
            /*
             * Creates and writes to the output file.
             */
            using (StreamWriter output = new StreamWriter(@"testOutput.csv"))
            {
            
                /*
                 * Writes the column headings.
                 */
                output.WriteLine("\"GUID\", \"Val1 + Val2\", \"isDuplicateGuid " +
                                     "(Y or N)\", \"Is Val3 length greater than " +
                                     "the average length of Val3 (Y or N)\"");
 
                /*
                 * Iterates through the lists and writes a row for each record.
                 */
                for(int i = 0; i<guid.Count; i++)
                {
                
                    /*
                     * Checks a list of duplicated values to see if a GUID value 
                     * is a duplicate.  If a GUID value is not in the list, all 
                     * the remaining GUID values are checked for equality.  If 
                     * there is a match, that GUID value is added to the 
                     * duplicates list.
                     */
                    bool isDuplicateGuid = false;
                    if (!duplicates.Contains(guid[i]))
                    {
                        for(int j = i + 1; j<guid.Count; j++)
                        {
                            if(guid[i].Equals(guid[j]))
                             {
                                duplicates.Add(guid[i]);
                                isDuplicateGuid = true;
                             }
                        }
                    }
                    else
                    { 
                        isDuplicateGuid = true;
                    }

                    /*
                     * Writes the GUID of the current record in the GUID values 
                     * column.
                     */
                     output.Write("\"" + guid[i] + "\", ");
                     
                    /*
                     * Checks to see if the sum of Val1 and Val2 for the current 
                     * record is larger than the largestSum value.  If so, then 
                     * the largestSum value is replaced with the sum of Val1 
                     * and Val2 for the current record.
                     */
                    if(val1[i] + val2[i] > largestSum)
                    {
                        largestSum = val1[i] + val2[i];
                    }
 
                    /*
                     * Writes the value of Val1 + Val2 in the appropiate column.
                     */
                    output.Write("\"" + (val1[i] + val2[i]) + "\", ");
 
                    /*
                     * Writes Y or N in the appropiate column depending on 
                     * whether the current GUID value is a duplicate.
                     */
                    if (isDuplicateGuid)
                    {
                        output.Write("\"Y\", ");
                    }
                    else
                    {
                        output.Write("\"N\", ");
                    }
            
                    /*
                     * Writes Y or N in the appropiate column depending on 
                     * whether the length of Val3 is greater than the average 
                     * length of Val3.
                     */
                    if(val3[i].Length > val3Average)
                    {
                        output.Write("\"Y\", ");
                    }
                    else
                    {
                        output.Write("\"N\", ");
                    }
                    output.Write("\n");
                 }
 
                 /*
                  * Closes the output file stream.
                  */
                 output.Close();
             }
    
            /*
             * Writes to the console the largest sum of Val1 and Val2.
             */
            Console.WriteLine("Largest sum of Val1 and Val2: " + largestSum);
 
            /*
             * Writes to the console all the GUID values that are duplicates.
             */
            Console.WriteLine("Duplicate GUID values:\n");
            for(int i = 0; i<duplicates.Count; i++)
            {
                Console.WriteLine(duplicates[i]);
            }
            Console.WriteLine();
 
            /*
             * Writes to the console the average length of Val3.
             */
            Console.WriteLine("Average length of Val3: " + val3Average);
        }
    }
 }

