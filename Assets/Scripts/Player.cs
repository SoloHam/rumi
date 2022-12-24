namespace rumi
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.EventSystems;

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
        public List<Meld> Melds;

        [SerializeField]
        Transform HandCardsTransform;

        [SerializeField]
        RectTransform CommandPanelTransform;

        [SerializeField]
        CardUI SelectedCard;

        public int BuysRemaining;
        public int Points;
        public int TotalPoints;

        public bool IsMyTurn;

        public TurnState CurTurnState;

        GameManager GameManager => FindObjectOfType<GameManager>();

        // Define the event and its event handler delegate
        public delegate void HandCardClickedEventHandler(CardUI cardUI);
        public event HandCardClickedEventHandler HandCardClicked;

        public Player(List<Card> hand, List<Meld> melds, int buysRemaining, int totalPoints)
        {
            Hand = hand;
            Melds = melds;
            BuysRemaining = buysRemaining;
            TotalPoints = totalPoints;
        }

        public void AddCard(Card card)
        {
            this.Hand.Add(card);

            card.CardUI.Move(this.HandCardsTransform);
            card.CardUI.ShowFace = this.IsMyTurn;
        }

        private void Awake()
        {
            // Iterate through the children of the game object
            foreach (Transform child in HandCardsTransform)
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

        public void OnDrawCardFromStockPile(CardUI _)
        {
            if (!this.IsMyTurn || CurTurnState != TurnState.Drawing)
                return;

            var card = GameManager.StockPileComponent.DrawCard();
            card.CardUI.Move(this.HandCardsTransform);
            card.CardUI.ShowFace = this.IsMyTurn;

            CurTurnState = TurnState.Melding;
        }

        public void OnDrawCardFromDiscardPile(CardUI _)
        {
            if (!this.IsMyTurn || CurTurnState != TurnState.Drawing)
                return;

            var card = GameManager.DiscardPileComponent.Pop();
            card.CardUI.Move(this.HandCardsTransform);
            card.CardUI.ShowFace = this.IsMyTurn;

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

            Debug.Log("Card Clicked");

            // Raise the CardClicked event
            HandCardClicked?.Invoke(cardUI);
        }

        private void ShowHandCardUI(CardUI card)
        {
            if (!IsMyTurn || CurTurnState != TurnState.Melding)
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

        private void Update()
        {

        }
    }
}