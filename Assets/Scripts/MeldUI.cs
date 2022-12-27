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

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void Initialise(Meld meld)
        {
            Meld = meld;
            MeldText.text = meld.ToString();
        }

        public void AddCard(CardUI cardUI)
        {
            Cards.Add(cardUI.Card);
            cardUI.Move(CardsHolder);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HoveredMeld = this;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HoveredMeld = null;

            if (CardUI.DraggedCard.Card != null && Cards.Contains(CardUI.DraggedCard.Card))
            {
                Cards.Remove(CardUI.DraggedCard.Card);
            }
        }
    }
}