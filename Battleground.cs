using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace diceThroneAR
{
    class Battleground
    {
        public static Character currentPlayer;
        public static Character targettedPlayer, swapper, skipper;

        public static int[] RollResults = new int[5];

        public static void printRR(Character activePlayer)
        {
            Console.Write($"You rolled " +
            $"1: {RollResults[0]} {activePlayer.dice[1].symbols[RollResults[0] - 1]} || " +
            $"2: {RollResults[1]} {activePlayer.dice[1].symbols[RollResults[1] - 1]} || " +
            $"3: {RollResults[2]} {activePlayer.dice[1].symbols[RollResults[2] - 1]} || " +
            $"4: {RollResults[3]} {activePlayer.dice[1].symbols[RollResults[3] - 1]} || " +
            $"and 5: {RollResults[4]} {activePlayer.dice[1].symbols[RollResults[4] - 1]}!");
        }

        public static void printEachChar(string word)
        {
            for (int counter = 0; counter < word.Length; counter++)
            { Console.Write(word[counter].ToString()); Thread.Sleep(10); }
        }

        public static void shuffleCards(Character p, int deckSize)
        {
            int i = 0, j = 0, MAXCARDS = deckSize;
            Random sortRandom = new Random();
            for (i = 0; i <= (MAXCARDS - 1); i++)
            {
                j = Convert.ToInt32(sortRandom.Next(0, i + 1));
                var deck = p.cards[i];
                p.cards[i] = p.cards[j];
                p.cards[j] = deck;
            }
        } //Fisher-Yates shuffle algorithm

        public static Character RollForFirst(Character p1, Character p2)
        {
            Character ActivePlayer = p1; int player = 1, highestRoll = 0, whoRolledHighest = 3, breakCounter = 0; string winner = null;
            while (winner == null && breakCounter < 10)
            {
                Console.WriteLine($"\nPlayer {player}: Press enter to roll:"); Console.ReadLine();
                Random roll = new Random(); int rollResult = roll.Next(1, 7);
                Console.WriteLine($"Player {player}: You rolled a {rollResult}!");

                if (highestRoll == 0) { player = 2; highestRoll = rollResult; }
                else
                {
                    if (rollResult > highestRoll) { whoRolledHighest = 2; winner = "Player 2"; }
                    if (highestRoll > rollResult) { whoRolledHighest = 1; winner = "Player 1"; }
                    else
                    { breakCounter++; highestRoll = 0; player = 1; }
                }
            }
            switch (whoRolledHighest)
            {
                case 1: Console.WriteLine($"\nPlayer {p1.team}, {p1.name}, rolls first!\n"); targettedPlayer = p2; break;
                case 2: Console.WriteLine($"\nPlayer {p2.team}, {p2.name}, rolls first!\n"); targettedPlayer = p1; ActivePlayer = p2; break;
                case 3: Console.WriteLine("Well, this is interesting! Player 1 just go first."); break;
            }
            return ActivePlayer;
        }
        //=TODO=>     Update parameters to take List<Characters> and modify TIEBREAKER protocol accordingly...
        //

        public static void MainPhase(Character activePlayer)
        {
            currentPlayer = activePlayer;
            foreach (Cards card in activePlayer.hand) if (card.Type == 1 || card.Type == 4) card.isPlayable = true; //sets the boolean isPlayable to true if either Main Phase or Hero Upgrade card.
            int choice = 99;
            do
            {
                Console.WriteLine($"Player {activePlayer.team}, {activePlayer.name} please enter the number of the card you wish to play.\n" +
                    "Allowed cards in this phase: Main Phase / Hero Upgrade / Instant Action Cards.\n" +
                    "(Press 0 to view your current hand or -1 to advance to the next phase.)\n"); ;
                if (!int.TryParse(Console.ReadLine(), out int num)) Console.WriteLine("Invalid value entered, try again.");
                else if (num < -1 || num > activePlayer.hand.Count()) Console.WriteLine($"Value must be within -1 and {activePlayer.hand.Count()}!");
                else if (num == 0) { int cardCounter = 1; foreach (Cards card in activePlayer.hand) { Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++; } }
                else if (num > 0 && num <= activePlayer.hand.Count())
                {
                    if (activePlayer.hand[num - 1].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                    else { Console.WriteLine($"You played {activePlayer.hand[num - 1].Name}\n"); activePlayer.hand[num - 1].Action(); activePlayer.hand.RemoveAt(num - 1); activePlayer.cardsPlayed++; }
                }
                else if (num == -1) choice = -1;
            }
            while (choice != -1); //TODO: Modify to only allow 1-(number of cards in hand) then display that number as the range of acceptible inputs.
            foreach (Cards card in activePlayer.hand) if (card.Type == 1 || card.Type == 4) card.isPlayable = false; //flips bool isPlayable to False at end of phase
        }
        //
        // MainPhase(Character activePlayer) allows for a Main Phase, Hero Upgrade, or Instant Action card and certain Status Effects to be played.
        //
        public static Character OffensiveRollPhase(Character activePlayer)
        {
            foreach (Cards card in activePlayer.hand) if (card.Type == 2) card.isPlayable = true; //sets the boolean isPlayable to true if Roll Phase card.
            Console.WriteLine("Advancing to the Offensive Roll Phase...\n");
            int choice = -2, choice2 = -2, choice3 = -2, choice4 = -2; Random roll = new Random();

            do //===============================================================================================================================================First roll
            {
                Console.WriteLine($"Player {activePlayer.team}, {activePlayer.name} please enter the number of the card you wish to play.\n" +
                    "Allowed cards in this phase: Roll Phase / Instant Action Cards.\n" +
                    "(Enter 0 to view your current hand or enter 99 to roll your first roll attempt.)\n"); ;
                if (!int.TryParse(Console.ReadLine(), out int num)) Console.WriteLine("Invalid value entered, try again.");
                else if (num == 99)
                {
                    for (int x = 0; x < 5; x++) RollResults[x] = roll.Next(1, 7);

                    do //==========================================================================================================================Leads to second roll
                    {
                        printRR(activePlayer); //TODO: correlate the number rolled with the type of item on the character's di
                        Console.WriteLine("\nPlease enter the number of the move you wish to activate, the number of the card you wish to play, \n0 to view your hand or 99 to reroll some or all of your dice.\n");
                        if (!int.TryParse(Console.ReadLine(), out int num2)) Console.WriteLine("Invalid value entered, try again.");
                        else if (num2 == 99) //allows the player to pick 1-5 di(ce) and reroll
                        {
                            Console.WriteLine("\nEnter which di(ce) you wish to reroll. ie. 1 for the first di / 3 for the third di / 235 for dice 2, 3 and 5.\n");
                            if (!int.TryParse(Console.ReadLine(), out int num3)) Console.WriteLine("Invalid value entered, try again.");
                            else if (num3 <= 0 || num3 > 12345) Console.WriteLine("Value must be within 1 and 12345");
                            else
                            {
                                switch (num3)
                                {
                                    case 1: RollResults[0] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 12: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 13: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 14: RollResults[0] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 15: RollResults[0] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 123: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 124: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 125: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 1234: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 1235: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 1245: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 12345: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 134: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 1345: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 145: RollResults[0] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 2: RollResults[1] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 23: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 24: RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 25: RollResults[1] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 234: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 235: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 245: RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 2345: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 3: RollResults[2] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 34: RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 35: RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 345: RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 4: RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 45: RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                    case 5: RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                }
                                do //==========================================================================================================================Leads to final roll
                                {
                                    Console.WriteLine("\nPlease enter the number of the move you wish to activate, the number of the card you wish to play, \n0 to view your hand or 99 to reroll some or all of your dice.\n");
                                    if (!int.TryParse(Console.ReadLine(), out int num4)) Console.WriteLine("Invalid value entered, try again.");
                                    else if (num4 == 99) //allows the player to pick 1-5 di(ce) and reroll
                                    {
                                        Console.WriteLine("\nEnter which di(ce) you wish to reroll. ie. 1 for the first di / 3 for the third di / 235 for dice 2, 3 and 5.\n");
                                        if (!int.TryParse(Console.ReadLine(), out int num5)) Console.WriteLine("Invalid value entered, try again.");
                                        else if (num5 <= 0 || num5 > 12345) Console.WriteLine("Value must be within 1 and 12345");
                                        else
                                        {
                                            switch (num5)
                                            {
                                                case 1: RollResults[0] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 12: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 13: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 14: RollResults[0] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 15: RollResults[0] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 123: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 124: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 125: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 1234: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 1235: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 1245: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 12345: RollResults[0] = roll.Next(1, 7); RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 134: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 1345: RollResults[0] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 145: RollResults[0] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 2: RollResults[1] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 23: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 24: RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 25: RollResults[1] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 234: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 235: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 245: RollResults[1] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 2345: RollResults[1] = roll.Next(1, 7); RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 3: RollResults[2] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 34: RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 35: RollResults[2] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 345: RollResults[2] = roll.Next(1, 7); RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 4: RollResults[3] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 45: RollResults[3] = roll.Next(1, 7); RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                                case 5: RollResults[4] = roll.Next(1, 7); printRR(activePlayer); break;
                                            }

                                            do //==========================================================================================================================Leads to SOGOTP moment
                                            {
                                                Console.WriteLine("\nPlease enter the number of the move you wish to activate, the number of the card you wish to play, \n0 to view your hand or -1 if you cannot activate a move.\n");
                                                if (!int.TryParse(Console.ReadLine(), out int num6)) Console.WriteLine("Invalid value entered, try again.");
                                                else if (num6 == 0) { int cardCounter = 1; foreach (Cards card in activePlayer.hand) { Console.Write(cardCounter + ": "); card.ShowDetails(); cardCounter++; } }
                                                else if (num6 > 0 && num6 <= activePlayer.hand.Count())
                                                {
                                                    if (activePlayer.hand[num6 - 1].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                                                    else { Console.WriteLine($"You played {activePlayer.hand[num6 - 1].Name}\n"); activePlayer.hand[num6 - 1].Action(); activePlayer.hand.RemoveAt(num6 - 1); activePlayer.cardsPlayed++; }
                                                }
                                                else if (num6 == -1) { choice4 = -1; choice3 = -1; choice2 = -1; choice = -1; } //need to find a way to bypass Targetting/Defensive Phase and go straight to Main Phase 2
                                                else if (num6 < -1 || (activePlayer.hand.Count() < num6 && num6 < 25) || num6 > 35) Console.WriteLine($"Value must be within -1 and {activePlayer.hand.Count()} or 25 and 35!");
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
                                    else if (num4 > 0 && num4 <= activePlayer.hand.Count())
                                    {
                                        if (activePlayer.hand[num4 - 1].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                                        else { Console.WriteLine($"You played {activePlayer.hand[num4 - 1].Name}\n"); activePlayer.hand[num4 - 1].Action(); activePlayer.hand.RemoveAt(num4 - 1); activePlayer.cardsPlayed++; }
                                    }
                                    else if (num4 < 0 || (activePlayer.hand.Count() < num4 && num4 < 25) || num4 > 35) Console.WriteLine($"Value must be within 0 and {activePlayer.hand.Count()} or 25 and 35 or 99!");
                                    else if (num4 == -1) { choice3 = -1; choice2 = -1; choice = -1; } //In the event that a player only has two rolls due to losing a roll turn...
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
                        else if (num2 > 0 && num2 <= activePlayer.hand.Count())
                        {
                            if (activePlayer.hand[num2 - 1].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                            else { Console.WriteLine($"You played {activePlayer.hand[num2 - 1].Name}\n"); activePlayer.hand[num2 - 1].Action(); activePlayer.hand.RemoveAt(num2 - 1); activePlayer.cardsPlayed++; }
                        }
                        else if (num2 < 0 || (25 < num2 && num2 > activePlayer.hand.Count()) || num2 > 35) Console.WriteLine($"Value must be within 0 and {activePlayer.hand.Count()} or 25 and 35 or 99!");
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
                else if (num > 0 && num <= activePlayer.hand.Count())
                {
                    if (activePlayer.hand[num - 1].isPlayable != true) Console.WriteLine("This card is not currently playable.");
                    else { Console.WriteLine($"You played {activePlayer.hand[num - 1].Name}\n"); activePlayer.hand[num - 1].Action(); activePlayer.hand.RemoveAt(num - 1); activePlayer.cardsPlayed++; }
                }
                else if (num < 0 || num > activePlayer.hand.Count()) Console.WriteLine($"Value must be within 0 and {activePlayer.hand.Count()} or 99!");
                else if (num == -1) { Console.WriteLine("Advancing to next phase..."); choice = -1; } //<--shouldnt allow player to skip Offensive Roll Phase UNLESS MAYBE UNDER A KNOCKDOWN CONDITION??
            }
            while (choice != -1); //==================================================================================================================================Leads to first roll
            Console.WriteLine("\nAdvancing to the Targetting Roll phase...\n");
            return targettedPlayer;
        }
        //
        // OffensiveRollPhase(Character activePlayer) allows for a Roll Phase or Instant Action card and certain Status Effects to be played.
        //
        public static Character TargetingRollPhase(Character activePlayer)
        {

            //ADD CODE FOR TARGETTING PROTOCOL
            //Roll 1 die (This die may be manipulated with cards, unless the Attack is an Ultimate Ability.
            //Determine the Defender who will be receiving the damage based on the result of your die roll.
            // 1 or 2 - Target the opponent on your left
            // 3 or 4 - Target the opponent on your right
            // 5 - Your opponents choose which of them you target
            // 6 - Choose either opponent as your target
            if (GameFlow.numberOfPlayers == 2) return targettedPlayer;
            else
            {
                //add code for KOTH and 2v2 GAMES
                Console.WriteLine($"Your targetted player is {targettedPlayer.name}!\n");
                return targettedPlayer;
            }
        }
        //
        // OffensiveRollPhase(Character activePlayer) allows for a Roll Phase or Instant Action card and certain Status Effects to be played.
        //
        public static void DefensiveRollPhase(Character targettedPlayer)
        {
            //ADD CODE FOR players that must chose between 2 Defensive Abilities
            //ADD CODE TO ACTIVATE WHERE Moves.type == 7
            int defensiveResult;
            if (targettedPlayer.name == "Shadow Thief")
                //run choosing protocol 
                Console.WriteLine("TODO: Create Choosing Protocol");
            else
                defensiveResult = targettedPlayer.DefenseRoll(targettedPlayer); //going to change to action of that card instead.

            //run damageMath() to modify targettedPlayer's and activePlayer's HEALTH
        }
        //
        // DefensiveRollPhase(Character targettedPlayer) allows for Roll Phase or Instant Action cards and certain Status Effects to be played.
        // This phase **if the incoming damage is not Undefendable or Ultimate** allows the player to call the action() of their Defensive Ability
        // or (if Shadow Thief) first allows to choose among their two Defensive Abilities. Then EXECUTES AND APPLIES DAMAGE MATH.
        //
        public static void DiscardPhase(Character activePlayer)
        {
            Console.WriteLine("Advancing to the Discard Phase...");
            Console.WriteLine("You do not have more than 6 cards in your hand, press ENTER to end your turn.\n");
            swapper = currentPlayer; currentPlayer = targettedPlayer; targettedPlayer = swapper;
            Console.ReadKey();
        }
        //
        // DiscardPhase(Character activePlayer) checks to see if hand is greater than 6.  If so, force player to discard until less than or equal.
        // Then next player in rotation is set to currentPlayer.
        //
        public static void UpkeepPhase(Character activePlayer)
        {
            Console.WriteLine("Entering the Upkeep Phase:\nResolve any actions you must regarding your Passive or in-play Status Effects then press ENTER.");
            Console.ReadKey();
        }
        //
        // UpkeepPhase(Character activePlayer) allows for player to utilize any Passive abilities and resolve Status Effect requirements.
        //
        public static void IncomePhase(Character activePlayer)
        {
            Console.WriteLine("Add 1 CP to your counter and draw 1 Card! Press ENTER.\n");
            activePlayer.cP += 1; activePlayer.cards[activePlayer.nextCardtoDraw].ShowDetails();
            activePlayer.cards[activePlayer.nextCardtoDraw].Drawn = true; activePlayer.hand.Add(activePlayer.cards[activePlayer.nextCardtoDraw]);
            activePlayer.nextCardtoDraw += 1;
            Console.ReadKey();
        }
        //
        // IncomePhase(Character activePlayer) grants the active player 1 CP and 1 Card.
        //
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
        public Cards(string name, string desc)
        {
            this.Name = name.ToUpper(); this.Desc = desc;
        }
        public Cards(int cpcost, string name, string desc)
        {
            this.CPCost = cpcost; this.Name = name.ToUpper(); this.Desc = desc;
        }
        public virtual void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + "\nDescription: " + Desc);
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
    class PassiveAbility : Cards
    {
        new public string Type = "Passive Ability"; //5

        public PassiveAbility(string name, string desc) : base(name, desc)
        { base.Type = 5; }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " || Type: " + Type + " || Description: \n" + Desc + "\n");
        }
    }
    class OffensiveAbility : Cards
    {
        new public string Type = "Offensive Ability"; //6

        public OffensiveAbility(string name, string desc) : base(name, desc)
        { base.Type = 6; }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " || Type: " + Type + " || Description: \n" + Desc + "\n");
        }
    }
    class DefensiveAbility : Cards
    {
        new public string Type = "Defensive Ability"; //7

        public DefensiveAbility(string name, string desc) : base(name, desc)
        { base.Type = 7; }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " || Type: " + Type + " || Description: \n" + Desc + "\n");

        }
    }
}

/*
 * 
 *
 *
 *1] Making ROLL REQUIREMENTS to ACTIVATE MOVE --->

If RollResults 
3 between 1-3
or 4 between 1-3
or 5 between 1-3

1111
11111
11112
11113
2222
22221
22222
22223
3333
33331
33332
33333

2]

IF THERE IS NO MOVE TO BE PLAYED AT THE END OF THE OFFENSIVE PHASE

we must ---

SKIP the TARGETTING ROLL PHASE AND THE DEFENSIVE ROLL PHASE!

if there is an UNDEFENDABLE MOVE PLAYED AT THE END OF THE OFFENSIVE PHASE

we must ---

SKIP THE DEFENSIVE ROLL PHASE!

===> turn OffensiveRollPhase into a Character function and return a 
SkipTargettingPhase, SkipDefensivePhase, and SkipTargettingAndDefensivePhase character
that would negate either or both phases <=============================================

3]

Going to have to redo the CP deduction process, but then
compare the Roll Results to the Roll Requirements to switch
if a move is Playable =====================================

4]

Apply Damage Math --- deduction and strike back plus any applicable Status Effects

/\/\/
RANDOM THOUGHT == how to write the most efficient code in any language?
/\/\/

---------old Pseudo----------
        public static Character rollForFirst(Character p1, Character p2)
        {
            int player = 1, highestRoll = 0, whoRolledHighest = 0;
            while (GameFlow.numberOfPlayers != 0)
            {
                Console.WriteLine("Let's roll to see who goes first!");
                Console.WriteLine($"\nPlayer {player}: Press enter to roll:");
                Console.ReadLine();
                Random roll = new Random(); int rollResult = roll.Next(1, 7);
                Console.WriteLine($"Player {player}: You rolled a {rollResult}!");
                if (rollResult > highestRoll) { highestRoll = rollResult; whoRolledHighest = player; };
                player++; GameFlow.numberOfPlayers--;
            }
            switch (whoRolledHighest)
            {
                case 1: p1.WinFirstRoll = true; break;
                case 2: p2.WinFirstRoll = true; break;
            }
            if (p1.WinFirstRoll == true) { Console.WriteLine($"\nPlayer {p1.team}, {p1.name}, rolls first!\n"); targettedPlayer = p2; return p1; }
            else { Console.WriteLine($"\nPlayer {p2.team}, {p2.name}, rolls first!\n"); targettedPlayer = p1; return p2; }
        } //TODO:
          //Update parameters to take List<Characters> and add TIEBREAKER protocol


basically it needs to log who rolls the highest...

TIEBREAKER Protocol


int player = 1, highestRoll = 0, whoRolledHighest = 0;
while (winner == null)


Deal --< function

Adds to damage math


** need to correct, if unable to do a move, we must 
   skip the targetting/defensive roll phase and move straight to Main Phase II. **
											
== WRITING DEFENSIVE ROLL PROTOCOL??
==================================================
== Simply Play Defensive Ability Move's ACTION ===
==================================================


{ "Countermeasures","7","DEFENSIVE ROLL 4 DICE | 
   For every 2 [SABER], deal 1 dmg.  
   Prevent 1 x [FLAG] dmg.
   Gain 1 x [MEDAL] Tactical Advantage."} }, //Tactician

public static int DefensiveRoll()
	int[] RollResults = new int[3];
	Console.WriteLine("Press Enter to Roll your Defensive Roll!");
	Console.ReadKey();
	for (int x = 0; x < 3; x++) RollResults[x] = roll.Next(1, 7);
	Console.Write($"You rolled " +
            $"1: {RollResults[0]} {activePlayer.dice[1].symbols[RollResults[0]-1]} || " +
            $"2: {RollResults[1]} {activePlayer.dice[1].symbols[RollResults[1] - 1]} || " +
            $"3: {RollResults[2]} {activePlayer.dice[1].symbols[RollResults[2] - 1]} || " +
	//COMPARE AGAINST ROLL REQUIREMENTS FOR DEFENSIVE ROLL
	//RESOLVE ACTIONS (COUNTER ATTACK DMG/STATUS EFFECTS
	//RETURN A VALUE

//rewriting printRR(Character activePlayer) to be able to display the results of 1-5 dice versus always displaying 5
int num = 1;

  NEW LINE CINEMA
  PARAMOUNT
  
  Just be able to maintain your 

1) Hololens (Futuristic, So Simplistic) /\/ 
2) PC (Have it Follow me around) /\/ 
3) Dell Work Comp (Convenient) /\/

  1) iPad (Not As Convenient) /\/ 2) iPhone (Convenient) /\/ 3) AppleReality (Futurstic, So Simplistic)
    1) Accessories (Monitors, Wireless Keyboards/Mice/Airpods, Flashlights, PowerBankz)
      1) Ok, gonna call it, 4:23AM ... see u tomorrow.

---------even older pseudo-----------

ok, so now we have 2 characters, their SEs, Cards, Health, CPs, introQuip AND we know who goes first

Players 1 and 2 will need to draw 5 cards
Then the player who winFirstRoll = true
begins Upkeep Phase -> (SKIPS Income Phase) -> Main Phase I ->
Offesnvie Roll Phase -> Targeting Roll Phase -> Defensive Roll Phase
-> Main Phase II -> Discard Phase
----> next player goes

to make it POLYFUCKINGMORPHIC
       TODO :: make the gameModeSelect less hard coded
 NEXT UP :: shuffle decks // each player draws 5 cards

 Sun 20 Nov 2022 1:01 AM UTC
     TeamViewer-ID: 1 278 803 792
	IP Address: 2600:1700:840:ba20::/64

I need to write a program that helps people do things
Learn_It

M1X6TL

 * 
 */