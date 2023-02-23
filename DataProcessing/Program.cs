using DataProcessing.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataProcessing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var appSetting = config.GetSection("AppConfig").Get<AppConfig>();
            var inputPath = new DirectoryInfo(appSetting.inputDirectory);

            var fileQueue = new Queue<FileInfo>();
            var parsedLinesQueue= new Queue<List<string[]>>();
            var watcher = new FileWatcher();
            var parser = new FileParser();

            Task.Factory.StartNew(() => watcher.Start(fileQueue, inputPath));
            Task.Factory.StartNew(() => parser.Start(fileQueue, parsedLinesQueue));
            while (true)

            {
                Console.WriteLine("fdgdfg");
                Thread.Sleep(6000);
            }
        }
    }
}
