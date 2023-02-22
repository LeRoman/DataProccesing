using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataProcessing
{
    public class ProcessQueue
    {
        public Queue<FileInfo> fileQueue;
        public ProcessQueue()
        {
            fileQueue = new Queue<FileInfo>();
        }

        public void AddToFileQueue(FileInfo fileName)
        {
            fileQueue.Enqueue(fileName);
        }

        public void PrintQueue()
        {
            foreach (var item in fileQueue)
            {
                Console.WriteLine(item.Name);
            }
        }
    }
}
