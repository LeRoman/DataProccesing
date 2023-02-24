using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class FileParser
    {
        const char quotationStart = '“';
        const char quotationEnd = '”';
        Logger logger;
        public FileParser(DirectoryInfo dir)
        {
            logger = new Logger(dir);
        }
        public async void Start(Queue<FileInfo> fileQueue, Queue<List<string[]>> linesListQueue)
        {

            await Task.Run(() =>
            {
                while (true)
                {
                    if (fileQueue.Count > 0)
                    {
                        Parse(fileQueue.First(), linesListQueue);
                        Console.WriteLine("Element " + fileQueue.Dequeue().Name + "was dequeued"); ;

                    }
                    else
                        Thread.Sleep(1000);
                }
            });
        }
        public static void Parse(FileInfo fileName, Queue<List<string[]>> linesListQueue)
        {

            using (StreamReader reader = new StreamReader(fileName.FullName))
            {

                string? line;
                List<string[]> lines = new List<string[]>();

                while ((line = reader.ReadLine()) != null)
                {
                    string[] rowElements = Split(line);
                    if (Validator.IsValid(rowElements))
                    {
                        lines.Add(rowElements);
                    }
                    else Console.WriteLine(line + "wrong line");
                }
                linesListQueue.Enqueue(lines);

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
                    rowElements[elementsAmount] = line.Substring(secondComma + 1).Trim();
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

    }
}
