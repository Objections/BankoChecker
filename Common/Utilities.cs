using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace BankoChecker
{
    public static class Utilities
    {
        public static void PrintNumbersBase10(HashSet<int> numbers)
        {
            PrintNumbersBase(10, numbers);
        }

        public static void PrintNumbersBase18(HashSet<int> numbers)
        {
            PrintNumbersBase(18, numbers);
        }

        public static void PrintNumbersBase9(HashSet<int> numbers)
        {
            PrintNumbersBase(9, numbers);
        }

        public static void PrintNumbersBase(int numberBase, HashSet<int> numbers)
        {
            AnsiConsole.Render(CreateTableNumbersBase(numberBase, numbers));
        }

        public static Panel CreateTableNumbersBase(int numberBase, HashSet<int> numbers)
        {
            int[] ceilings = Enumerable.Range(1, 90 / numberBase).Select(number => number * numberBase).ToArray();

            List<List<string>> numberLines = GetNumbersValuesPivot(numbers, ceilings);

            Table table = new Table().HideHeaders();
            table.AddColumns(ceilings.Select(ceiling => ceiling.ToString()).ToArray());
            foreach (var numberLine in numberLines)
            {
                table.AddRow(numberLine.ToArray());
            }

            return new Panel(table).Header("Numbers Drawn");
        }

        private static List<List<string>> GetNumbersValuesPivot(HashSet<int> numbers, int[] ceilings)
        {
            var celingBase = ceilings[0];
            List<List<string>> numberLines = new List<List<string>>();
            for (int i = 1; i <= celingBase; i++)
            {
                var values = new List<string>();
                foreach (var ceiling in ceilings)
                {
                    int number = ceiling - celingBase + i;
                    bool numberDrawn = numbers.Contains(number);

                    values.Add(string.Format(
                        GetNumberString(number, numberDrawn) + "{0}",
                        ceilings.Contains(number) ? "" : "\n"));
                }
                numberLines.Add(values);
            }
            return numberLines;
        }

        public static string GetNumberString(string number, bool colorNumber)
        {
            return string.Format(
                        "{0}{1,2}{2}",
                        colorNumber ? "[green]" : "",
                        number,
                        colorNumber ? "[/]" : "");
        }

        public static string GetNumberString(int number, bool colorNumber)
        {
            return GetNumberString(number.ToString(), colorNumber);
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        public static string RemoveAfterPhoneNumber(string input)
        {
            return Regex.Replace(input, "[+]45.*", string.Empty);
        }

        public static string ReplaceHtmlNewlines(string input)
        {
            return Regex.Replace(input, "<br><br>", "\n");
        }
    }
}
