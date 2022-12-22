namespace rumi
{
    using System.Collections.Generic;

    // Represents a group of cards that form a meld
    public class Meld
    {
        public List<Card> Cards { get; set; }
        public bool IsSet { get; set; }
        public Meld(List<Card> cards, bool isSet)
        {
            Cards = cards;
            IsSet = isSet;
        }
    }
}