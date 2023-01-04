namespace rumi
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public enum TurnState
    {
        Drawing,
        Melding,
        Discarded
    }

    // Represents a player in the game
    public class Player : MonoBehaviour
    {
        [SerializeField]
        public List<Card> Hand;

        [SerializeField]
        public List<Meld> Melds => HandCardMeldsTransform.GetComponentsInChildren<MeldUI>().Select(x => x.Meld).ToList();

        [SerializeField]
        Transform HandCardsTransform;

        [SerializeField]
        Transform HandCardMeldsTransform;

        [SerializeField]
        RectTransform CommandPanelTransform;

        [SerializeField]
        CardUI SelectedCard;

        [SerializeField]
        GameObject MeldUIPrefab;

        [SerializeField]
        Button MeldOffBtn;

        public int BuysRemaining;
        public int Points;
        public int TotalPoints;

        public bool IsMyTurn;
        public bool HasMelded;

        public TurnState CurTurnState;

        GameManager GameManager => FindObjectOfType<GameManager>();

        // Define the event and its event handler delegate
        public delegate void HandCardClickedEventHandler(CardUI cardUI);
        public event HandCardClickedEventHandler HandCardClicked;

        public void AddCard(Card card)
        {
            if (!Hand.Contains(card))
            {
                Hand.Add(card);
            }

            card.CardUI.Move(HandCardsTransform);
            card.CardUI.ShowFace = IsMyTurn;
        }

        private void Awake()
        {
            // Iterate through the children of the HandCardsTransform game object
            foreach (Transform child in HandCardsTransform)
            {
                if (child == HandCardMeldsTransform)
                {
                    continue;
                }

                // Destroy the child game object
                Destroy(child.gameObject);
            }

            // Iterate through the children of the HandCardMeldsTransform game object
            foreach (Transform child in HandCardMeldsTransform)
            {
                // Destroy the child game object
                Destroy(child.gameObject);
            }
        }

        private void Start()
        {
            GameManager.StockPileComponent.PileClicked += OnDrawCardFromStockPile;
            GameManager.DiscardPileComponent.PileClicked += OnDrawCardFromDiscardPile;

            HandCardClicked += ShowHandCardUI;
        }

        // Allows the current player to make their move by drawing or discarding a card and attempting to meld off their cards
        public void MakeMove()
        {
            this.IsMyTurn = true;
            this.Hand.ForEach(c => c.CardUI.ShowFace = true);
            this.CurTurnState = TurnState.Drawing;
        }

        public void EndMove()
        {
            this.IsMyTurn = false;
            this.Hand.ForEach(c => c.CardUI.ShowFace = false);
        }

        public void OnDrawCardFromStockPile(CardUI _)
        {
            if (!this.IsMyTurn || CurTurnState != TurnState.Drawing)
                return;

            var card = GameManager.StockPileComponent.DrawCard();
            card.CardUI.Move(this.HandCardsTransform);
            card.CardUI.ShowFace = this.IsMyTurn;
            Hand.Add(card);

            CurTurnState = TurnState.Melding;
        }

        public void OnDrawCardFromDiscardPile(CardUI _)
        {
            if (!this.IsMyTurn || CurTurnState != TurnState.Drawing)
                return;

            var card = GameManager.DiscardPileComponent.Pop();
            card.CardUI.Move(this.HandCardsTransform);
            card.CardUI.ShowFace = this.IsMyTurn;
            Hand.Add(card);

            CurTurnState = TurnState.Melding;
        }

        public void OnHandCardClicked(BaseEventData eventData)
        {
            // Get the PointerEventData from the event data
            var pointerEventData = (PointerEventData)eventData;

            // Get the CardUI component of the clicked card
            var cardUI = pointerEventData.pointerPressRaycast.gameObject.GetComponent<CardUI>();
            if (cardUI == null)
            {
                return;
            }

            // Raise the CardClicked event
            HandCardClicked?.Invoke(cardUI);
        }

        public void InitialiseMelds()
        {
            var melds = GameManager.Instance.CurrentRound.Melds.Select(x => new Meld(x.Type, x.MinimumCount)).ToList();
            foreach (var meld in melds)
            {
                var meldUI = Instantiate(MeldUIPrefab, HandCardMeldsTransform);
                var meldUIComponent = meldUI.GetComponent<MeldUI>();

                meldUIComponent.Initialise(meld);
            }
        }

        private void ShowHandCardUI(CardUI card)
        {
            if (!IsMyTurn || CurTurnState != TurnState.Melding || Melds.Any(x => x.MeldUI.Cards.Contains(card.Card)))
                return;

            this.SelectedCard = card;
            CommandPanelTransform.SetParent(card.transform);
            CommandPanelTransform.SetLeft(0);
            CommandPanelTransform.SetRight(0);
            var pos = CommandPanelTransform.localPosition;
            pos.y = 160;
            CommandPanelTransform.localPosition = pos;
            CommandPanelTransform.localRotation = Quaternion.identity;
            CommandPanelTransform.gameObject.SetActive(true);
        }

        public void DicardCard()
        {
            Hand.Remove(SelectedCard.Card);
            GameManager.DiscardPileComponent.Add(SelectedCard.Card);
            CommandPanelTransform.gameObject.SetActive(false);

            CurTurnState = TurnState.Discarded;
        }

        public void MeldOff()
        {
            HasMelded = true;
        }

        public void ResetSelectedCardUI()
        {
            CommandPanelTransform.gameObject.SetActive(false);
            this.SelectedCard = null;
        }

        private void Update()
        {
            var showMeldOffBtn = !HasMelded && Melds.All(x => x.MeldUI.IsValid ?? false);
            MeldOffBtn.gameObject.SetActive(showMeldOffBtn);
        }
    }
}