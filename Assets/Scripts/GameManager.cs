namespace rumi
{
    using System.Collections.Generic;
    using System;

    using UnityEngine;
    using System.Linq;
    using UnityEngine.UI;

    public class GameManager : MonoBehaviour
    {
        // List of all players in the game
        List<Player> players;

        // The current player's index
        int currentPlayerIndex = 0;

        // The current round
        int currentRound = 1;

        // The stock pile
        Queue<Card> stockPile;

        // The discard pile
        Stack<Card> discardPile;

        // Whether the game has ended
        bool gameEnded = false;

        [SerializeField]
        List<Card> Deck;

        [SerializeField]
        public GameObject CardPrefab;

        [SerializeField]
        RectTransform StockParent;

        [SerializeField]
        RectTransform DiscardPileParent;

        [SerializeField]
        public Sprite CardBackSide;

        [SerializeField]
        public Deck DeckComponent => FindObjectOfType<Deck>();

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
            // Create a list of all cards in the game (two decks)
            List<Card> allCards = this.Deck.Union(this.Deck).ToList();

            // Shuffle the cards
            ShuffleCards(allCards);

            // Add cards to the deck UI
            this.DeckComponent.InitialiseCards(allCards);

            // Deal the cards to the players
            players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                players.Add(new Player(new List<Card>(), new List<Meld>(), 3, 0));
            }

            for (int i = 0; i < 11; i++)
            {
                foreach (Player player in players)
                {
                    player.Hand.Add(allCards[0]);
                    allCards.RemoveAt(0);
                }
            }

            // Set up the stock pile and discard pile
            stockPile = new Queue<Card>(allCards);
            discardPile = new Stack<Card>();
            discardPile.Push(stockPile.Dequeue());
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
            foreach (Player player in players)
            {
                player.Melds.Clear();
                player.Points = 0;
            }
            MakeMove();
        }

        // Allows the current player to make their move by drawing or discarding a card and attempting to meld off their cards
        void MakeMove()
        {
            Player currentPlayer = players[currentPlayerIndex];

            // Check if the current player has any buys remaining
            if (currentPlayer.BuysRemaining > 0 && CanBuyCard())
            {
                // Allow the player to buy the top card from the discard pile
                BuyCard();
            }
            else
            {
                // Allow the player to draw a card from the stock pile or discard pile
                DrawCard();
            }

            // Attempt to meld off the current player's cards
            MeldCards();

            // Check if the current player has gone out
            if (currentPlayer.Hand.Count == 0)
            {
                // End the current round
                EndRound();
            }
            else
            {
                // Move to the next player
                currentPlayerIndex = (currentPlayerIndex + 1) % 4;
                MakeMove();
            }
        }

        // Determines if the current player can buy the card on the top of the discard pile
        public bool CanBuyCard()
        {
            // Get the current player and the card on the top of the discard pile
            Player currentPlayer = players[currentPlayerIndex];
            Card card = discardPile.Peek();

            // Check if the player has any remaining buys
            if (currentPlayer.BuysRemaining > 0)
            {
                // Check if the player has the card in their hand or melds
                if (currentPlayer.Hand.Contains(card) || currentPlayer.Melds.Any(meld => meld.Cards.Contains(card)))
                {
                    // The player cannot buy the card because they already have it
                    return false;
                }
                else
                {
                    // The player can buy the card
                    return true;
                }
            }
            else
            {
                // The player has no remaining buys
                return false;
            }
        }

        // Allows the current player to buy the top card from the discard pile
        void BuyCard()
        {
            Player currentPlayer = players[currentPlayerIndex];
            currentPlayer.Hand.Add(discardPile.Pop());
            currentPlayer.Hand.Add(stockPile.Dequeue());

            currentPlayer.BuysRemaining--;
        }

        // Allows the current player to draw a card from the stock pile or discard pile
        void DrawCard()
        {
            Player currentPlayer = players[currentPlayerIndex];
            bool drewFromStock = false;

            // Check if the player wants to draw from the stock pile or discard pile
            // You can use UI buttons or other input methods to allow the player to make their choice

            if (drewFromStock)
            {
                currentPlayer.Hand.Add(stockPile.Dequeue());
            }
            else
            {
                currentPlayer.Hand.Add(discardPile.Pop());
            }
        }

        // Attempts to meld off the current player's cards
        void MeldCards()
        {
            Player currentPlayer = players[currentPlayerIndex];

            // Check if the current player has made the required first meld for the round
            bool madeFirstMeld = false;
            switch (currentRound)
            {
                case 1:
                    madeFirstMeld = MeldSet(currentPlayer, 3) && MeldSet(currentPlayer, 3);
                    break;
                case 2:
                    madeFirstMeld = MeldSet(currentPlayer, 3) && MeldRun(currentPlayer, 4);
                    break;
                case 3:
                    madeFirstMeld = MeldRun(currentPlayer, 4) && MeldRun(currentPlayer, 4);
                    break;
                case 4:
                    madeFirstMeld = MeldSet(currentPlayer, 3) && MeldSet(currentPlayer, 3) && MeldSet(currentPlayer, 3);
                    break;
                case 5:
                    madeFirstMeld = MeldSet(currentPlayer, 3) && MeldSet(currentPlayer, 3) && MeldRun(currentPlayer, 4);
                    break;
                case 6:
                    madeFirstMeld = MeldSet(currentPlayer, 3) && MeldRun(currentPlayer, 4) && MeldRun(currentPlayer, 4);
                    break;
                case 7:
                    madeFirstMeld = MeldRun(currentPlayer, 4) && MeldRun(currentPlayer, 4) && MeldRun(currentPlayer, 4);
                    break;
                case 8:
                    madeFirstMeld = MeldSet(currentPlayer, 3) && MeldRun(currentPlayer, 10);
                    break;
                case 9:
                    madeFirstMeld = MeldSet(currentPlayer, 3) && MeldSet(currentPlayer, 3) && MeldSet(currentPlayer, 3) && MeldRun(currentPlayer, 5);
                    break;
                case 10:
                    madeFirstMeld = MeldRun(currentPlayer, 5) && MeldRun(currentPlayer, 5) && MeldRun(currentPlayer, 5);
                    break;
            }

            if (!madeFirstMeld)
            {
                // The player has not made the required first meld yet, so they must continue to try until they succeed or run out of moves
                return;
            }

            // The player has made the required first meld, so they can now try to meld off their other cards
            while (MeldCards(currentPlayer))
            {
                // Keep melding until no more melds can be made
            }
        }

        // Ends the current player's turn by discarding a card
        public void EndTurn()
        {
            // Get the current player and the melds required in the current round
            Player currentPlayer = players[currentPlayerIndex];
            List<Meld> requiredMelds = new List<Meld>(); //GetRequiredMelds(currentRound);

            // Find the most useless card in the current player's hand
            Card mostUselessCard = null;
            int minScore = int.MaxValue;
            foreach (Card card in currentPlayer.Hand)
            {
                // Calculate the score of the card based on its usefulness for the required melds
                int score = CalculateCardScore(card, currentPlayer.Hand, requiredMelds);

                // Update the most useless card if the score is lower
                if (score < minScore)
                {
                    mostUselessCard = card;
                    minScore = score;
                }
            }

            // Discard the most useless card
            currentPlayer.Hand.Remove(mostUselessCard);
            discardPile.Push(mostUselessCard);

            // Check if the current player has gone out
            if (currentPlayer.Hand.Count == 0)
            {
                // The current player has gone out, end the game
                EndGame();
            }
            else
            {
                // The current player's turn has ended, move to the next player
                //AdvanceTurn();
            }

        }
        // Calculates the score of the specified card based on its usefulness for the required melds
        private int CalculateCardScore(Card card, List<Card> hand, List<Meld> requiredMelds)
        {
            // Initialize the score to the value of the card
            int score = card.GetValue();

            // Check if the card can be used to complete any of the required melds
            foreach (Meld requiredMeld in requiredMelds)
            {
                // Check if the required meld is a run
                if (!requiredMeld.IsSet)
                {
                    // Check if the card can be added to the run
                    if (CanAddToRun(card, requiredMeld.Cards))
                    {
                        // The card can be added to the run, reduce the score
                        score -= 5;
                    }
                }
                else
                {
                    // The required meld is a set
                    // Check if the card can be added to the set
                    if (CanAddToSet(card, requiredMeld.Cards))
                    {
                        // The card can be added to the set, reduce the score
                        score -= 5;
                    }
                }
            }

            // Check if the card can be used to form a new meld
            List<Card> meldCards = new List<Card>();
            meldCards.Add(card);
            //if (CanMeldCards(meldCards))
            //{
            //    // The card can be used to form a new meld, reduce the score
            //    score -= 10;
            //}

            // Check if the card is a wildcard
            if (card.IsWild)
            {
                // The card is a wildcard, increase the score
                score += 5;
            }

            // Return the score
            return score;
        }

        // Determines if the specified card can be added to the specified run
        private bool CanAddToRun(Card card, List<Card> run)
        {
            // Check if the card is a wildcard
            if (card.IsWild)
            {
                // The card is a wildcard, it can be added to any run
                return true;
            }

            // Check if the run is empty
            if (run.Count == 0)
            {
                // The run is empty, the card can be added
                return true;
            }

            // Check if the card has the same suit as the run
            if (card.Suit != run[0].Suit)
            {
                // The card has a different suit than the run, it cannot be added
                return false;
            }

            // Check if the card is consecutive with the highest or lowest card in the run
            if (card.Rank == run[run.Count - 1].Rank + 1 || card.Rank == run[0].Rank - 1)
            {
                // The card is consecutive with the highest or lowest card in the run, it can be added
                return true;
            }

            // The card cannot be added to the run
            return false;

        }

        // Determines if the specified card can be added to the specified set
        private bool CanAddToSet(Card card, List<Card> set)
        {
            // Check if the card is a wildcard
            if (card.IsWild)
            {
                // The card is a wildcard, it can be added to any set
                return true;
            }

            // Check if the set is empty
            if (set.Count == 0)
            {
                // The set is empty, the card can be added
                return true;
            }

            // Check if the card has the same rank as the set
            if (card.Rank == set[0].Rank)
            {
                // The card has the same rank as the set, it can be added
                return true;
            }

            // The card cannot be added to the set
            return false;

        }

        // Attempts to meld off the current player's cards and returns true if a meld was made, false otherwise
        bool MeldCards(Player player)
        {
            // Check if the player can make a set meld
            for (int i = 0; i < player.Hand.Count; i++)
            {
                if (MeldSet(player, i))
                {
                    return true;
                }
            }

            // Check if the player can make a run meld
            for (int i = 0; i < player.Hand.Count; i++)
            {
                if (MeldRun(player, i))
                {
                    return true;
                }
            }

            return false;
        }


        // Attempts to make a set meld with the current player's cards and returns true if successful, false otherwise
        bool MeldSet(Player player, int index)
        {
            Card card = player.Hand[index];
            List<Card> setCards = new List<Card>();
            setCards.Add(card);

            // Find any other cards in the player's hand with the same rank as the selected card
            for (int i = 0; i < player.Hand.Count; i++)
            {
                if (i == index)
                {
                    continue;
                }

                if (player.Hand[i].Rank == card.Rank)
                {
                    setCards.Add(player.Hand[i]);
                }
            }

            // Check if the set meld is valid
            if (setCards.Count < 3)
            {
                return false;
            }

            // Remove the set meld cards from the player's hand
            foreach (Card setCard in setCards)
            {
                player.Hand.Remove(setCard);
            }

            // Add the set meld to the player's melds
            player.Melds.Add(new Meld(setCards, true));
            return true;
        }

        // Attempts to make a run meld with the current player's cards and returns true if successful, false otherwise
        bool MeldRun(Player player, int index)
        {
            Card card = player.Hand[index];
            List<Card> runCards = new List<Card>();
            runCards.Add(card);

            // Find any other cards in the player's hand that can be added to the run
            for (int i = index + 1; i < player.Hand.Count; i++)
            {
                Card otherCard = player.Hand[i];
                if (otherCard.Suit == card.Suit && (int)otherCard.Rank == (int)card.Rank + runCards.Count)
                {
                    runCards.Add(otherCard);
                }
            }
            // Check if the run meld is valid
            if (runCards.Count < 3)
            {
                return false;
            }

            // Remove the run meld cards from the player's hand
            foreach (Card runCard in runCards)
            {
                player.Hand.Remove(runCard);
            }

            // Add the run meld to the player's melds
            player.Melds.Add(new Meld(runCards, false));
            return true;
        }

        // Calculates the current player's points for the round
        void CalculatePoints()
        {
            Player currentPlayer = players[currentPlayerIndex];
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
            foreach (Player player in players)
            {
                if (player.Hand.Count > 0)
                {
                    CalculatePoints();
                }
            }

            // Check if the game is over
            if (currentRound == 10)
            {
                // End the game
                EndGame();
            }
            else
            {
                // Start the next round
                currentRound++;
                currentPlayerIndex = (currentPlayerIndex + 1) % 4;
                StartRound();
            }
        }

        // Ends the game and calculates the final scores for each player
        void EndGame()
        {
            // Calculate the final scores for each player
            foreach (Player player in players)
            {
                player.TotalPoints += player.Points;
            }

            // Determine the winner
            winner = players.OrderBy(player => player.TotalPoints).First();

            // Display the final scores and winner
            // You can use UI text or other methods to display this information to the player
            Debug.Log("Game Over!");
            Debug.Log("Scores:");
            foreach (Player player in players)
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
            foreach (Player player in players)
            {
                scoresString += player.TotalPoints + " points\n";
            }
            scoresString += "\nWinner: " + winner.TotalPoints + " points";

            // Display the scores
            scoresText.text = scoresString;
        }
    }
}