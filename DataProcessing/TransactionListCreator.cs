using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcessing
{
    internal class TransactionListCreator : BackgroundService
    {
        readonly Queue<List<string[]>> parsedLinesQueue;
        readonly Queue<List<Transaction>> transactionlistQueue;

        public TransactionListCreator(Queue<List<string[]>> parsedLinesQueue, Queue<List<Transaction>> transactionlistQueue)
        {
            this.parsedLinesQueue = parsedLinesQueue;
            this.transactionlistQueue = transactionlistQueue;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                if (parsedLinesQueue.Count > 0)
                    while (parsedLinesQueue.Count > 0)
                    {
                        Create();
                    }

                await Task.Delay(1000, stoppingToken);
            }
        }

        void Create()
        {
            var parsedLines = parsedLinesQueue.Dequeue();

            var list = new List<Transaction>();
            Parallel.ForEach(parsedLines, line =>
            {
                var transaction = new Transaction(line);
                list.Add(transaction);
            });

            transactionlistQueue.Enqueue(list);
        }

    }
}
