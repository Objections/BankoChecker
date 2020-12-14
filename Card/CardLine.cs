using System;
using System.Collections.Generic;
using System.Linq;

namespace BankoChecker
{
    public class CardLine
    {
        public CardLine(string line, HashSet<int> numbersToCheck)
        {
            var lineNumbers = line.Split(',');
            foreach (var number in lineNumbers)
            {
                Numbers.Add(int.Parse(number), numbersToCheck.Contains(int.Parse(number)));
            }

            Bingo = Numbers.All(number => number.Value);
        }

        public Dictionary<int, bool> Numbers { get; private set; } = new Dictionary<int, bool>();
        public bool Bingo { get; private set; }

        public List<string> GetLineValues()
        {
            var ceilings = new[] { 10, 20, 30, 40, 50, 60, 70, 80, 91 };

            var numberRanges = Numbers.GroupBy(number => ceilings.First(ceiling => ceiling > number.Key));
            List<string> values = new List<string>();
            foreach (var ceiling in ceilings)
            {
                var range = numberRanges.FirstOrDefault(range => range.Key == ceiling);
                string number = range?.First().Key.ToString() ?? "";
                bool color = range?.First().Value ?? false;
                values.Add(Utilities.GetNumberString(number, color));
            }
            return values;
        }
    }
}
