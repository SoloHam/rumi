namespace rumi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    public class StockPile : MonoBehaviour
    {
        private GameObject CardPrefab => FindObjectOfType<GameManager>().CardPrefab;

        // The stock pile
        Queue<Card> stockPile;

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void Initialise(List<Card> cards)
        {
            // Iterate through the children of the game object
            foreach (Transform child in transform)
            {
                // Destroy the child game object
                Destroy(child.gameObject);
            }

            foreach (var card in cards)
            {
                var i = cards.IndexOf(card);

                var cardObj = Instantiate(this.CardPrefab, this.transform);
                var cardUIComponent = cardObj.GetComponent<CardUI>();

                cardUIComponent.InitialiseCard(card);
                var cardPos = cardObj.transform.position;

                card.CardUI = cardUIComponent;

                if (i % 13 == 0)
                {
                    cardPos.x += (cards.Count - i) * 0.3f;
                }

                cardObj.transform.position = cardPos;
                cardObj.name = card.Rank + "_of_" + card.Suit;
            }

            stockPile = new Queue<Card>(cards);
        }

        public Card DrawCard()
        {
            return stockPile.Dequeue();
        }
    }
}