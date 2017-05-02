﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SimilatiryMeasures
{
    public class CsvReader
    {
        private static readonly char[] Delimiters = { ',', ';' };

        public static IDictionary ReadConnections()
        {
            try
            {
                using (StreamReader reader = new StreamReader(@"data.csv"))
                {
                    string[] fields;

                    var dataDictornary = new Dictionary<int, Dictionary<double, double>>();

                    while (true)
                    {
                        //Breaks out when at the end
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }

                        //Splits the rows at the delimiters char
                        fields = line.Split(Delimiters);

                        //If the key does not exist yet in the dictionary, add a new empty nested dictionary and then a value into the new nested dictionary
                        if (!dataDictornary.ContainsKey(Convert.ToInt16(fields[0])))
                        {
                            dataDictornary.Add(Convert.ToInt16(fields[0]), new Dictionary<double, double>());
                            dataDictornary[Convert.ToInt16(fields[0])].Add(Convert.ToDouble(fields[1]), Convert.ToDouble(fields[2]));
                        }
                        //Else add a new value to the nested dictionary
                        else
                        {
                            dataDictornary[Convert.ToInt16(fields[0])].Add(Convert.ToDouble(fields[1]), Convert.ToDouble(fields[2]));
                        }


                    }
                    Console.WriteLine("Done Reading CSV Data");
                    return dataDictornary;
                }
            }
            //CSV file could not be found
            catch (FileNotFoundException f)
            {
                Console.WriteLine("CSV-File not found \n Exception: {0}", f.Message);
                return null;
            }
            //CSV file could not be accessed
            catch (IOException ioException)
            {
                Console.WriteLine("Cannot open CSV file, please make sure it is not opened in your Excel or any other Editor \n Exception: {0}",
                   ioException);
                return null;
            }
            //Invalid convert cast
            catch (InvalidCastException icException)
            {
                Console.WriteLine("Invalid data input, please input integers and/or doubles \n Exception: {0}",
                    icException);
                return null;
            }
            //Other exceptions
            catch (Exception e)
            {
                Console.WriteLine("Error {0}", e.StackTrace);
                return null;
            }
        }
    }
}