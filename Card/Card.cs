using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console;

namespace BankoChecker
{
    public class Card
    {
        public Card(string fileName, HashSet<int> numbers)
        {
            FileName = fileName;
            foreach (var line in File.ReadAllLines(FileName))
            {
                CardLines.Add(new CardLine(line, numbers));
            }

            FullCard = CardLines.All(cardLine => cardLine.Bingo);

            CreateTable();
        }

        public Panel Table { get; set; }
        private string FileName { get; set; }
        private List<CardLine> CardLines { get; set; } = new List<CardLine>();
        private bool FullCard { get; set; }
        private List<int> _numbers;
        private List<int> Numbers
        {
            get
            {
                if (_numbers != null)
                {
                    return _numbers;
                }

                _numbers = new List<int>();
                CardLines.ForEach(cardline => _numbers.AddRange(cardline.Numbers.Keys));
                _numbers.Sort();
                return _numbers;
            }
            set
            {
                _numbers = value;
            }
        }

        public void CheckBanko()
        {
            if (FullCard)
            {
                Console.WriteLine();
                Console.WriteLine($"Full card - on {Path.GetFileNameWithoutExtension(FileName)} - Banko ");
                Console.WriteLine($"Numbers where: {string.Join(", ", Numbers)}");
            }
            else
            {
                for (var i = 0; i < CardLines.Count; i++)
                {
                    var cardLine = CardLines[i];
                    if (cardLine.Bingo)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Bingo - on {Path.GetFileNameWithoutExtension(FileName)} line {i + 1}");
                        Console.WriteLine($"Numbers where: {string.Join(", ", cardLine.Numbers)}");
                    }
                }
            }
        }

        private void CreateTable()
        {
            var lineValuesTop = CardLines[0].GetLineValues().ToArray();
            var lineValuesButtom = CardLines[2].GetLineValues().ToArray();

            var table = new Table();
            for (int i = 0; i < lineValuesTop.Length; i++)
            {
                table.AddColumns(new TableColumn(lineValuesTop[i]).Footer(lineValuesButtom[i]));
            }

            table.AddRow(CardLines[1].GetLineValues().ToArray());
            Table = new Panel(table).Header(Path.GetFileNameWithoutExtension(FileName));
        }

        public void PrintCard()
        {
            AnsiConsole.Render(Table);
        }
    }
}
