using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace matching
{
    public struct CardType
    {
        public const string CardBack = "🂠";
        public const string DarkSpade = "♠";
        public const string LightSpade = "♤";
        public const string DarkHeart = "♥";
        public const string LightHeart = "♡";
        public const string DarkClub = "♣";
        public const string LightClub = "♧";
        public const string DarkDiamond = "♦";
        public const string LightDiamond = "♢";
    }

    class Program
    {
        static List<Card> Deck = new List<Card>();
        static Card PickedCard;
        static int MaxTries = 3;

        static void Main(string[] args)
        {
            Clear();
            CreateDeck();

            // pick a random card for the player to guess
            PickRandomCard();

            DrawDeck();

            var tries = 0;

            // the game loop...
            while (true)
            {
                WriteLine("Pick a card. Any card...");
                var guess = ReadKey().KeyChar.ToString();

                // check for errors
                if (!int.TryParse(guess, out int guessedNumber))
                {
                    ShowError("\n\nYou have to enter a number, ya goof!");
                    continue;
                }

                if (guessedNumber < 1 || guessedNumber > Deck.Count)
                {
                    ShowError($"\n\nTry a number between 1 and {Deck.Count} please.");
                    continue;
                }

                var guessedCard = Deck[guessedNumber - 1];

                // check to see if they picked that one already
                if (guessedCard.FaceUp)
                {
                    ShowError($"\n\nYou already picked that one, silly. Try another one.");
                    continue;
                }

                // update the try count and get the card they guessed
                tries++;

                // turn over the card they picked
                Clear();
                guessedCard.FaceUp = true;
                DrawDeck(tries);

                // check if they got it right or not...
                if (guessedCard.Color == PickedCard.Color && guessedCard.Symbol == PickedCard.Symbol)
                {
                    ForegroundColor = ConsoleColor.Green;
                    WriteLine($"\n\nYOU GOT IT! NICE JOB!\n\nIt only took you {tries} {(tries == 1 ? "try" : "tries")}!\n\n");
                    ResetColors();
                    break;
                }
                else
                {
                    ForegroundColor = ConsoleColor.Yellow;
                    var triesLeft = MaxTries - tries;
                    if (tries >= MaxTries)
                    {
                        WriteLine($"\n\nYou guessed {guess}, which is incorrect.\n\nSorry, game over. Play again to see if you can win!\n\n");
                        break;
                    }
                    else
                    {
                        WriteLine($"\n\nYou guessed {guess}, which is incorrect.\n\nYou have {triesLeft} {(triesLeft == 1 ? "try" : "tries")} left.\n\n");
                    }
                    ResetColors();
                }
            }
        }

        static void CreateDeck()
        {
            Deck.Clear();

            // create 9 cards (this creates duplicates, but is fine for now)
            for (int c = 1; c <= 9; c++)
            {
                Deck.Add(new Card
                {
                    BackColor = (c % 2 == 0) ? ConsoleColor.DarkRed : ConsoleColor.DarkBlue,
                    SymbolColor = (c % 2 == 0) ? ConsoleColor.Magenta : ConsoleColor.Cyan,
                    Color = (c % 2 == 0) ? "red" : "blue",
                    Symbol = (c % 5 == 0) ? CardType.DarkClub : (c % 3 == 0) ? CardType.DarkDiamond : (c % 2 == 0) ? CardType.DarkHeart : CardType.DarkSpade
                });
            }

            // shuffle the new deck
            Deck = Deck.OrderBy(o => Guid.NewGuid()).ToList();
        }

        static void ResetColors()
        {
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.White;
        }

        static void DrawDeck(int tries = 0)
        {
            ResetColor();
            var triesLeft = MaxTries - tries;
            ForegroundColor = ConsoleColor.Green;
            WriteLine($"Guess the Card!\n\n");

            AskThePlayerToGuess();

            ResetColors();
            Write("\t");
            for (int c = 1; c <= 9; c++)
            {
                Deck[c - 1].Draw(c);

                ResetColors();
                if (c % 3 == 0)
                    Write(Environment.NewLine);
                Write("\t");
            }

            WriteLine("");
        }

        static void PickRandomCard()
        {
            var rnd = new Random();
            PickedCard = Deck[rnd.Next(Deck.Count)];
        }

        static void AskThePlayerToGuess()
        {
            ResetColors();
            Write($"Guess which card is a ");
            ForegroundColor = PickedCard.SymbolColor;
            Write($"{PickedCard.Color} {PickedCard.Symbol}");
            ResetColors();
            Write($".\n\n");
        }

        static void ShowError(string message)
        {
            ForegroundColor = ConsoleColor.Red;
            WriteLine(message);
            ResetColors();
        }
    }

    class Card
    {
        public ConsoleColor BackColor;
        public ConsoleColor SymbolColor;
        public string Color;
        public string Symbol;
        public bool FaceUp;

        public void Draw(int number)
        {
            ForegroundColor = ConsoleColor.Gray;
            Write($"{number}) ");
            if (FaceUp)
            {
                BackgroundColor = BackColor;
                ForegroundColor = SymbolColor;
                Write(Symbol);
            }
            else
            {
                BackgroundColor = ConsoleColor.Black;
                ForegroundColor = ConsoleColor.White;
                Write(CardType.CardBack);
            }
        }
    }
}
