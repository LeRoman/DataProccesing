using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcessing
{
    internal class FileWriter
    {
        public async Task Start(Queue<List<Transaction>> transactionListQueue, DirectoryInfo outputDirectory)
        {
            await Task.Run(() =>
            {
                if (outputDirectory.Exists)
                {
                    while (true)
                    {
                        if (transactionListQueue.Count > 0)
                        {
                            var list = transactionListQueue.Dequeue();
                            WriteFile(list, outputDirectory);
                        }
                        else Thread.Sleep(3000);
                    }
                }
            });
        }

        private void WriteFile(List<Transaction> transactions, DirectoryInfo outputPath)
        {
            string outputString = String.Empty;

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

            var outputFileName = Path.Combine(outputPath.ToString(), DateTime.Now.ToLongTimeString().Replace(':','_')+"_output.json");
            File.WriteAllText(outputFileName, outputString);
        }
    }
}

