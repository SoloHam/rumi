namespace rumi
{
    using UnityEngine;
    using UnityEngine.UI;

    // Represents a single playing card
    public class CardUI: MonoBehaviour
    {
        public Card Card;
        public Image ImageComponent;

        private bool ShowFace;
        private Sprite BackSideSprite;

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void InitialiseCard(Card card)
        {
            this.BackSideSprite = FindObjectOfType<GameManager>().CardBackSide;
            this.ImageComponent = GetComponent<Image>();

            this.Card = card;
            this.ImageComponent.sprite = !this.ShowFace ? this.BackSideSprite : this.Card.Artwork;
        }
    }
}