namespace rumi
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.EventSystems;

    public class DiscardPile : MonoBehaviour
    {
        private GameObject CardPrefab => FindObjectOfType<GameManager>().CardPrefab;

        // The discard pile
        Stack<Card> discardPile = new Stack<Card>();

        // Define the event and its event handler delegate
        public delegate void PileClickedEventHandler(CardUI cardUI);
        public event PileClickedEventHandler PileClicked;

        // Define the event and its event handler delegate
        public delegate void CardDiscardedEventHandler();
        public event CardDiscardedEventHandler CardDiscarded;

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

        public void Add(Card card, bool raiseEvent = true)
        {
            discardPile.Push(card);

            card.CardUI.Move(this.transform);
            card.CardUI.ShowFace = true;

            if (raiseEvent)
            {
                CardDiscarded?.Invoke();
            }
        }

        public Card Peek()
        {
            return discardPile.Peek();
        }

        public Card Pop()
        {
            return discardPile.Pop();
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