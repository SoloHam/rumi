namespace rumi
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    // Represents a single playing card
    public class Deck : MonoBehaviour
    {
        private GameObject CardPrefab => FindObjectOfType<GameManager>().CardPrefab;

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void InitialiseCards(List<Card> cards)
        {
            foreach (var card in cards)
            {
                var i = cards.IndexOf(card);

                var cardObj = Instantiate(this.CardPrefab, this.transform);
                var cardUIComponent = cardObj.GetComponent<CardUI>();

                cardUIComponent.InitialiseCard(card);
                var cardPos = cardObj.transform.position;

                if (i % 13 == 0)
                {
                    cardPos.x += (cards.Count - i) * 0.3f;
                }

                cardObj.transform.position = cardPos;
            }
        }
    }
}