using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankoChecker
{
    class CardRack
    {
        public CardRack(List<Card> cards)
        {
            Cards = cards;

            var table = new Table().NoBorder().AddColumn("");

            foreach (var card in Cards)
            {
                table.AddRow(card.Table);
                table.AddEmptyRow();
            }

            CardTable = table.Collapse();
        }

        List<Card> Cards { get; set; }
        public Table CardTable { get; set; }

        public void CheckBankoOnCards()
        {
            foreach (var card in Cards)
            {
                card.CheckBanko();
            }
        }
    }
}
