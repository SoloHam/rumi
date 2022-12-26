namespace rumi
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    // Represents a single playing card
    public class CardUI: MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Card Card;
        public Image ImageComponent;

        public bool ShowFace;
        private Sprite BackSideSprite;

        private static CardUI HoveredCard;

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

            this.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 215);

            this.Card = card;
        }

        public void Move(Transform parent, Quaternion? rotation = null)
        {
            transform.SetParent(parent);

            var cardTransform = GetComponent<RectTransform>();

            cardTransform.localRotation = Quaternion.identity;
            cardTransform.anchorMin = new Vector2(0.5f, 0.5f);
            cardTransform.anchorMax = new Vector2(0.5f, 0.5f);
            cardTransform.localPosition = new Vector3(0, 0);

            if (rotation != null)
            {
                cardTransform.localRotation = rotation.Value;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (HoveredCard == null)
            {
                return;
            }

            var hoveredSiblingIndex = transform.parent.childCount;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                if (transform.parent.GetChild(i) == HoveredCard.transform)
                {
                    hoveredSiblingIndex = i;
                    break;
                }
            }
            transform.SetSiblingIndex(hoveredSiblingIndex);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HoveredCard = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HoveredCard = null;
        }
    }
}