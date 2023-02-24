using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcessing
{
    class FileWatcher
    {
        DateTime folderLastModified;
        DateTime lastProcessTime;

        public async Task Start(Queue<FileInfo> queue, DirectoryInfo directory)
        {
            if (directory.Exists)
                await Task.Run(() =>
                {
                    while (true)
                    {
                        folderLastModified = Directory.GetLastWriteTime(directory.FullName);

                        if (lastProcessTime < folderLastModified || lastProcessTime == default)
                        {
                            ParseFolder(queue, directory);
                            lastProcessTime = DateTime.Now;
                        }
                        else
                            Thread.Sleep(1000);
                    }

                });
        }

        void ParseFolder(Queue<FileInfo> queue, DirectoryInfo directory)
        {
            var dir = Path.Combine(directory.FullName, "inwork\\").ToString();
            Directory.CreateDirectory(dir);

            while (true)
            {
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
}
