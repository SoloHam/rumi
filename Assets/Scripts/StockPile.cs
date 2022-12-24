namespace rumi
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.EventSystems;

    public class StockPile : MonoBehaviour
    {
        private GameObject CardPrefab => FindObjectOfType<GameManager>().CardPrefab;

        // The stock pile
        Queue<Card> stockPile;

        public bool AllowDrawing;

        // Define the event and its event handler delegate
        public delegate void PileClickedEventHandler(CardUI cardUI);
        public event PileClickedEventHandler PileClicked;

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

        public void OnPileClicked(BaseEventData eventData)
        {
            // Get the PointerEventData from the event data
            var pointerEventData = (PointerEventData)eventData;

            // Get the CardUI component of the clicked card
            var cardUI = pointerEventData.pointerPressRaycast.gameObject.GetComponent<CardUI>();
            if (cardUI == null)
            {
                return;
            }

            // Raise the CardClicked event
            PileClicked?.Invoke(cardUI);
        }
    }
}