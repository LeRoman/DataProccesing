using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcessing
{
    internal class TransactionListCreator
    {
        public async Task Start(Queue<List<string[]>> parsedLinesQueue, Queue<List<Transaction>> transactionlistQueue )
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    if (parsedLinesQueue.Count > 0)
                        Create(parsedLinesQueue.Dequeue(), transactionlistQueue);
                    else Thread.Sleep(3000);
                }
            });
        }

        void Create(List<string[]> parsedLines, Queue<List<Transaction>> transactionlistQueue)
        {
            var list= new List<Transaction>();
            Parallel.ForEach(parsedLines, line =>
            {
                var transaction = new Transaction(line);
                list.Add(transaction);
            });
            transactionlistQueue.Enqueue(list);
        }

    }
}
