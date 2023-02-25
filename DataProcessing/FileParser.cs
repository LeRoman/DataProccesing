using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace DataProcessing
{
    public class FileParser : BackgroundService
    {
        const char quotationStart = '“';
        const char quotationEnd = '”';
        readonly Queue<FileInfo> fileQueue;
        readonly Queue<List<string[]>> linesListQueue;
        readonly Logger log;

        public FileParser(Queue<FileInfo> fileQueue, Queue<List<string[]>> linesListQueue, Logger log)
        {
            this.fileQueue = fileQueue;
            this.linesListQueue = linesListQueue;
            this.log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (fileQueue.Count > 0)
                {
                    while (fileQueue.Count > 0)
                    {
                        Parse();
                       // Console.WriteLine("Element " + fileQueue.First().Name + "was dequeued"); ;
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }

        public void Parse()
        {
            var current = fileQueue.Dequeue();

            using (StreamReader reader = new StreamReader(current.FullName))
            {
                bool fileHaveErrors = false;
                string? line;
                List<string[]> lines = new List<string[]>();

                while ((line = reader.ReadLine()) != null)
                {
                    string[] rowElements = Split(line);
                    if (Validator.IsValid(rowElements))
                    {
                        lines.Add(rowElements);
                    }
                    else
                    {
                        log.AddFoundErrors();
                        fileHaveErrors = true;
                    }

                    log.AddParsedLines();
                }

                linesListQueue.Enqueue(lines);

                if (fileHaveErrors) { log.AddInvalidFile(current.FullName); }
                    
                
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
