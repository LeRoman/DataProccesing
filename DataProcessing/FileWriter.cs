using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcessing
{
    internal class FileWriter : BackgroundService
    {
        readonly Queue<List<Transaction>> transactionListQueue;
        readonly DirectoryInfo outputDirectory;

        public FileWriter(Queue<List<Transaction>> transactionListQueue, DirectoryInfo outputDirectory)
        {
            this.transactionListQueue = transactionListQueue;
            this.outputDirectory = outputDirectory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (outputDirectory.Exists)
                {
                    if (transactionListQueue.Count > 0)
                    {
                        while (transactionListQueue.Count > 0)
                        {
                            WriteFile();
                        }

                    }
                    else Thread.Sleep(3000);

                }
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void WriteFile()
        {
            string outputString = String.Empty;
            var transactions = transactionListQueue.First();
            var outputPath = Path.Combine(outputDirectory.ToString(), DateTime.Now.ToShortDateString().Replace('.', '-'));
            Directory.CreateDirectory(outputPath);
            var outputFileName = Path.Combine(outputPath, DateTime.Now.ToLongTimeString().Replace(':', '_') + "_output.json");


            var transactionCollection = transactions.GroupBy(x => x.City).Select(cities => new
            {
                city = cities.Key,
                services = cities.GroupBy(x => x.Service).Select(services => new
                {
                    name = services.Key,
                    payers = services.ToArray(),
                    total = services.Sum(x => x.Payment)
                }),
                total = cities.Sum(x => x.Payment)
            }); ;

            outputString = JsonConvert.SerializeObject(transactionCollection, Formatting.Indented);

            File.WriteAllText(outputFileName, outputString);
            transactionListQueue.Dequeue();
        }
    }
}

