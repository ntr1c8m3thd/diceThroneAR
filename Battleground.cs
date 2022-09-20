using System; using System.Collections.Generic; using System.Linq;
using System.Text; using System.Threading.Tasks; using System.Threading;

namespace diceThroneAR
{
    class Battleground                                                          //WHAT IS BATTLEGROUND? -- Functions that help the GameFlow class.
    {
        public static void printEachChar(string word)
        {
            for (int counter = 0; counter < word.Length; counter++)
            { Console.Write(word[counter].ToString()); Thread.Sleep(10); }
        }
        public static void shuffleCards(Character p, int deckSize)
        {
            //Fisher-Yates shuffle algorithm
            int i = 0, j = 0, MAXCARDS = deckSize;
            Random sortRandom = new Random();
            for (i = 0; i <= (MAXCARDS - 1); i++)
            {
                j = Convert.ToInt32(sortRandom.Next(0, i + 1));
                var deck = p.cards[i];
                p.cards[i] = p.cards[j];
                p.cards[j] = deck;
            }
        }
        public static Character rollForFirst(Character p1, Character p2)        //TODO: Update parameters to take List<Characters> and add TIEBREAKER protocol
        {
            int player = 1, highestRoll = 0, whoRolledHighest = 0;
            Console.WriteLine("/**************************************4DBUGG1NG************************************************/");
            while (GameFlow.numberOfPlayers != 0)
            {
                Console.WriteLine($"\nPlayer {player}: Press enter to roll:");
                Console.ReadLine();
                Random roll = new Random(); int rollResult = roll.Next(1, 7);
                Console.WriteLine($"Player {player}: You rolled a got damn {rollResult}!");
                if (rollResult > highestRoll) { highestRoll = rollResult; whoRolledHighest = player; };
                player++; GameFlow.numberOfPlayers--;
            };
            switch (whoRolledHighest)
            {
                case 1: p1.winFirstRoll = true; break;
                case 2: p2.winFirstRoll = true; break;
            }
            if (p1.winFirstRoll == true) { Console.WriteLine($"\nPlayer {p1.team}, {p1.name}, rolls first!\n"); return p1; }
            else { Console.WriteLine($"\nPlayer {p2.team}, {p2.name}, rolls first!\n"); return p2; }
        }
        public static void playMainPhaseHand(Character activePlayer)            //Control system that check's boolean isPlayable for each phase and either plays card or denies play.
        {
            foreach (Cards card in activePlayer.cards) if (card.Drawn == true) if (card.Type == 1 || card.Type == 4) card.isPlayable = true;
            //sets the boolean isPlayable to true if either Main Phase or Hero Upgrade card.
            int choice = 99;
            do                                                                  //TODO: Modify to only allow 1-(number of cards in hand)
            {                                                                   //then display that number as the range of acceptible inputs. 
                Console.WriteLine($"Player {activePlayer.team}, {activePlayer.name} please enter the number of the card you wish to play.\n" +
                    "Allowed cards in this phase: Main Phase / Hero Upgrade / Instant Action Cards.\n" +
                    "(Press 0 to view your current hand or -1 to advance to the next phase.)\n"); ;
                if (!int.TryParse(Console.ReadLine(), out int num))
                    Console.WriteLine("Invalid value entered, try again.");
                else if ((num < -1 || num > 10)) Console.WriteLine("Value must be within -1 and 10");
                else
                {
                    if (num == 0)
                    {
                        int cardCounter = 1; foreach (Cards card in activePlayer.cards) if (card.Drawn == true)
                            {
                                Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++;
                            }
                    }
                    else if (num > 0 && num < 11)
                    {
                        if (activePlayer.cards[num - 1 + activePlayer.cardsPlayed].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                        else
                        {
                            Console.WriteLine($"You played {activePlayer.cards[num - 1 + activePlayer.cardsPlayed].Name}");
                            activePlayer.cards[num - 1 + activePlayer.cardsPlayed].Action(); activePlayer.cardsPlayed++;
                        }
                    }
                    else if (num == -1) { Console.WriteLine("Advancing to next phase..."); choice = -1; }
                }
            }
            while (choice != -1);
            foreach (Cards card in activePlayer.cards) if (card.Drawn == true) if (card.Type == 1 || card.Type == 4) card.isPlayable = false; //flips bool isPlayable to False at end of phase
        }
        public static void playRollPhaseHand(Character activePlayer)
        {
            foreach (Cards card in activePlayer.cards) if (card.Drawn == true) if (card.Type == 2) card.isPlayable = true;
            //sets the boolean isPlayable to true if Roll Phase card.
            int choice = 99;
            do                                                                  //TODO: Modify to only allow 1-(number of cards in hand)
            {                                                                   //then display that number as the range of acceptible inputs. 
                Console.WriteLine($"Player {activePlayer.team}, {activePlayer.name} please enter the number of the card you wish to play.\n" +
                    "Allowed cards in this phase: Roll Phase / Instant Action Cards.\n" +
                    "(Press 0 to view your current hand or -1 to advance to the next phase.)\n"); ;
                if (!int.TryParse(Console.ReadLine(), out int num))
                    Console.WriteLine("Invalid value entered, try again.");
                else if ((num < -1 || num > 10)) Console.WriteLine("Value must be within -1 and 10");
                else
                {
                    if (num == 0)
                    {
                        int cardCounter = 1; foreach (Cards card in activePlayer.cards) if (card.Drawn == true)
                            {
                                Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++;
                            }
                    }
                    else if (num > 0 && num < 11)
                    {
                        if (activePlayer.cards[num - 1 + activePlayer.cardsPlayed].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                        else
                        {
                            Console.WriteLine($"You played {activePlayer.cards[num - 1 + activePlayer.cardsPlayed].Name}");
                            activePlayer.cards[num - 1 + activePlayer.cardsPlayed].Action(); activePlayer.cardsPlayed++;
                        }
                    }
                    else if (num == -1) { Console.WriteLine("Advancing to next phase..."); choice = -1; }
                }
            }
            while (choice != -1);
            foreach (Cards card in activePlayer.cards) if (card.Drawn == true) if (card.Type == 2) card.isPlayable = false; //flips bool isPlayable to False at end of phase
        }
    }

    class Cards
    {
        public int CPCost { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Type { get; set; }
        public bool Drawn { get; set; } = false;
        public bool Discarded { get; set; } = false;
        public bool isPlayable { get; set; } = false;
        public Cards(int cpcost, string name, string desc)
        {
            this.CPCost = cpcost; this.Name = name.ToUpper(); this.Desc = desc;
        }
        public virtual void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + "\nDescription: " + Desc );
        }
        public void Action() { Drawn = false; Discarded = true; }
    }
    class MainPhaseCard : Cards
    {
        new public string Type = "Main Phase Card"; // 1

        public MainPhaseCard(int cpcost, string name, string desc) : base(cpcost, name, desc)
        { base.Type = 1; }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + " || Is Playable? = " + isPlayable + "\n" +
                "Type: " + Type + " || Description: " + Desc + "\n");
        }
    }
    class RollPhaseCard : Cards
    {
        new public string Type = "Roll Phase Card"; // 2

        public RollPhaseCard(int cpcost, string name, string desc) : base(cpcost, name, desc)
        { base.Type = 2; }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + " || Is Playable? = " + isPlayable + "\n" +
                "Type: " + Type + " || Description: " + Desc + "\n");
        }
    }
    class InstantActionCard : Cards
    {
        new public string Type = "Instant Action Card"; //3
        

        public InstantActionCard(int cpcost, string name, string desc) : base(cpcost, name, desc)
        { base.Type = 3; isPlayable = true; }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + " || Is Playable? = " + isPlayable + "\n" +
                "Type: " + Type + " || Description: " + Desc + "\n");
        }
    }
    class HeroUpgradeCard : Cards
    {
        new public string Type = "Hero Upgrade Card"; //4

        public HeroUpgradeCard(int cpcost, string name, string desc) : base(cpcost, name, desc)
        { base.Type = 4; }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + " || Is Playable? = " + isPlayable + "\n" +
                "Type: " + Type + " || Description: " + Desc + "\n");
        }
    }
}

/*public static void jaggedLesson() //Creating an Array within An Array
{
    //---JAGGED ARRAYS-- -
    int[][] jaggedArray = new int[3][];
    jaggedArray[0] = new int[3]; // or new int[] {value1, value2, value3};
    jaggedArray[1] = new int[2]; // or new int[] {value1, value2};
    jaggedArray[2] = new int[1]; // or new int[] {value1};

    //\/ Alternative way /\/

    int value1 = 5, value2 = 10, value3 = 15;
    int[][] jaggedArray2 = new int[][] {
                new int[] {value1, value2, value3},
                new int[] {value1, value2},
                new int[] {value1}
            };

    Console.WriteLine("The Value in the middle of the first entry is {0}", jaggedArray2[0][2]);
    int x = 0, y = 0; foreach (int[] test in jaggedArray2)
    {
        Console.WriteLine("\nElement ({0})", x);
        foreach (int boo in test)
        {
            Console.WriteLine("At Element ({0}, {1}): {2} ", x, y, boo); y++;
        }
        x++; y = 0;
    }
    Console.WriteLine();

    //Teacher's way ((SUPERIOR))
    for (int i = 0; i < jaggedArray2.Length; i++)
    {
        Console.WriteLine("\nElement {0}", i);
        for (int j = 0; j < jaggedArray2[i].Length; j++) Console.WriteLine("At Element {0}, {1}: {2} ", i, j, jaggedArray2[i][j]);
    }

    string[][] friendsAndFamily = new string[][]
    {
                new string[] {"Andre", "Cameron", "Sister"},
                new string[] {"Zachary", "Caitlyn", "Milli"},
                new string[] {"Michael", "Colleen", "Dan" }
    };

    for (int i = 0; i < friendsAndFamily.Length; i++)
    {
        Console.Write("\nFamily Members of {0}'s are ", friendsAndFamily[i][0]);
        for (int j = 1; j < 2; j++) Console.WriteLine("{0} and {1}. Pleasure to meet you!", friendsAndFamily[i][j], friendsAndFamily[i][j + 1]);
    }
}

public static double GetAverage(int[] gradesArray) //Convenient method using arrays
{
    int size = gradesArray.Length;
    double average;
    int sum = 0;

    for (int i = 0; i < size; i++)
        sum += gradesArray[i];
    average = (double)sum / size;
    return average;
}

public static void testGetAverage()
{
    int[] studentsGrades = new int[] { 15, 13, 8, 12, 6, 16 };
    double averageResult = GetAverage(studentsGrades);
    Console.WriteLine("The average is: {0}", averageResult);
    Console.ReadKey();
}*/