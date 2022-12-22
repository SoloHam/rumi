namespace rumi
{
    using System.Collections.Generic;

    // Represents a player in the game
    public class Player
    {
        public List<Card> Hand { get; set; }
        public List<Meld> Melds { get; set; }
        public int BuysRemaining { get; set; }
        public int Points { get; set; }
        public int TotalPoints { get; set; }
        public Player(List<Card> hand, List<Meld> melds, int buysRemaining, int totalPoints)
        {
            Hand = hand;
            Melds = melds;
            BuysRemaining = buysRemaining;
            TotalPoints = totalPoints;
        }
    }
}