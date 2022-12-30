namespace rumi
{
    // The attribute to get the point value of a card
    public class CardValueAttribute : System.Attribute
    {
        public int Value { get; }
        public CardValueAttribute(int value)
        {
            Value = value;
        }
    }

    // Enum for the different card ranks
    public enum CardRank
    {
        [CardValue(20)]
        Ace = 13,
        [CardValue(20)]
        Two = 1,
        [CardValue(5)]
        Three = 2,
        [CardValue(5)]
        Four = 3,
        [CardValue(5)]
        Five = 4,
        [CardValue(5)]
        Six = 5,
        [CardValue(5)]
        Seven = 6,
        [CardValue(5)]
        Eight = 7,
        [CardValue(5)]
        Nine = 8,
        [CardValue(10)]
        Ten = 9,
        [CardValue(10)]
        Jack = 10,
        [CardValue(10)]
        Queen = 11,
        [CardValue(10)]
        King = 12
    }
}