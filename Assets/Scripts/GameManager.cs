namespace rumi
{
    using System.Collections.Generic;

    using UnityEngine;
    using System.Linq;
    using UnityEngine.UI;
    using TMPro;
    using System;

    public class GameManager : MonoBehaviour
    {
        // List of all players in the game
        List<Player> Players;

        // The current player's index
        int currentPlayerIndex = 0;

        // The current round
        int currentRoundNumber = 1;

        // Whether the game has ended
        bool gameEnded = false;

        [SerializeField]
        List<Card> Deck;

        [SerializeField]
        List<Round> Rounds;

        [SerializeField]
        TextMeshProUGUI RoundText;

        [SerializeField]
        TextMeshProUGUI MeldsText;

        [SerializeField]
        public GameObject CardPrefab;

        [SerializeField]
        RectTransform StockParent;

        [SerializeField]
        RectTransform DiscardPileParent;

        [SerializeField]
        public Sprite CardBackSide;

        [SerializeField]
        public StockPile StockPileComponent => FindObjectOfType<StockPile>();

        [SerializeField]
        public DiscardPile DiscardPileComponent => FindObjectOfType<DiscardPile>();

        private Player currentPlayer => Players[currentPlayerIndex];

        private Round currentRound => Rounds[currentRoundNumber - 1];

        // The player who won the game
        private Player winner;

        void Start()
        {
            // Initialize the players, stock pile, and discard pile
            InitializeGame();

            // Start the first round
            StartRound();
        }

        void Update()
        {
            // Check if the game has ended
            if (gameEnded)
            {
                // Display the final scores and end the game
                DisplayFinalScores();
                EndGame();
            }
        }

        // Initializes the game by shuffling and dealing the cards and setting up the players, stock pile, and discard pile
        void InitializeGame()
        {
            // Initialise the players
            Players = FindObjectsOfType<Player>().ToList();

            // Create a list of all cards in the game (two decks)
            var totalDecksToUse = Mathf.FloorToInt(Players.Count / 2f);
            var allCards = new List<Card>();
            for (int i = 0; i < totalDecksToUse; i++)
            {
                allCards.AddRange(Deck.Select(x => new Card(x.Rank, x.Suit, x.Artwork)));
            }

            // Shuffle the cards
            ShuffleCards(allCards);

            // Add cards to the deck UI
            this.StockPileComponent.Initialise(allCards);

            DiscardPileComponent.CardDiscarded += DiscardPileComponent_CardDiscarded;
        }

        // Shuffles the list of cards using the Fisher-Yates shuffle algorithm
        void ShuffleCards(List<Card> cards)
        {
            System.Random random = new System.Random();
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        // Starts a new round by resetting the player's melds and points for the round and allowing the first player to make their first move
        void StartRound()
        {
            RoundText.text = $"ROUND {currentRoundNumber}";
            MeldsText.text = currentRound.ToString();

            foreach (Player player in Players)
            {
                player.Melds.Clear();
                player.Points = 0;
            }

            for (int i = 0; i < 11; i++)
            {
                foreach (Player player in Players)
                {
                    player.AddCard(StockPileComponent.DrawCard());
                }
            }

            // Set up the discard pile
            DiscardPileComponent.Add(StockPileComponent.DrawCard(), false);

            currentPlayer.MakeMove();
        }

        private void DiscardPileComponent_CardDiscarded()
        {
            currentPlayer.EndMove();
            currentPlayerIndex = (currentPlayerIndex + 1) % 4;
            currentPlayer.MakeMove();
        }

        // Calculates the current player's points for the round
        void CalculatePoints()
        {
            Player currentPlayer = Players[currentPlayerIndex];
            foreach (Card card in currentPlayer.Hand)
            {
                if (card.Rank == CardRank.Three || card.Rank == CardRank.Four || card.Rank == CardRank.Five || card.Rank == CardRank.Six || card.Rank == CardRank.Seven || card.Rank == CardRank.Eight || card.Rank == CardRank.Nine)
                {
                    currentPlayer.Points += 5;
                }
                else if (card.Rank == CardRank.Ten || card.Rank == CardRank.Jack || card.Rank == CardRank.Queen || card.Rank == CardRank.King)
                {
                    currentPlayer.Points += 10;
                }
                else if (card.Rank == CardRank.Ace || card.Rank == CardRank.Two)
                {
                    currentPlayer.Points += 20;
                }
            }
        }

        // Ends the current round and begins the next round or ends the game if all rounds have been completed
        void EndRound()
        {
            // Calculate the points for the players who did not go out
            foreach (Player player in Players)
            {
                if (player.Hand.Count > 0)
                {
                    CalculatePoints();
                }
            }

            // Check if the game is over
            if (currentRoundNumber == 10)
            {
                // End the game
                EndGame();
            }
            else
            {
                // Start the next round
                currentRoundNumber++;
                currentPlayerIndex = (currentPlayerIndex + 1) % 4;
                StartRound();
            }
        }

        // Ends the game and calculates the final scores for each player
        void EndGame()
        {
            // Calculate the final scores for each player
            foreach (Player player in Players)
            {
                player.TotalPoints += player.Points;
            }

            // Determine the winner
            winner = Players.OrderBy(player => player.TotalPoints).First();

            // Display the final scores and winner
            // You can use UI text or other methods to display this information to the player
            Debug.Log("Game Over!");
            Debug.Log("Scores:");
            foreach (Player player in Players)
            {
                Debug.Log(player.TotalPoints + " points");
            }
            Debug.Log("Winner: " + winner.TotalPoints + " points");
        }

        // Displays the final scores and winner of the game
        public void DisplayFinalScores()
        {
            // Get a reference to the UI text element that will display the scores
            Text scoresText = GameObject.Find("ScoresText").GetComponent<Text>();

            // Build the scores text string
            string scoresString = "Game Over!\n\nScores:\n";
            foreach (Player player in Players)
            {
                scoresString += player.TotalPoints + " points\n";
            }
            scoresString += "\nWinner: " + winner.TotalPoints + " points";

            // Display the scores
            scoresText.text = scoresString;
        }
    }
}