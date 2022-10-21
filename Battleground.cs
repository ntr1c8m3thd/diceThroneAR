using System; using System.Collections.Generic; using System.Linq;
using System.Text; using System.Threading.Tasks; using System.Threading;

namespace diceThroneAR
{
    //BATTLEGROUND -- Functions that help the GameFlow class.
    class Battleground                                                          
    {
        public static Character currentPlayer;
        public static int[] RollResults = new int[5];
        public static void printRR() { Console.Write($"You rolled 1: {RollResults[0]} || 2: {RollResults[1]} || 3: {RollResults[2]} || 4: {RollResults[3]} || and 5: {RollResults[4]}!"); }
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

        //playMainPhaseHand(Character activePlayer) allows for a Main Phase or Hero Upgrade card and certain Status Effects to be played.
        public static void MainPhase(Character activePlayer)
        {
            currentPlayer = activePlayer;
            foreach (Cards card in activePlayer.hand) if (card.Type == 1 || card.Type == 4) card.isPlayable = true; //sets the boolean isPlayable to true if either Main Phase or Hero Upgrade card.
            int choice = 99;
            do
            {
                int CardsInHand = activePlayer.hand.Count();
                Console.WriteLine($"Player {activePlayer.team}, {activePlayer.name} please enter the number of the card you wish to play.\n" +
                    "Allowed cards in this phase: Main Phase / Hero Upgrade / Instant Action Cards.\n" +
                    "(Press 0 to view your current hand or -1 to advance to the next phase.)\n"); ;
                if (!int.TryParse(Console.ReadLine(), out int num)) Console.WriteLine("Invalid value entered, try again.");
                else if (num < -1 || num > CardsInHand) Console.WriteLine($"Value must be within -1 and {CardsInHand}!");
                else if (num == 0) { int cardCounter = 1; foreach (Cards card in activePlayer.hand) { Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++; } }
                else if (num > 0 && num <= CardsInHand)
                    {
                        if (activePlayer.hand[num - 1].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                        else {Console.WriteLine($"You played {activePlayer.hand[num - 1].Name}\n"); activePlayer.hand[num - 1].Action(); activePlayer.hand.RemoveAt(num - 1); activePlayer.cardsPlayed++; }
                    }
                else if (num == -1) { Console.WriteLine("Advancing to the Offensive Roll phase...\n"); choice = -1; }
            }
            while (choice != -1); //TODO: Modify to only allow 1-(number of cards in hand) then display that number as the range of acceptible inputs.
            foreach (Cards card in activePlayer.hand) if (card.Type == 1 || card.Type == 4) card.isPlayable = false; //flips bool isPlayable to False at end of phase
        }

        public static void OffensiveRollPhase(Character activePlayer)
        {
            foreach (Cards card in activePlayer.hand) if (card.Type == 2) card.isPlayable = true; //sets the boolean isPlayable to true if Roll Phase card.
            int choice = -2, choice2 = -2, choice3 = -2, choice4 = -2; Random roll = new Random();

            do //==================================================================================================================================Leads to first roll
            {
                int CardsInHand = activePlayer.hand.Count();
                Console.WriteLine($"Player {activePlayer.team}, {activePlayer.name} please enter the number of the card you wish to play.\n" +
                    "Allowed cards in this phase: Roll Phase / Instant Action Cards.\n" +
                    "(Enter 0 to view your current hand or enter 99 to roll your first roll attempt.)\n"); ;
                if (!int.TryParse(Console.ReadLine(), out int num)) Console.WriteLine("Invalid value entered, try again.");
                else if (num == 99)
                {
                    for (int x = 0; x < 5; x++) RollResults[x] = roll.Next(1, 7);

                    do //==========================================================================================================================Leads to second roll
                    {
                        printRR(); //TODO: correlate the number rolled with the type of item on the character's di
                        Console.WriteLine("\nPlease enter the number of the move you wish to activate, the number of the card you wish to play, \n0 to view your hand or 99 to reroll some or all of your dice.");
                        if (!int.TryParse(Console.ReadLine(), out int num2)) Console.WriteLine("Invalid value entered, try again.");
                        else if (num2 == 99) //allows the player to pick 1-5 di(ce) and reroll
                        {
                            Console.WriteLine("\nEnter which di(ce) you wish to reroll. ie. 1 for the first di / 3 for the third di / 235 for dice 2, 3 and 5.");
                            if (!int.TryParse(Console.ReadLine(), out int num3)) Console.WriteLine("Invalid value entered, try again.");
                            else if (num3 <= 0 || num3 > 12345) Console.WriteLine("Value must be within 1 and 12345");
                            else
                            {
                                switch (num3)
                                {
                                    case 1: RollResults[0] = roll.Next(1, 7); printRR(); break;
                                    case 12: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); printRR(); break;
                                    case 13: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(); break;
                                    case 14: RollResults[0] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                    case 15: RollResults[0] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 123: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(); break;
                                    case 124: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                    case 125: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 1234: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                    case 1235: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 12345: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 134: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                    case 1345: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 145: RollResults[0] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 2: RollResults[1] = roll.Next(1, 7); printRR(); break;
                                    case 23: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(); break;
                                    case 24: RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                    case 25: RollResults[1] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 234: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                    case 235: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 2345: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 3: RollResults[2] = roll.Next(1, 7); printRR(); break;
                                    case 34: RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                    case 35: RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 345: RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 4: RollResults[3] = roll.Next(1, 7); printRR(); break;
                                    case 45: RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                    case 5: RollResults[4] = roll.Next(1, 7); printRR(); break;
                                }
                                do //==========================================================================================================================Leads to final roll
                                {
                                    Console.WriteLine("\nPlease enter the number of the move you wish to activate, the number of the card you wish to play, \n0 to view your hand or 99 to reroll some or all of your dice.");
                                    if (!int.TryParse(Console.ReadLine(), out int num4)) Console.WriteLine("Invalid value entered, try again.");
                                    else if (num4 == 99) //allows the player to pick 1-5 di(ce) and reroll
                                    {
                                        Console.WriteLine("\nEnter which di(ce) you wish to reroll. ie. 1 for the first di / 3 for the third di / 235 for dice 2, 3 and 5.");
                                        if (!int.TryParse(Console.ReadLine(), out int num5)) Console.WriteLine("Invalid value entered, try again.");
                                        else if (num5 <= 0 || num5 > 12345) Console.WriteLine("Value must be within 1 and 12345");
                                        else
                                        {
                                            switch (num5)
                                            {
                                                case 1: RollResults[0] = roll.Next(1, 7); printRR(); break;
                                                case 12: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); printRR(); break;
                                                case 13: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(); break;
                                                case 14: RollResults[0] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                                case 15: RollResults[0] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 123: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(); break;
                                                case 124: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                                case 125: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 1234: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                                case 1235: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 12345: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 134: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                                case 1345: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 145: RollResults[0] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 2: RollResults[1] = roll.Next(1, 7); printRR(); break;
                                                case 23: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(); break;
                                                case 24: RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                                case 25: RollResults[1] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 234: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                                case 235: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 2345: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 3: RollResults[2] = roll.Next(1, 7); printRR(); break;
                                                case 34: RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(); break;
                                                case 35: RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 345: RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 4: RollResults[3] = roll.Next(1, 7); printRR(); break;
                                                case 45: RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(); break;
                                                case 5: RollResults[4] = roll.Next(1, 7); printRR(); break;
                                            }

                                            do //==========================================================================================================================Leads to SOGOTP moment
                                            {
                                                Console.WriteLine("\nPlease enter the number of the move you wish to activate, the number of the card you wish to play, \n0 to view your hand or -1 if you cannot activate a move.");
                                                if (!int.TryParse(Console.ReadLine(), out int num6)) Console.WriteLine("Invalid value entered, try again.");
                                                else if (num6 == 0) { int cardCounter = 1; foreach (Cards card in activePlayer.hand) { Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++; } }
                                                else if (num6 > 0 && num6 <= CardsInHand)
                                                {
                                                    if (activePlayer.hand[num6 - 1].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                                                    else { Console.WriteLine($"You played {activePlayer.hand[num6 - 1].Name}\n"); activePlayer.hand[num6 - 1].Action(); activePlayer.hand.RemoveAt(num6 - 1); activePlayer.cardsPlayed++; }
                                                }
                                                else if (num6 == -1) { choice4 = -1;  choice3 = -1; choice2 = -1; choice = -1; } //need to find a way to bypass Targetting/Defensive Phase and go straight to Main Phase 2
                                                else if (num6 < -1 || (25 < num6 && num6 > CardsInHand) || num6 > 35) Console.WriteLine($"Value must be within -1 and {CardsInHand} or 25 and 35!");
                                                else
                                                {
                                                    switch (num6)
                                                    {
                                                        case 25: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                        case 26: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                        case 27: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                        case 28: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                        case 29: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                        case 30: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                        case 31: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                        case 32: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                        case 33: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                        case 34: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                        case 35: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; break;
                                                    }
                                                }
                                            }
                                            while (choice4 != -1); //==========================================================================================================================Leads to SOGOTP moment
                                        }
                                    }
                                    else if (num4 == 0) { int cardCounter = 1; foreach (Cards card in activePlayer.hand) { Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++; } }
                                    else if (num4 > 0 && num4 <= CardsInHand)
                                    {
                                        if (activePlayer.hand[num4 - 1].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                                        else { Console.WriteLine($"You played {activePlayer.hand[num4 - 1].Name}\n"); activePlayer.hand[num4 - 1].Action(); activePlayer.hand.RemoveAt(num4 - 1); activePlayer.cardsPlayed++; }
                                    }
                                    else if (num4 == -1) { choice3 = -1; choice2 = -1; choice = -1; } //need a way to bypass Targetting Phase and Defensive Phase and go straight to Main Phase
                                    else if (num4 < -1 || (25 < num4 && num4 > CardsInHand) || num4 > 35) Console.WriteLine($"Value must be within -1 and {CardsInHand} or 25 and 35 or 99!");
                                    else
                                    {
                                        switch (num4)
                                        {
                                            case 25: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                            case 26: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                            case 27: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                            case 28: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                            case 29: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                            case 30: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                            case 31: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                            case 32: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                            case 33: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                            case 34: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                            case 35: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice3 = -1; choice2 = -1; choice = -1; break;
                                        }
                                    }
                                }
                                while (choice3 != -1); //==========================================================================================================================Leads to final roll
                            }
                        }
                        else if (num2 == 0)
                        { int cardCounter = 1; foreach (Cards card in activePlayer.hand) { Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++; } }
                        else if (num2 > 0 && num2 <= CardsInHand)
                        {
                            if (activePlayer.hand[num2 - 1].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                            else { Console.WriteLine($"You played {activePlayer.hand[num2 - 1].Name}\n"); activePlayer.hand[num2 - 1].Action(); activePlayer.hand.RemoveAt(num2 - 1); activePlayer.cardsPlayed++; }
                        }
                        else if (num2 < -1 || (25 < num2 && num2 > CardsInHand) || num2 > 35) Console.WriteLine($"Value must be within -1 and {CardsInHand} or 25 and 35 or 99!");
                        else
                        {
                            switch (num2)
                            {
                                case 25: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                                case 26: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                                case 27: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                                case 28: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                                case 29: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                                case 30: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                                case 31: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                                case 32: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                                case 33: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                                case 34: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                                case 35: /* INSERT FUNCTION TO ACTIVATE MOVE */ choice2 = -1; choice = -1; break;
                            }
                        }
                    }
                    while (choice2 != -1); //==========================================================================================================================Leads to second roll
                }
                else if (num == 0)
                {
                    int cardCounter = 1; foreach (Cards card in activePlayer.hand) { Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++; }
                }
                else if (num > 0 && num <= CardsInHand)
                {
                    if (activePlayer.hand[num - 1].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                    else { Console.WriteLine($"You played {activePlayer.hand[num - 1].Name}\n"); activePlayer.hand[num - 1].Action(); activePlayer.hand.RemoveAt(num - 1); activePlayer.cardsPlayed++; }
                }
                else if ((num < -1 || num > 10)) Console.WriteLine($"Value must be within -1 and {CardsInHand} or 99!");
                else if (num == -1) { Console.WriteLine("Advancing to next phase..."); choice = -1; } //<--shouldnt allow player to skip Offensive Roll Phase UNLESS MAYBE UNDER A KNOCKDOWN CONDITION??
            }
            while (choice != -1); //==================================================================================================================================Leads to first roll

            foreach (Cards card in activePlayer.hand) if (card.Type == 2) card.isPlayable = false; //flips bool isPlayable to False at end of phase
            Console.WriteLine("Advancing to the Targetting Roll phase...\n");
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