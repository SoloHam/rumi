namespace rumi
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;

    // Represents a single playing card
    public class CardUI: MonoBehaviour
    {
        public Card card;
        public Image imageComponent;

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void InitialiseCard(Card card)
        {
            this.card = card;
            this.imageComponent.sprite = this.card.Artwork;
        }
    }
}