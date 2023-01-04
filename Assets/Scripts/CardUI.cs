namespace rumi
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    // Represents a single playing card
    public class CardUI: MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
    {
        public Card Card;
        public Image ImageComponent;

        public bool ShowFace;
        private Sprite BackSideSprite;

        private static CardUI HoveredCard;
        public static CardUI DraggedCard;

        public Player Player => GetComponentInParent<Player>();
        public MeldUI Meld => transform.parent.GetComponent<MeldUI>();

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
            cardTransform.localPosition = Vector3.zero;
            cardTransform.localScale = Vector3.one;

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
            DraggedCard = this;
            if (HoveredCard != null)
            {
                var hoveredSiblingIndex = HoveredCard.transform.parent.childCount;
                for (int i = 0; i < HoveredCard.transform.parent.childCount; i++)
                {
                    if (HoveredCard.transform.parent.GetChild(i) == HoveredCard.transform)
                    {
                        hoveredSiblingIndex = i;
                        break;
                    }
                }

                if (HoveredCard.transform.parent.parent.GetComponent<MeldUI>() != null)
                {
                    MeldUI.HoveredMeld.AddCard(this);
                    Player.ResetSelectedCardUI();
                }

                transform.SetParent(HoveredCard.transform.parent);
                transform.SetSiblingIndex(hoveredSiblingIndex);
                transform.localScale = Vector3.one;
                return;
            }

            if (MeldUI.HoveredMeld != null)
            {
                MeldUI.HoveredMeld.AddCard(this);
                return;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HoveredCard = this;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            DraggedCard = null;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HoveredCard = null;
        }
    }
}