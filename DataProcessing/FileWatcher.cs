using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;

namespace DataProcessing
{
    class FileWatcher
    {
        DateTime folderLastModified;
        DateTime lastProcessTime;

        public async Task  Start(Queue<FileInfo> queue, DirectoryInfo directory)
        {
            await Task.Run(() =>
            {
                if (directory.Exists)
                {
                    ParseFolder(queue, directory);
                }
            });
        }

        void ParseFolder (Queue<FileInfo> queue, DirectoryInfo directory)
        {
            var dir = Path.Combine(directory.FullName, "temp\\").ToString();
            Directory.CreateDirectory(dir);

            while (true)
            {
                folderLastModified = Directory.GetLastWriteTime(directory.FullName);

                if (lastProcessTime < folderLastModified|| lastProcessTime == default)
                { 
                var list = directory
                      .GetFiles()
                      .Where(x => x.Extension == ".txt" || x.Extension == ".csv")
                      .ToList();

                 lastProcessTime= DateTime.Now;
                  

                    if (list.Count != 0)
                    {
                        foreach (var item in list)
                        {
                            queue.Enqueue(item);
                            Console.WriteLine(DateTime.Now.ToShortTimeString()); 
                            item.MoveTo(dir + DateTime.Now.ToString().Replace(':', '_') + item.Name); // to avoid file name conflicts
                            Console.WriteLine(item.FullName);
                        }
                    }
                }
                
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(3000);
                foreach (var item in queue)
                {
                    Console.Write("Files in queue:"+item.Name+"  ");
                }

            }
        }

    }
}
