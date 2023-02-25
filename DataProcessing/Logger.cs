using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataProcessing
{

    public class Logger
    {
        static DateTime Date { get; set; }
        static int ParsedFiles { get; set; }
        static int ParsedLines { get; set; }
        static int FoundErrors { get; set; }
        static List<string> InvalidFiles { get; set; }
        string outputPath { get; set; }
        string outputFolderName { get; set; }

        public Logger(DirectoryInfo output)
        {
            outputFolderName = output.FullName;
            Date = DateTime.Now;
            outputFolderName = Date.ToString("d").Replace('.', '-');
        }
        void LogToFile()
        {

            var result = new StringBuilder();
            result.Append($"parsed_files:{ ParsedFiles}\n")
                .Append($"parsed_liles:{ ParsedLines}\n")
                .Append($"found_errors:{ FoundErrors}\n")
                .Append($"invalid_files:[{ string.Join('\n', InvalidFiles)}]\n");
            StatReset();

            File.WriteAllText(Path.Combine(outputPath, outputFolderName, "meta.log"), result.ToString());
        }

        private void StatReset()
        {
            Date = DateTime.Now;
            ParsedFiles = default;
            ParsedLines = default;
            FoundErrors = default;
            outputFolderName = Date.ToString("d").Replace('.', '-');
            InvalidFiles.Clear();
        }

        public void AddParsedFiles()
        {
            CheckDate();
            ParsedFiles++;

        }

        public void AddParsedLines()
        {
            CheckDate();
            ParsedLines++;
        }

        public void AddFoundErrors()
        {
            CheckDate();
            FoundErrors++;
        }

        public void AddInvalidFile(string fileName)
        {
            CheckDate();
            InvalidFiles.Add(fileName);
        }
        void CheckDate()
        {
            var date1 = Date.ToShortDateString();
            var date2 = DateTime.Now.ToShortTimeString();

            if (Date.ToShortDateString() != DateTime.Now.ToShortDateString())
            {
                LogToFile();
                StatReset();
            }
        }

    }
}
