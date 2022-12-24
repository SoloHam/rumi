namespace rumi
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    public class DiscardPile : MonoBehaviour
    {
        private GameObject CardPrefab => FindObjectOfType<GameManager>().CardPrefab;

        // The discard pile
        Stack<Card> discardPile = new Stack<Card>();

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void Add(Card card)
        {
            // Iterate through the children of the game object
            foreach (Transform child in transform)
            {
                // Destroy the child game object
                Destroy(child.gameObject);
            }

            discardPile.Push(card);

            card.CardUI.Move(this.transform);
            card.CardUI.ShowFace = true;
        }

        public Card Peek()
        {
            return discardPile.Peek();
        }

        public Card Pop()
        {
            return discardPile.Pop();
        }
    }
}