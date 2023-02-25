using DataProcessing.Config;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var appSetting = config.GetSection("AppConfig").Get<AppConfig>();

            var inputPath = new DirectoryInfo(appSetting.inputDirectory);
            var outputPath = new DirectoryInfo(appSetting.outputDirectory);

            var fileQueue = new Queue<FileInfo>();
            var parsedLinesQueue= new Queue<List<string[]>>();
            var transactionListQueue = new Queue<List<Transaction>>();
            var log = new Logger(outputPath);

            IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<FileWatcher>(s => new FileWatcher(fileQueue, inputPath));
                    services.AddHostedService<FileParser>(s => new FileParser(fileQueue,parsedLinesQueue,log));
                    services.AddHostedService<TransactionListCreator>(s => new TransactionListCreator(parsedLinesQueue, transactionListQueue));
                    services.AddHostedService<FileWriter>(s => new FileWriter(transactionListQueue,outputPath));

                });

            CreateHostBuilder(args).Build().Run();
        }
    }
}
