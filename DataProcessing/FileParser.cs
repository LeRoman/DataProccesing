using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DataProcessing
{
    public class FileParser
    {
       const char quotationStart = '“';
       const char quotationEnd = '”';
        public static void   Parse(FileInfo fileName)
        {
           

           using (StreamReader reader = new StreamReader(fileName.FullName))
            {
                
                string? line;
                List<string[]> lines = new List<string[]>();

                while ((line = reader.ReadLine()) != null)
                {

                    string[] rowElements = Split(line);
                    if (ValidateRow(rowElements))
                    {
                        lines.Add(rowElements);
                    }
                    else Console.WriteLine(line);
                }

            }

            

        }
        private static string[] Split(string line)
        {
            int firstComma = 0;
            int secondComma = 0;
            int quoteStack = 0;
            int elementsAmount = 0;
            string[] rowElements = new string[7];

            for (int i = 0; i < line.Length; i++)
            {
                if (elementsAmount == rowElements.Length) break;

                if (i == line.Length - 1)  // taking the last element in line
                {
                    rowElements[elementsAmount] = line.Substring(secondComma+1).Trim();
                    break;
                }

                if (line[i] == quotationStart || line[i] == quotationEnd) quoteStack += 1;
                if (line[i] == ',' && quoteStack % 2 == 0)
                {
                    firstComma = secondComma;
                    secondComma = i;
                        string element = line.Substring(firstComma, secondComma - firstComma);
                        rowElements[elementsAmount] = (element.StartsWith(',') ? element.Substring(1) : element).Trim();
                        elementsAmount++;

                }
                   
                
            }

            return rowElements;
        }

        static bool ValidateRow(string[] rowElements)
        {

            if ((!decimal.TryParse(rowElements[3], out decimal parsed)) ||
                (!DateTime.TryParse(rowElements[4], out DateTime date)) ||
                (!int.TryParse(rowElements[5], out int parsed1))) return false;
            return true;

        }
    }
}
