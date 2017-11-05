using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class UniqueCard
    {
        public UniqueCard(int card, int suit)
        {
            Card = card;
            Suit = suit;
        }

        public int Card { get; set; }
        public int Suit { get; set; }
    }

    class Program
    {

        //just for reference
        string[] Cards = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        string[] Suits = { "Clubs", "Diamonds", "Hearts", "Spades" };
        string[] Scores = { "Royal Flush", "Straight Flush", "Four of Kind", "Full House", "Flush", "Straight", "Three of Kind", "Two Pair", "One Pair", "High Card" };
        int HandSize = 5;

        List<UniqueCard> Pack;

        /// <summary>
        /// Generate card combinatios, unnecessary waste of memory but improves readability of code
        /// </summary>
        private void BuildPack()
        {
            Pack = new List<UniqueCard>();
            for (int i = 0; i < Cards.Length; i++)
            {
                for (int j = 0; j < Suits.Length; j++)
                {
                    Pack.Add(new UniqueCard(i, j));
                }
            }
        }

        List<List<UniqueCard>> Players = new List<List<UniqueCard>>();

        private void PickCards()
        {
            List<UniqueCard> PlayerCards = new List<UniqueCard>();
            Random rnd = new Random();
            for (int i = 0; i < HandSize; i++)
            {
                int randomNumber = rnd.Next(0, Pack.Count() - 1);
                PlayerCards.Add(new UniqueCard(Pack.ElementAt(randomNumber).Card, Pack.ElementAt(randomNumber).Suit));
                Pack.RemoveAt(randomNumber);
            }
            Players.Add(PlayerCards.OrderBy(o => o.Card).ToList());
            //
        }

        private string ScorePlayer(List<UniqueCard> Hand)
        {
            Dictionary<int, int> CardCount = new Dictionary<int, int>();
            int CardIndex = 0, SequenceCount = 0;
            foreach (var Card in Hand)
            {
                if (CardIndex != Hand.Count - 1 && (Hand.ElementAt(CardIndex + 1).Card - Card.Card == 1)) SequenceCount++;
                if (!CardCount.ContainsKey(Card.Card)) CardCount[Card.Card] = 0;
                CardCount[Card.Card]++;
                CardIndex++;
            }


            int SuitCount = 1;
            if (SequenceCount == 5 || (SequenceCount == 4 && Hand.ElementAt(5).Card - Hand.ElementAt(0).Card == 12)) //got a sequence!
            {

                for (int i = 1; i < Hand.Count; i++) if (Hand.ElementAt(i).Suit == Hand.ElementAt(0).Suit) SuitCount++;

                if (SuitCount == 5)
                {
                    if (Hand.ElementAt(Hand.Count - 1).Card == 12)
                    {
                        return "Royal Flush";
                    }

                    return "Straight Flush";
                }

                return "Straight";
            }

            if (SuitCount == 5)
            {
                return "Flush";
            }

            Dictionary<int, int> StacksCount = new Dictionary<int, int>();
            for (int i = 2; i < 5; i++) StacksCount[i] = 0;
            foreach (var CardStack in CardCount.OrderByDescending(x => x.Value))
            {

                if (CardStack.Value == 4)
                {
                    StacksCount[4]++;
                }
                if (CardStack.Value == 3)
                {
                    StacksCount[3]++;
                }
                if (CardStack.Value == 2)
                {
                    StacksCount[2]++;
                }


            }

            if (StacksCount[4] == 1)
            {
                return "Four of Kind";
            }

            if (StacksCount[3] == 1 && StacksCount[2] == 1)
            {
                return "Full House";
            }
            if (StacksCount[2] == 2)
            {
                return "Two Pair";
            }
            if (StacksCount[2] == 1)
            {
                return "One Pair";
            }

            return "High Card";

        }

        static void Main(string[] args)
        {
            Console.WriteLine("How many players?");
            int PlayersCount = Convert.ToInt32(Console.ReadLine());

            Program program = new Program();
            program.BuildPack();

            for (int i = 0; i < PlayersCount; i++)
            {
                program.PickCards();
            }


            Dictionary<int,int> PlayersRanking = new Dictionary<int, int>();
            foreach (var Hand in program.Players)
            {
                 PlayersRanking[program.Players.IndexOf(Hand)] = Array.IndexOf(program.Scores, program.ScorePlayer(Hand));
            }

            int Places = 1;
            foreach (var Player in PlayersRanking.OrderBy(x => x.Value))
            {
                Console.Write("\n {0} Place - Player {1} has a: {2} \n",Places, Player.Key+1, program.Scores[Player.Value]);
                foreach (var Card in program.Players.ElementAt(Player.Key))
                    Console.Write("[{0} {1}] ", program.Cards[Card.Card] , program.Suits[Card.Suit]);

                Places++;
            }

        }

    }
}
