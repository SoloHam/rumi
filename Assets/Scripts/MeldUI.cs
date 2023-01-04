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

        public bool? IsValid;
        public Player Player => GetComponentInParent<Player>();

        public bool IsMelded => Player.HasMelded;

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

        [SerializeField]
        Color MeldedColor;

        void Start()
        {

        }

        void Update()
        {
            var width = (Cards.Count == 0 ? 150 : (Cards.Count * 150) - ((Cards.Count - 1) * 130)) + 20;
            GetComponent<RectTransform>().sizeDelta = new Vector2(width, GetComponent<RectTransform>().sizeDelta.y);

            IsValid = Validate();

            if (IsMelded)
            {
                MeldBackground.color = MeldedColor;
            }
            else if (IsValid.HasValue)
            {
                MeldBackground.color = IsValid.Value ? ValidColor : InvalidColor;
            }
        }

        public void Initialise(Meld meld)
        {
            Meld = meld;
            MeldText.text = meld.ToString();
            Meld.MeldUI = this;
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

        private bool? Validate()
        {
            if (!Cards.Any())
            {
                MeldBackground.color = IdleColor;
                return null;
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

                    if (card.IsWild)
                    {
                        maxSequence++;
                        if (maxSequence > 1)
                        {
                            lastRank++;
                        }
                        else if (Cards.Count > 1)
                        {
                            lastRank = Cards[1].Rank - 1;
                            lastSuit = Cards[1].Suit;
                        }
                        return;
                    }

                    if (!(lastSuit == card.Suit && lastRank + 1 == card.Rank))
                    {
                        maxSequence = 0;
                    }

                    maxSequence++;
                    lastRank = card.Rank;
                    lastSuit = card.Suit;
                });
                isValid = maxSequence >= Meld.MinimumCount;
            }

            return isValid;
        }
    }
}