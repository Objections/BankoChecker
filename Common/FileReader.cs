using MsgReader.Outlook;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BankoChecker
{
    public static class FileReader
    {
        public static HashSet<int> GetNumbers(string numberFilesDirectory)
        {
            var fileTypeGroups = Directory.GetFiles(numberFilesDirectory).GroupBy(file => Path.GetExtension(file));
            HashSet<int> numbersFromFiles = new HashSet<int>();
            HashSet<int> numbersFromMails = new HashSet<int>(); ;

            foreach (var fileTypeGroup in fileTypeGroups)
            {
                if (fileTypeGroup.Key == ".msg")
                {
                    numbersFromMails = GetNumbersFromEmail(fileTypeGroup.ToArray());
                }
                else
                {
                    numbersFromFiles = GetNumbersFromFiles(fileTypeGroup.ToArray());
                }
            }

            numbersFromFiles.UnionWith(numbersFromMails);

            return numbersFromFiles;
        }

        private static HashSet<int> GetNumbersFromFiles(string[] fileNames)
        {
            HashSet<int> numbers = new HashSet<int>();
            foreach (var fileName in fileNames)
            {
                var numbersFromFile = new List<int>();
                foreach (var line in File.ReadAllLines(fileName))
                {
                    var numberListStart = Regex.Match(line, @"\d+, ").Value;
                    if (string.IsNullOrEmpty(numberListStart))
                    {
                        continue;
                    }

                    var splitLine = line.Split(',');

                    foreach (var number in splitLine)
                    {
                        var numberCleaned = Regex.Match(number, @"\d+").Value;

                        if (int.TryParse(numberCleaned, out int numberParsed))
                        {
                            numbersFromFile.Add(numberParsed);
                        }
                    }
                }

                Console.WriteLine($"Numbers from '{Path.GetFileName(fileName)}' where {string.Join(", ", numbersFromFile)}");
                numbers.UnionWith(numbersFromFile);
            }

            return numbers;
        }

        private static HashSet<int> GetNumbersFromEmail(string[] fileNames)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            HashSet<int> numbers = new HashSet<int>();
            List<string[]> tableRows = new List<string[]>();
            foreach (var fileName in fileNames)
            {
                using Storage.Message message = new Storage.Message(fileName);
                var htmlBody = message.BodyHtml;

                var numbersFromFile = new List<int>();
                var lines = Utilities.RemoveAfterPhoneNumber(Utilities.StripHTML(Utilities.ReplaceHtmlNewlines(htmlBody))).Split("\n");

                foreach (var line in lines)
                {
                    if (numbersFromFile.Count >= 4)
                    {
                        break;
                    }

                    var numberListStart = Regex.Match(line, @"\d+, ").Value;
                    if (string.IsNullOrEmpty(numberListStart))
                    {
                        continue;
                    }

                    if (line.Split(',').Length < 4)
                    {
                        continue;
                    }

                    var splitLine = line.Split(',', '.');

                    foreach (var number in splitLine)
                    {
                        var numberCleaned = Regex.Match(number, @"\d+").Value;

                        if (int.TryParse(numberCleaned, out int numberParsed) &&
                            numberParsed > 0 &&
                            numberParsed <= 90)
                        {
                            numbersFromFile.Add(numberParsed);
                        }
                    }
                }
                tableRows.Add(new[] { $"{message.FileName}", $"{message.SentOn.Value:d}", $"{string.Join(", ", numbersFromFile)}" });
                numbers.UnionWith(numbersFromFile);
            }

            var tableRowsOrdered = tableRows.OrderBy(column => column[1]);

            var table = new Table();
            table.AddColumns(new[] { "File Name", "Date", "Numbers" });
            foreach (var tableRow in tableRowsOrdered)
            {
                table.AddRow(tableRow);
            }

            AnsiConsole.Render(new Panel(table).Header("Number extraction result"));

            return numbers;
        }

        public static List<Card> GetCards(string cardFilesDirectory, HashSet<int> numbers)
        {
            var filePaths = Directory.GetFiles(cardFilesDirectory);
            var cards = new List<Card>();

            foreach (var filePath in filePaths)
            {
                cards.Add(new Card(filePath, numbers));
            }

            return cards;
        }
    }
}
