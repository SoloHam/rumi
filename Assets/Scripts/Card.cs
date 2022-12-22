namespace rumi
{
    // Represents a single playing card
    public class Card
    {
        public CardRank Rank { get; set; }
        public CardSuit Suit { get; set; }

        // Indicates whether the card is a wildcard
        public bool IsWild => Rank == CardRank.Two;

        public Card(CardRank rank, CardSuit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        // Returns the point value of the specified card
        public int GetValue()
        {
            // Get the CardValueAttribute for the rank
            CardValueAttribute attribute = System.Attribute.GetCustomAttribute(
                typeof(CardRank).GetField(Rank.ToString()), typeof(CardValueAttribute)) as CardValueAttribute;

            // Return the value from the attribute
            return attribute.Value;
        }
    }
}