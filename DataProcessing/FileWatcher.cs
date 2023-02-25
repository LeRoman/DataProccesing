using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcessing
{
    class FileWatcher : BackgroundService
    {
        DateTime folderLastModified;
        DateTime lastProcessTime;
        readonly DirectoryInfo directory;
        readonly Queue<FileInfo> fileQueue;

        public FileWatcher(Queue<FileInfo> queue, DirectoryInfo directory)
        {
            this.directory = directory;
            this.fileQueue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                folderLastModified = Directory.GetLastWriteTime(directory.FullName);

                if (lastProcessTime < folderLastModified || lastProcessTime == default)
                {
                    ParseFolder(fileQueue, directory);
                    lastProcessTime = DateTime.Now;
                }
                await Task.Delay(1000, stoppingToken);
            }
        }

        void ParseFolder(Queue<FileInfo> queue, DirectoryInfo directory)
        {
            var dir = Path.Combine(directory.FullName, "Inwork\\").ToString();
            Directory.CreateDirectory(dir);

                var list = directory
                      .GetFiles()
                      .Where(x => x.Extension == ".txt" || x.Extension == ".csv")
                      .ToList();

                if (list.Count != 0)
                {
                    foreach (var item in list)
                    {
                        queue.Enqueue(item);
                        item.MoveTo(dir + DateTime.Now.ToShortTimeString().Replace(':', '_') + " " + item.Name); // to avoid file name conflicts
                        Console.WriteLine(item.FullName + " added");
                    }
                }
        }

    }
}
