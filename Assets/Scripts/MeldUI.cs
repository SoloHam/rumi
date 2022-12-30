namespace rumi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TMPro;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    // Represents a single playing card
    [Serializable]
    public class MeldUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static MeldUI HoveredMeld;

        public Meld Meld;

        [SerializeField]
        public List<Card> Cards => transform.GetComponentsInChildren<CardUI>().Select(x => x.Card).ToList();

        [SerializeField]
        RectTransform CardsHolder;

        [SerializeField]
        TextMeshProUGUI MeldText;

        [SerializeField]
        Image MeldBackground;

        [SerializeField]
        Color IdleColor;

        [SerializeField]
        Color InvalidColor;

        [SerializeField]
        Color ValidColor;

        void Start()
        {

        }

        void Update()
        {
            var width = (Cards.Count == 0 ? 150 : (Cards.Count * 150) - ((Cards.Count - 1) * 130)) + 20;
            GetComponent<RectTransform>().sizeDelta = new Vector2(width, GetComponent<RectTransform>().sizeDelta.y);

            ValidateMeld();
        }

        public void Initialise(Meld meld)
        {
            Meld = meld;
            MeldText.text = meld.ToString();
        }

        public void AddCard(CardUI cardUI)
        {
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
                GameManager.Instance.CurrentPlayer.AddCard(CardUI.DraggedCard.Card);
            }
        }

        private void ValidateMeld()
        {
            if (!Cards.Any())
            {
                MeldBackground.color = IdleColor;
                return;
            }

            var isValid = false;
            var wildCount = Cards.Count(x => x.IsWild);

            if (Meld.Type == MeldType.Set)
            {
                isValid = Cards.GroupBy(x => x.Rank).Any(x => x.Count() + wildCount >= Meld.MinimumCount);
            }
            else if (Meld.Type == MeldType.Run)
            {
                var maxSequence = 0;
                CardRank? lastRank = null;
                CardSuit? lastSuit = null;

                Cards.ForEach(card =>
                {
                    if (maxSequence >= Meld.MinimumCount)
                    {
                        return;
                    }

                    if (lastRank == null)
                    {
                        maxSequence++;

                        lastRank = card.Rank;
                        lastSuit = card.Suit;
                        return;
                    }

                    if (card.IsWild)
                    {
                        maxSequence++;
                        lastRank = card.Rank;
                        return;
                    }

                    if (!(lastSuit == card.Suit && (lastRank + 1 == card.Rank || lastRank == CardRank.Two)))
                    {
                        maxSequence = 0;
                    }

                    maxSequence++;
                    lastRank = card.Rank;
                    lastSuit = card.Suit;
                });
                isValid = maxSequence >= Meld.MinimumCount;
            }

            MeldBackground.color = isValid ? ValidColor : InvalidColor;
        }
    }
}