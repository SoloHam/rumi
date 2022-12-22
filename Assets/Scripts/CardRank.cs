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
        Ace,
        [CardValue(20)]
        Two,
        [CardValue(5)]
        Three,
        [CardValue(5)]
        Four,
        [CardValue(5)]
        Five,
        [CardValue(5)]
        Six,
        [CardValue(5)]
        Seven,
        [CardValue(5)]
        Eight,
        [CardValue(5)]
        Nine,
        [CardValue(10)]
        Ten,
        [CardValue(10)]
        Jack,
        [CardValue(10)]
        Queen,
        [CardValue(10)]
        King
    }
}