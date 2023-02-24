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
            var outputPath = new DirectoryInfo(appSetting.outputDirectory);

            var fileQueue = new Queue<FileInfo>();
            var parsedLinesQueue= new Queue<List<string[]>>();
            var transactionListQueue = new Queue<List<Transaction>>();

            var fileWatcher = new FileWatcher();
            var trnsCreator = new TransactionListCreator();
            var fileParser = new FileParser();
            var fileWriter = new FileWriter();

            Task.Factory.StartNew(() => fileWatcher.Start(fileQueue, inputPath));
            Task.Factory.StartNew(() => fileParser.Start(fileQueue, parsedLinesQueue));
            Task.Factory.StartNew(() => trnsCreator.Start(parsedLinesQueue, transactionListQueue));
            Task.Factory.StartNew(() => fileWriter.Start( transactionListQueue, outputPath));

            Console.ReadKey();
            //    while (true)

            //    {
            //        Console.WriteLine("fdgdfg");
            //        Thread.Sleep(6000);
            //    }
        }
    }
    }
