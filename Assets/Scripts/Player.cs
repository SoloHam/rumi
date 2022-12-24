namespace rumi
{
    using System.Collections.Generic;

    using UnityEngine;

    // Represents a player in the game
    public class Player : MonoBehaviour
    {
        [SerializeField]
        public List<Card> Hand;

        [SerializeField]
        public List<Meld> Melds;

        [SerializeField]
        public int BuysRemaining;

        [SerializeField]
        public int Points;

        [SerializeField]
        public int TotalPoints;

        [SerializeField]
        Quaternion CardRotation;

        public Player(List<Card> hand, List<Meld> melds, int buysRemaining, int totalPoints)
        {
            Hand = hand;
            Melds = melds;
            BuysRemaining = buysRemaining;
            TotalPoints = totalPoints;
        }

        public void AddCard(Card card)
        {
            this.Hand.Add(card);

            card.CardUI.Move(this.transform, this.CardRotation);
            card.CardUI.ShowFace = true;
        }

        private void Awake()
        {
            // Iterate through the children of the game object
            foreach (Transform child in transform)
            {
                // Destroy the child game object
                Destroy(child.gameObject);
            }
        }

        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}