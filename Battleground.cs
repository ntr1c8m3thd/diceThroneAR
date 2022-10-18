using System; using System.Collections.Generic; using System.Linq;
using System.Text; using System.Threading.Tasks; using System.Threading;

namespace diceThroneAR
{
    //BATTLEGROUND -- Functions that help the GameFlow class.
    class Battleground                                                          
    {
        public static Character currentPlayer;
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
                Console.WriteLine($"Player {player}: You rolled a {rollResult}!");
                if (rollResult > highestRoll) { highestRoll = rollResult; whoRolledHighest = player; };
                player++; GameFlow.numberOfPlayers--;
            };
            switch (whoRolledHighest)
            {
                case 1: p1.WinFirstRoll = true; break;
                case 2: p2.WinFirstRoll = true; break;
            }
            if (p1.WinFirstRoll == true) { Console.WriteLine($"\nPlayer {p1.team}, {p1.name}, rolls first!\n"); return p1; }
            else { Console.WriteLine($"\nPlayer {p2.team}, {p2.name}, rolls first!\n"); return p2; }
        }

        //playMainPhaseHand(Character activePlayer) allows for a Main Phase or Hero Upgrade card to be played.
        public static void playMainPhaseHand(Character activePlayer)
        {
            currentPlayer = activePlayer;
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
            foreach (Cards card in activePlayer.cards) if (card.Drawn == true) if (card.Type == 2) card.isPlayable = true; //sets the boolean isPlayable to true if Roll Phase card.
            int choice = -2, choice2 = -2, choice3 = -2, choice4 = -2; Random roll = new Random(); int[] RollResults = new int[5];
            string RR = $"You rolled 1: {RollResults[0]} || 2: {RollResults[1]} || 3: {RollResults[2]} || 4: {RollResults[3]} || and 5: {RollResults[4]}!"; 
            Console.WriteLine("/**************************************4DBUGG1NG************************************************/");

            do //==================================================================================================================================FIRST DO
            {
                Console.WriteLine($"Player {activePlayer.team}, {activePlayer.name} please enter the number of the card you wish to play.\n" +
                    "Allowed cards in this phase: Roll Phase / Instant Action Cards.\n" +
                    "(Enter 0 to view your current hand or enter 99 to roll your first roll attempt.)\n"); ;
                if (!int.TryParse(Console.ReadLine(), out int num)) Console.WriteLine("Invalid value entered, try again.");
                else if (num == 99)
                {
                    for (int x = 0; x < 5; x++) { RollResults[x] = roll.Next(1, 7); }
                    Console.WriteLine(RR); //TODO: correlate the number rolled with the type of item on the character's di
                    Console.WriteLine("Please enter the number of the move you wish to activate, the number of the card you wish to play, \n0 to view your hand or 99 to reroll some or all of your dice.");

                    do //==========================================================================================================================THEN DO
                    {
                        if (!int.TryParse(Console.ReadLine(), out int num2)) Console.WriteLine("Invalid value entered, try again.");
                        else if (num2 == 99) //this block  allows the player to pick 1-5 di(ce) and reroll
                        {
                            Console.WriteLine("Enter which di(ce) you wish to reroll. ie. 1 for the first di / 3 for the third di / 235 for  dice 2, 3 and 5.");
                            if (!int.TryParse(Console.ReadLine(), out int num3)) Console.WriteLine("Invalid value entered, try again.");
                            else if (num3 <= 0 || num3 > 12345) Console.WriteLine("Value must be within 1 and 12345");
                            else
                            {
                                switch (num3)
                                {
                                    case 1: RollResults[0] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 12: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 13: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 14: RollResults[0] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 15: RollResults[0] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 123: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 124: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 125: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 1234: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 1235: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 12345: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 134: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 1345: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    case 145: RollResults[0] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); Console.WriteLine(RR); break;
                                    //do 2-2345, 3-345, 4-5, 5 later
                                }
                            }
                        }
                        else if (num2 == 0)
                        {
                            int cardCounter = 1; foreach (Cards card in activePlayer.cards) if (card.Drawn == true) { Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++; }
                        }
                        else if (num2 > 0 && num2 < 11)
                        {
                            if (activePlayer.cards[num - 1 + activePlayer.cardsPlayed].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                            else { Console.WriteLine($"You played {activePlayer.cards[num - 1 + activePlayer.cardsPlayed].Name}"); activePlayer.cards[num - 1 + activePlayer.cardsPlayed].Action(); activePlayer.cardsPlayed++; }
                        }
                        else if ((num2 < 25 || num2 > 35)) Console.WriteLine("Value must be within 25 and 35");
                        else
                        {
                            switch (num2)
                            {
                                case 25: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                                case 26: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                                case 27: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                                case 28: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                                case 29: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                                case 30: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                                case 31: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                                case 32: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                                case 33: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                                case 34: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                                case 35: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; break;
                            }
                        }
                    }
                    while (choice2 != -1);

                 }
                else if (num == 0)
                {
                    int cardCounter = 1; foreach (Cards card in activePlayer.cards) if (card.Drawn == true) { Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++; }
                }
                else if (num > 0 && num < 11)
                {
                    if (activePlayer.cards[num - 1 + activePlayer.cardsPlayed].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                    else { Console.WriteLine($"You played {activePlayer.cards[num - 1 + activePlayer.cardsPlayed].Name}"); activePlayer.cards[num - 1 + activePlayer.cardsPlayed].Action(); activePlayer.cardsPlayed++; }
                }
                else if ((num < -1 || num > 10)) Console.WriteLine("Value must be within 0 and 10");
                else if (num == -1) { Console.WriteLine("Advancing to next phase..."); choice = -1; }
            }
            while (choice != -1);
            foreach (Cards card in activePlayer.cards) if (card.Drawn == true) if (card.Type == 2) card.isPlayable = false; //flips bool isPlayable to False at end of phase
        }
        
        /* 1st rendition
        public static void playRollPhaseHand(Character activePlayer)
        {
            foreach (Cards card in activePlayer.cards) if (card.Drawn == true) if (card.Type == 2) card.isPlayable = true;
            //sets the boolean isPlayable to true if Roll Phase card.
            int choice = -2; Random roll = new Random();
            int[] RollResults = new int[5];
            do                                                                  //TODO: Modify to only allow 1-(number of cards in hand)
            {                                                                   //then display that number as the range of acceptible inputs. 
                Console.WriteLine($"Player {activePlayer.team}, {activePlayer.name} please enter the number of the card you wish to play.\n" +
                    "Allowed cards in this phase: Roll Phase / Instant Action Cards.\n" +
                    "(Enter 0 to view your current hand or enter 99 to roll your first roll attempt.)\n"); ;
                if (!int.TryParse(Console.ReadLine(), out int num))
                    Console.WriteLine("Invalid value entered, try again.");
                else if (num == 99)
                {
                    for (int x = 0; x < 5; x++) { RollResults[x] = roll.Next(1, 7); }
                    Console.WriteLine($"You rolled 1: {RollResults[0]} || 2: {RollResults[1]} || 3: {RollResults[2]} || 4: {RollResults[3]} || and 5: {RollResults[4]}!"); //TODO: correlate the number rolled with the type of item on the character's di
                    Console.WriteLine("Please enter the number of the move you wish to activate, the number of the card you wish to play, \n0 to view your hand or 99 to reroll some or all of your dice.");
                    if (!int.TryParse(Console.ReadLine(), out int num2))
                        Console.WriteLine("Invalid value entered, try again.");
                    else if (num2 == 99)
                    {
                        //this block  allows the player to pick 1-5 dice and reroll
                        Console.WriteLine("Enter which di(ce) you wish to reroll. ie. 35 if dice 3 and 5, 124 if dice 1, 2 and 4.");
                        if (!int.TryParse(Console.ReadLine(), out int num3))
                            Console.WriteLine("Invalid value entered, try again.");
                        else if (num3 <= 0 || num3 > 12345) Console.WriteLine("Value must be within 1 and 12345");
                        else
                        {
                            switch (num3)
                            {
                                case 1: RollResults[0] = roll.Next(1, 7); Console.WriteLine($"You now have 1: {RollResults[0]} || 2: {RollResults[1]} || 3: {RollResults[2]} || 4: {RollResults[3]} || and 5: {RollResults[4]}!"); break;
                                case 12: break;
                                case 13: break;
                                case 14: break;
                                case 15: break;
                                case 123: break;
                                case 124: break;
                                case 125: break;
                                case 1234: break;
                                case 1235: break;
                                case 12345: break;
                                case 134: break;
                                case 1345: break;
                                case 145: break;
                            }
                        }
                    }
                    else if (num2 == 0)
                    {
                        int cardCounter = 1; foreach (Cards card in activePlayer.cards) if (card.Drawn == true)
                            {
                                Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++;
                            }
                    }
                    else if (num2 > 0 && num2 < 11)
                    {
                        if (activePlayer.cards[num - 1 + activePlayer.cardsPlayed].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                        else { Console.WriteLine($"You played {activePlayer.cards[num - 1 + activePlayer.cardsPlayed].Name}"); activePlayer.cards[num - 1 + activePlayer.cardsPlayed].Action(); activePlayer.cardsPlayed++; }
                    }
                    else if ((num2 < 25 || num2 > 35)) Console.WriteLine("Value must be within 25 and 35");
                    else
                    {
                        switch (num2)
                        {
                            case 25: break;
                            case 26: break;
                            case 27: break;
                            case 28: break;
                            case 29: break;
                            case 30: break;
                            case 31: break;
                            case 32: break;
                            case 33: break;
                            case 34: break;
                            case 35: break;
                        }
                    }
                }
                else if ((num < -1 || num > 10)) Console.WriteLine("Value must be within 0 and 10");


                //TODO: if 99 is entered, prompt user to chose which dice (2-3-4) and reroll
                //      then loop to offer a declaration of move orrr allow for final reroll
                //      must also allow user to play Roll Phase or IA card.
                //      fuck this shit is no cake walk lol
            }
            while (choice != -1);
            foreach (Cards card in activePlayer.cards) if (card.Drawn == true) if (card.Type == 2) card.isPlayable = false; //flips bool isPlayable to False at end of phase
        }*/
    }
    //-----------------------------------------------------------------------------------------------------------------------------------------------
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