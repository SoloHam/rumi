namespace rumi
{
    using System;
    using System.Collections.Generic;

    using TMPro;

    using UnityEngine;
    using UnityEngine.EventSystems;

    // Represents a single playing card
    [Serializable]
    public class MeldUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static MeldUI HoveredMeld;

        public Meld Meld;

        [SerializeField]
        public List<Card> Cards;

        [SerializeField]
        RectTransform CardsHolder;

        [SerializeField]
        TextMeshProUGUI MeldText;

        void Start()
        {

        }

        void Update()
        {
            var width = (Cards.Count == 0 ? 150 : (Cards.Count * 150) - ((Cards.Count - 1) * 130)) + 20;
            GetComponent<RectTransform>().sizeDelta = new Vector2(width, GetComponent<RectTransform>().sizeDelta.y);
        }

        public void Initialise(Meld meld)
        {
            Meld = meld;
            MeldText.text = meld.ToString();
        }

        public void AddCard(CardUI cardUI)
        {
            if (!Cards.Contains(cardUI.Card))
            {
                Cards.Add(cardUI.Card);
            }
            cardUI.Move(CardsHolder);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HoveredMeld = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HoveredMeld = null;

            if (CardUI.DraggedCard?.Card != null && Cards.Contains(CardUI.DraggedCard.Card))
            {
                Cards.Remove(CardUI.DraggedCard.Card);
                GameManager.Instance.CurrentPlayer.AddCard(CardUI.DraggedCard.Card);
            }
        }
    }
}