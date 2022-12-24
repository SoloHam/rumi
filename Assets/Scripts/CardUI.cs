namespace rumi
{
    using UnityEngine;
    using UnityEngine.UI;

    // Represents a single playing card
    public class CardUI: MonoBehaviour
    {
        public Card Card;
        public Image ImageComponent;

        public bool ShowFace;
        private Sprite BackSideSprite;

        private void Start()
        {

        }

        private void Update()
        {
            this.ImageComponent.sprite = !this.ShowFace ? this.BackSideSprite : this.Card.Artwork;
        }

        public void InitialiseCard(Card card)
        {
            this.BackSideSprite = FindObjectOfType<GameManager>().CardBackSide;
            this.ImageComponent = GetComponent<Image>();

            this.Card = card;
        }

        public void Move(Transform parent, Quaternion? rotation = null)
        {
            transform.SetParent(parent);

            var cardTransform = GetComponent<RectTransform>();
            var cardPos = cardTransform.localPosition;
            cardPos.x = 0;
            cardTransform.localPosition = cardPos;

            if (rotation != null)
            {
                cardTransform.localRotation = rotation.Value;
            }
        }
    }
}