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

            Console.WriteLine();
            Utilities.PrintNumbersBase10(numbers);

            foreach (var card in cards)
            {
                Console.WriteLine();
                card.CheckBanko();
                card.PrintCard();
            }

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
