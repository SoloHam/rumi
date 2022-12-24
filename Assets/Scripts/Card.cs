namespace rumi
{
    using System;

    using UnityEngine;

    // Represents a single playing card
    [Serializable]
    public class Card
    {
        public CardRank Rank;

        public CardSuit Suit;

        public Sprite Artwork;

        public CardUI CardUI;

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