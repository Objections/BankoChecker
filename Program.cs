using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace BankoChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            var numberFilesPath = args.Length > 0 ? args[0] : null ?? @"C:\Banko\Numbers";
            var cardsFilesPath = args.Length > 1 ? args[1] : null ?? @"C:\Banko\Cards";

            numberFilesPath = CheckNumbersFilePath(numberFilesPath);
            if(numberFilesPath == null)
            {
                return;
            }

            cardsFilesPath = CheckCardsFilePath(cardsFilesPath);
            if (cardsFilesPath == null)
            {
                return;
            }

            HashSet<int> numbers = FileReader.GetNumbers(numberFilesPath);
            List<Card> cards = FileReader.GetCards(cardsFilesPath, numbers);

            var cardHolder = new CardRack(cards);

            cardHolder.CheckBankoOnCards();

            var numberTable = new Table().AddColumn("");
            numberTable.AddRow(Utilities.CreateTableNumbersBase(10, numbers));

            List<IRenderable> tablesToRender = new List<IRenderable>
            {
                cardHolder.CardTable,
                numberTable.Border(TableBorder.None)
            };

            AnsiConsole.Render(new Columns(tablesToRender).Collapse());

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static string CheckNumbersFilePath(string numberFilesPath)
        {
            var programPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(numberFilesPath))
            {
                if (!Directory.Exists(programPath + @"\Numbers"))
                {
                    Console.WriteLine($@"Number files need to go to {numberFilesPath} or here \Numbers");
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    return null;
                }
                else
                {
                    numberFilesPath = programPath + @"\Numbers";
                }
            }

            return numberFilesPath;
        }

        private static string CheckCardsFilePath(string cardsFilesPath)
        {
            var programPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(cardsFilesPath))
            {
                if (!Directory.Exists(programPath + @"\Cards"))
                {
                    Console.WriteLine($@"Card files need to go to {cardsFilesPath} or here \Cards");
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    return null;
                }
                else
                {
                    cardsFilesPath = programPath + @"\Cards";
                }
            }

            return cardsFilesPath;
        }
    }
}
