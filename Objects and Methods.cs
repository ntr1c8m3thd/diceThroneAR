﻿using System;
using System.Collections;
using System.Collections.Generic;
using static diceThroneAR.Battleground;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace diceThroneAR
{
    class GameFlow
    {
        public static int numberOfPlayers = 1; public static Character player2;

        public static int characterSelect() //characterSelect() allows Player 1 to choose his character from a list of available characters
        {
            Console.WriteLine("Welcome to DICE THRONE!"); Thread.Sleep(500);
            Console.WriteLine("Character List:"); Thread.Sleep(500);
            ArrayList characters = new() { "[1] Artificer", "[2] Barbarian", "[3] Cursed Pirate", "[4] Gunslinger",
                "[5] Huntress", "[6] Monk", "[7] Moon Elf", "[8] Ninja" };
            ArrayList characters2 = new() { "[9] Paladin", "[10] Pyromancer", "[11] Samurai", "[12] Seraph",
                "[13] Shadow Thief", "[14] Tactician", "[15] Treeant", "[16] Vampire Lord" };
            characters.AddRange(characters2); int count = 1; int count2 = 0;
            foreach (string name in characters) //--------------------------------------------------Cleanly displays 4 characters per row
            {
                if (count != characters.Count)
                {
                    if (count2 != 3)
                    { /*printEachChar()*/Console.Write(name); Console.Write(", "); count++; count2++; Thread.Sleep(100); }
                    else { Console.Write(name + "\n"); count2 = 0; Thread.Sleep(100); }
                }
                else Console.Write(name); Thread.Sleep(100);
            }
            Thread.Sleep(555); int charSelect = -1;
            while (charSelect < 0)
            {
                Console.WriteLine("\nEnter your character's number to continue: ");
                if (!int.TryParse(Console.ReadLine(), out int num)) Console.WriteLine("Invalid value entered, try again.");
                else if ((num < 1 || num > 16)) Console.WriteLine("Value must be within 1 and 16");
                else { Console.WriteLine($"You chose {characters[num - 1]}"); charSelect = num - 1; }
            }
            Thread.Sleep(555); return charSelect;
        }

        public static void gameModeSelect() //gameModeSelect() prompts Player 1 to choose from 3 gamemode styles, 
                                            //generating a match based off of the desired opponents
        {
            ArrayList gameMode = new() { "[1] King of the Hill", "[2] 1v1", "[3] 2v2" };
            int gameModeSelect = -1;
            while (gameModeSelect < 0)
            {
                Console.WriteLine("\nPlease select from the following game styles:\n");
                foreach (string type in gameMode) Console.Write(type + " ");
                Console.WriteLine();
                if (!int.TryParse(Console.ReadLine(), out int num))
                    Console.WriteLine("Invalid value entered, try again.");
                else if ((num < 1 || num > 7)) Console.WriteLine("Please choose by inputting a value between 1 - 3");
                else { Console.WriteLine($"You chose {gameMode[num - 1]}"); gameModeSelect = num; }
            }
            Thread.Sleep(1000);
            ArrayList opponent = new() { "[1] CPU", "[2] Local Friend", "[3] Online Opponent" }; int opponentSelect = -1;
            switch (gameModeSelect)
            {
                case 1: break;
                case 2:
                    numberOfPlayers++;
                    Console.WriteLine("\nPlease select from the following opponents:\n[1] CPU, [2] Local Friend, [3] Online Opponent");
                    if (!int.TryParse(Console.ReadLine(), out int num))
                        Console.WriteLine("Invalid value entered, try again.");
                    else if ((num < 1 || num > 3)) Console.WriteLine("Please choose by inputting a value between 1 - 3");
                    else { Console.WriteLine($"You chose {opponent[num - 1]}"); opponentSelect = num; }

                    printEachChar("\nGenerating a Random CPU opponent...\n");                 //***COME BACK TO FIX!
                    //WILL NEED TO ADD A CASE STATEMENT TO GENERATE WHICH OPPONENT IS SELECTED (opponentSelect integer value)

                    Random cpu = new Random(); /*int cpuRand = cpu.Next(0, 13);*/ int cpuRand = 13;                                                      //<========== HARD SET RANDOM CPU AS SHADOW THIEF ========== 
                    player2 = new Character("2", CharacterInfo.names[cpuRand], CharacterInfo.introQuips[cpuRand]);
                    player2.statusEffects.AddRange(CharacterInfo.generateStatusEffects(cpuRand));
                    player2.cards.AddRange(CharacterInfo.generateDeck(cpuRand));
                    player2.moves.AddRange(CharacterInfo.generateMoves(cpuRand)); player2.IntroCharacter();
                    player2.dice.AddRange(CharacterInfo.generateDice(cpuRand));
                    break;
                case 3: break; //case 4: break; //case 5: break; //case 6: break; //case 7: break;
            }
        }
        //=TODO=>    Needs KOTH (3 player game) and 2v2 additionally (**variant game modes will be added on in the future**)
        //

        public static void gameMatchMaking(Character p1, Character p2) //gameMatchMaking(List<Characters>) matchmaking system
                                                                       //for 2+ characters
        {
            shuffleCards(p1, p1.cards.Count); shuffleCards(p2, p2.cards.Count);
            Console.WriteLine("°º¤ø,¸¸,ø¤º°`°º¤ø,¸,ø¤°º¤ø,¸¸,ø¤º°`°º¤ø,¸,ø¤º°`°º¤ø,¸,ø¤°º¤ø,¸¸,ø¤º°`°º¤ø,¸¸,ø¤º°`\n");
            Console.WriteLine("Press enter to shuffle the cards and draw your starting hand (4 cards)!\n"); Console.ReadKey();
            Console.WriteLine("Here are your drawn cards: " + p1.name.ToString() + "\n");
            int c = 0; while (c < 4) { Console.Write(c + 1 + ": "); p1.cards[c].ShowDetails(); p1.cards[c].Drawn = true; p1.hand.Add(p1.cards[c]); c++; }
            //Console.WriteLine("Here are player 2's drawn cards: " + p2.name.ToString() + "\n");
            int d = 0; while (d < 4) { /*Console.Write(d + 1 + ": "); p2.cards[d].ShowDetails();*/ p2.cards[d].Drawn = true; p2.hand.Add(p2.cards[d]); d++; }
            Console.WriteLine("Press enter to Roll for First.");
            Console.ReadKey();

            Battleground.MainPhase(RollForFirst(p1, p2));
            Battleground.OffensiveRollPhase(Battleground.currentPlayer);
            Battleground.DefensiveRollPhase(Battleground.TargetingRollPhase(Battleground.currentPlayer));
            Battleground.MainPhase(currentPlayer);
            Battleground.DiscardPhase(currentPlayer);

            while (p1.hP >= 0 || p2.hP >= 0)
            {
                Console.WriteLine($"Beginning {currentPlayer.name}'s turn!\n");
                Console.WriteLine($"Current Health = {currentPlayer.hP} || CP = {currentPlayer.cP}!\n");
                UpkeepPhase(currentPlayer);
                IncomePhase(currentPlayer);
                MainPhase(currentPlayer);
                OffensiveRollPhase(currentPlayer);
                DefensiveRollPhase(TargetingRollPhase(currentPlayer));
                MainPhase(currentPlayer);
                DiscardPhase(currentPlayer);
            }
            Console.ReadKey();
        }
        //=TODO=>    Needs to take a List<Character>
        //
    }

    class Character
    {
        public bool WinFirstRoll { get; set; } = false;
        public int cP = 2, hP = 50, cardsPlayed = 0, nextCardtoDraw = 5;
        public string team, name;
        public string introQuip;
        //public string[] diceSymbols;
        public List<StatusEffect> statusEffects = new List<StatusEffect>();
        public List<Cards> cards = new List<Cards>();
        public List<Cards> hand = new List<Cards>();
        public List<Cards> moves = new List<Cards>();
        public List<Dice> dice = new List<Dice>();
        public Character(string t, string n, string iQ)
        {
            this.team = t; this.name = n; this.introQuip = iQ;
        }
        public void IntroCharacter()
        {
            Console.WriteLine($"\n\"{introQuip}\"");
            Console.WriteLine($"{name} - \"I currently have {cP} Combat Points and {hP} Health!\"");
            int b = 0; Console.Write($"\"These are my status effects!\" = ");
            foreach (StatusEffect a in statusEffects)
            {
                if (b + 1 != statusEffects.Count) { Console.Write($"{statusEffects[b].name}, Quantity ({statusEffects[b].quantity}), Type: {statusEffects[b].status}\n                               = "); b++; }
                else Console.Write($"{statusEffects[b].name}, Quantity ({statusEffects[b].quantity}), Type: {statusEffects[b].status}\n");
            }
            Console.Write($"\n\"I have {moves.Count} moves currently available.\"\n\n");
            int moveCounter = 1;
            foreach (Cards m in moves) { Console.Write(moveCounter + ": "); m.ShowDetails(); moveCounter++; }
            Console.Write($"\"I have {cards.Count} cards total.\"\n\n");
            //int cardCounter = 1;
            //foreach (Cards a in cards) { Console.Write(cardCounter + ": "); a.ShowDetails(); cardCounter++; }
            //int c = 0; while (c < 5) { cards[c].ShowDetails(); c++; //Shows just five out of x amount of cards.}
        }
        public int DefenseRoll(Character targettedPlayer)
        {
            int defensiveDeduction;

            //ACTION OF DEFENSIVE MOVE
            Random roll = new Random(); int[] RollResults = new int[4];
            Console.WriteLine("Press ENTER to Roll your Defensive Roll!");
            Console.ReadKey();
            for (int x = 0; x < 4; x++) RollResults[x] = roll.Next(1, 7);
            Console.Write($"You rolled " +
                    $"1: {RollResults[0]} {targettedPlayer.dice[1].symbols[RollResults[0] - 1]} || " +
                    $"2: {RollResults[1]} {targettedPlayer.dice[1].symbols[RollResults[1] - 1]} || " +
                    $"3: {RollResults[2]} {targettedPlayer.dice[1].symbols[RollResults[2] - 1]} || " +
                    $"and 4: {RollResults[3]} {targettedPlayer.dice[1].symbols[RollResults[3] - 1]}\n\n");
            //COMPARE AGAINST ROLL REQUIREMENTS FOR DEFENSIVE ROLL
            //RESOLVE ACTIONS (COUNTER ATTACK DMG/STATUS EFFECTS
            //RETURN A VALUE
            defensiveDeduction = 2;
            Console.WriteLine("Applying Damage and advancing to Main Phase II...\n");

            return defensiveDeduction;
        }
    }

    class CharacterInfo
    {
        public static string[] names = {
            "Artificer", "Barbarian", "Cursed Pirate", "Gunslinger", "Huntress", "Monk", "Moon Elf", "Ninja",
            "Paladin", "Pyromancer", "Samurai", "Seraph", "Shadow Thief", "Tactician", "Treeant", "Vampire Lord"};
        public static string[] introQuips = {
            "The probability of your demise is written in the schematics.  Let me show you.",
            "Hacking, slashing and mashing may not be elegant, but you know what, it works.",
            "Whatever it takes, th' booty will be mine.",
            "The gun is mightier than all of you.",
            "Two are better than one, in life and in battle.",
            "I am the calm before the storm.",
            "Look deep into the moon, for victory lies there!",
            "If you see me, it's too late.",
            "To win with honor, purity, and righteousness is the only true victory.",
            "I just want to watch the world burn!",
            "I know only honor and shame.  There is no middle ground.",
            "",
            "Entering the shadows is my safe place.  Safe place to shank you.",
            "All I survey, I command.",
            "Woe to you who awakens the forest itself.",
            "Only victory is sweeter than blood."};
        public static string[,,] statusEffects = new string[,,]
        {
            { {"Synth", "8", "Positive"}, {"Nanite", "6", "Positive"}, {"Nanobot (Basic)", "1", "Companion"}, {"Nanobot (Advanced)", "1", "Companion"},
              {"Shock Bot (Basic)", "1", "Companion"}, {"Shock Bot (Advanced)", "1", "Companion"}, {"Heal Bot (Basic)", "1", "Companion"}, {"Heal Bot (Advanced)", "1", "Companion"}, {null, null, null} }, //Artificer
            { {"Concussion", "2", "Negative" }, {"Stun", "1", "Negative"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Barbarian
            { {"Powder Keg", "4", "Negative"}, {"Wither", "4", "Negative"}, {"Parlay", "3", "Negative"}, {"Cursed Doubloon", "6", "Unique"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Cursed Pirate
            { {"Knockdown", "3", "Negative"}, {"Bounty", "3", "Negative"}, {"Evasive","5", "Positive"}, {"Reload","4", "Positive"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Gunslinger
            { {"Nyra","1", "Companion"}, {"Nyra's Bond", "2", "Positive"}, {"Bleed", "5", "Negative"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Huntress
            { {"Chi","10", "Positive"}, {"Evasive", "5", "Positive"}, {"Knockdown", "2", "Negative"}, {"Cleanse", "4", "Positive"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Monk
            { {"Blind","2", "Negative"}, {"Entangle", "2", "Negative"}, {"Targeted", "4", "Negative"}, {"Evasive", "5", "Positive"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Moon Elf
            { {"Delayed Poison","3", "Negative"}, {"Smoke Bomb", "2", "Positive"}, {"Ninjutsu", "5", "Positive"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Ninja
            { {"Retribution","2", "Positive"}, {"Crit", "2", "Positive"}, {"Protect", "2", "Positive"}, {"Accuracy", "2", "Positive"}, {"Blessing Of Divinity", "1", "Unique"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Paladin
            { {"Knockdown","2", "Negative"}, {"Crit", "2", "Positive"}, {"Protect", "2", "Positive"}, {"Accuracy", "2", "Positive"}, {"Blessing Of Divinity", "1", "Unique"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Pyromancer
            { {"Shame","2", "Negative"}, {"Crit", "2", "Positive"}, {"Protect", "2", "Positive"}, {"Accuracy", "2", "Positive"}, {"Blessing Of Divinity", "1", "Unique"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Samurai
            { {"Blinding Light","5", "Negative"}, {"Crit", "2", "Positive"}, {"Protect", "2", "Positive"}, {"Accuracy", "2", "Positive"}, {"Blessing Of Divinity", "1", "Unique"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Seraph
            { {"Poison","6", "Negative"}, {"Crit", "2", "Positive"}, {"Protect", "2", "Positive"}, {"Accuracy", "2", "Positive"}, {"Blessing Of Divinity", "1", "Unique"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Shadow Thief
            { {"Constrict","4", "Negative"}, {"Crit", "2", "Positive"}, {"Protect", "2", "Positive"}, {"Accuracy", "2", "Positive"}, {"Blessing Of Divinity", "1", "Unique"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Tactician
            { {"Barbed Vine","2", "Negative"}, {"Crit", "2", "Positive"}, {"Protect", "2", "Positive"}, {"Accuracy", "2", "Positive"}, {"Blessing Of Divinity", "1", "Unique"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Treeant
            { {"Blood Power","5", "Positive"}, {"Crit", "2", "Positive"}, {"Protect", "2", "Positive"}, {"Accuracy", "2", "Positive"}, {"Blessing Of Divinity", "1", "Unique"}, {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} } //Vampire Lord
        };
        public static string[,,] cards = new string[,,]
        {
            {{"TRANSFERENCE","2","1","1","Transfer 1 status effect token from a chosen player to another chosen player."},
            {"VEGAS BABY!","0","1","0","Roll 1 Die: Gain 1/2 the value as CP (rounded up)."},
            {"WHAT STATUS EFFECTS?","2","1","0","Remove all status effect tokens from a chosen player."},
            {"GET THAT OUTTA HERE!","1","1","1","Remove a status effect token from a chosen player."},
            {"ONE MORE TIME!","1","2","5","A chosen player may perform an additional Roll Attempt of up to five dice during their Offensive Roll Phase."},
            {"TWICE AS WILD!","3","2","2","Change the values of any two dice."},
            {"TRY, TRY AGAIN!","1","2","2","You or a chosen teamate may re-roll up to two dice (can be the same die twice in a row or two different dice)."},
            {"NOT THIS TIME!","1","2","6","A chosen player prevents 6 (Regular) incoming damage."},
            {"SO WILD!","2","2","1","Change the value of any one die."},
            {"SAMSIES!","1","2","1","Change the value of one of your dice to be identical to the value of another one of your dice (that was rolled within the same phase and for the same purpose)."},
            {"HELPING HAND","1","2","1","Select one iof your opponent's dice and force them to re-roll it."},
            {"SIX-IT!","1","2","1","Change the value of one of your dice to a 6."},
            {"BETTER D!","0","2","5","A chosen player may perform an additional Roll Attempt of up to five dice during their Defensive Roll Phase."},
            {"TIP IT!","1","3", "1","Increase or decrease any die by the value of 1 (a value of 1 cannot be decreased and a value of 6 cannot be increased)."},
            {"BUH, BYE!","2","3","1","Remove a status effect token from a chosen player."},
            {"DOUBLE UP!","1","3","2","Draw 2 cards."},
            {"TRIPLE UP!","2","3", "3", "Draw 3 cards."},
            {"GETTING PAID!","1","3","2","Gain 2 CP."} }, //Stock Cards
            {{null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null} }, //Artificer
            {{null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null} }, //Barbarian
            {{null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null} }, //Cursed Pirate
            {{null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null} }, //Gunslinger
            {{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null},
            {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null} }, //Huntress
            {{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null},
            {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null} }, //Monk
            {{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null},
            {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null} }, //Moon Elf
            {{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null},
            {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null} }, //Ninja
            {{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null},
            {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null} }, //Paladin
            {{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null},
            {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null} }, //Pyromancer
            {{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null,null,null},
            {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null} }, //Samurai
            {{"Divine Intervention!","4","1","5","Gain Holy Presence.  Inflict Blinding Light on a chosen player.  A chosen player gains 2 Flight.  A chosen player gains Cleanse."},
            {"To The Skies!","1","1","1","A chosen player gains Flight."},
            {"Smote!","1","2","0","Attack Modifier | Roll 5 Dice:  Add 1 x [BLADE] to the total dmg.  On [ANGELIC PENDANT], inflict Blinding Light."},
            {"Angelic Combat!","1","2","0","Attack Modifier | Roll 1 Di:  On [BLADE], add 3 dmg.  On [WINGS], gain Flight.  On [CROSS], gain Cleanse.  On [ANGELIC PENDANT], gain Holy Presence."},
            {"HOLY BLADE II/Cherubim (adds)","2","4","0","3 [BLADE] Deal 6 dmg | 4 [BLADE] Deal 7 dmg | 5 [BLADE] Deal 8 dmg | 3 [BLADE] 1 [ANGELIC PENDANT]  Gain Flight & Holy Presence."},
            {"HOLY BLADE III/Cherubim II (adds or upgrades)","3","4","0","3 [BLADE] Deal 5 dmg | 4 [BLADE] Deal 7 dmg | 5 [BLADE] Deal 9 dmg | 3 [BLADE] 1 [ANGELIC PENDANT]  Gain Cleanse, Flight, & Holy Presence."},
            {"Purify II","2","4","0","2 [CROSS] 1 [ANGELIC PENDANT] Choose a player:  If that player is an opponent, deal 6 undefendable dmg.  Otherwise, that player Heals 5.  Additionally, you may remove a status effect from that player."},
            {"Glorious II/Take Flight (adds)","2","4","0","3 [BLADE] Gain Flight.  Then deal 7 dmg. | 1 [BLADE] 2 [WINGS] Chosen player gains Flight.  Deal 3 undefendable dmg."},
            {"Holy Smite II/Holy Command (adds)","2","4","0","1 [BLADE] 1 [WINGS] 1 [CROSS] 1 [ANGELIC PENDANT] Roll 4 Dice:  Deal 2 x [BLADE] undefendable dmg.  Gain 1 x [WINGS] Flight.  Gain 1 x [CROSS] Cleanse.  On [ANGELIC PENDANT], inflict Blinding Light. | 3 [BLADE] 1 [CROSS] Heal 1.  Deal 4 undefendable dmg."},
            {"Triumphant II","2","4","0","SMALL STRAIGHT Deal 8 dmg & roll 1 Di:  On [BLADE], add 1 dmg.  On [WINGS], add 2 dmg.  On [CROSS], add 3 dmg.  On [ANGELIC PENDANT], this Attack becomes undefendable."},
            {"Archangel's Will II/Heaven's Blessing (adds)","2","4","0","LARGE STRAIGHT Gain Flight.  Inflict Blinding Light.  Then deal 9 dmg. | 1 [BLADE] 2 [CROSS] Chosen player gains 2 Cleanse & 2 Flight."},
            {"Highest Power II/Divine Visage (adds)","2","4","0","4 [ANGELIC PENDANT] Gain Flight & Holy Presence.  Inflict Blinding Light.  Then deal 10 dmg. | 3 [ANGELIC PENDANT] Gain Holy Presence, 2 Flight, & 2 Cleanse.  Inflict Blinding Light."},
            {"Angelic Mantle II","2","4","0","DEFENSIVE ROLL 1 DI On 1 [BLADE], deal 3 dmg.  On 1 [WINGS], gain Flight.  On 1 [CROSS], prevent 2 dmg.  On 1 [ANGELIC PENDANT], prevent 3 dmg."},
            {"Angelic Mantle III","2","4","0","DEFENSIVE ROLL 1 DI On 1 [BLADE], deal 3 dmg.  On 1 [WINGS], gain Flight.  On 1 [CROSS], prevent 2 dmg.  On 1 [ANGELIC PENDANT], prevent 4 dmg.  You may choose to re-roll this die 1 time."},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null}}, //Seraph
            {{"Sneaky!","1","1","1","Gain Sneak Attack."},{"CARD TRICK!","2","1","1","A chosen opponent discards 1 Card randomly.  Additionally, draw 1 Card.  If in Shadows, draw 2 Cards instead."},
            {"WILD SHADOW!","4","2","1","Change the value of any one die.  If in Shadows, change the values of any two dice instead."},{"ENTER THE SHADOWS!","4","3","1","Gain Shadows."},
            {"POISON WOUND!","2","3","1","Inflict Poison on a chosen opponent."},{"SHADOW COIN!","0","3","2","Gain 2 CP.  If in Shadows, gain 3 CP instead."},
            {"Dagger Strike II","0","4","0","3 [DAGGER] Deal 4 dmg | 4 [DAGGER] Deal 6 dmg | 5 [DAGGER] Deal 8 dmg | Gain 1 CP.  If [SHADOW] was rolled, inflict Poison.  If [CARD] was rolled, draw 1 Card."},
            {"Pickpocket II","1","4","0","2 [BAG] Gain 3 CP | 3 [BAG] Gain 4 CP | 4 [BAG] Gain 5 CP | 5 [BAG] Gain 6 CP | If [SHADOW] was rolled, up to 2 CP may instead be stolen from your opponent."},
            {"Shifty Strike II/Shadow Strike (adds)","2","4","0","SMALL STRAIGHT Gain 4 CP.  Then deal 1/2 CP as dmg (rounded up). | 2 [DAGGER] 2 [SHADOWS] Deal 1/2 CP as dmg (rounded up).  Inflict Poison."},
            {"Insidious Strike II/Shank Attack (adds)","2","4","0","LARGE STRAIGHT Gain 4 CP.  Then deal CP as dmg. | 1 [DAGGER] 1 [BAG] 1 [CARD] 1 [SHADOW] Gain 1 CP. Gain Sneak Attack.  Draw 1 Card.  Inflict Poison."},
            {"Shadow Dance II","1","4","0","3 [SHADOW] Roll 1 Di:  Deal 1/2 the value as pure dmg.  Then gain Shadows & Sneak Attack.  Draw 1 Card."},
            {"Carducopia II","2","4","0","2 [CARD] Draw 1 Card x [CARD].  If [SHADOW] was rolled, your opponent discards 1 Card randomly.  If [BAG] was rolled, gain 1 CP."},
            {"Shadow Defense II","3","4","0","DEFENSIVE ROLL 5 DICE On 2 [DAGGER], inflict Poison.  On 1 [SHADOW], gain Sneak Attack.  On 2 [SHADOW], gain Sneak Attack and Shadows immediately (ignoring incoming dmg)."},
            {"Counter Strike II","4","4","0","DEFENSIVE ROLL 5 DICE Deal 2 x [DAGGER] dmg.  On [DAGGER] [SHADOW], inflict Poison."},
            {null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null},{null, null, null, null, null}}, //Shadow Thief
            {{"Bunker Up!","2","1","1","A chosen player gains Protect."},
            {"War Room!","1","1","1","Roll 1 Di:  Gain 1/2 the value as Tactical Advantage (rounded up)."},
            {"Disengage!","0","2","1","Play only after being Attacked.  Roll 1 Di:  On [SABER], deal 2 dmg.  On [FLAG], prevent 3 dmg.  On [MEDAL], gain Protect."},
            {"Feigned Retreat!", "2","2","3", " only after being Attacked.  Inflict Constrict on your Attacker.  Prevent 3 incoming dmg."},
            {"Ambush!", "1","3","2", "Gain 2 Tactical Advantage."},
            {"Saber Strike II","1","4","0","3 [SABER] Deal 5 dmg | 4 [SABER] Deal 6 dmg | 5 [SABER] Deal 7 dmg | On 3-of-a-kind (#'s) inflict Constrict."},
            {"Profiteer II","2","4","0","1 [SABER] 3 [FLAG] Gain 2 Tactical Advantage & roll 1 Di:  On [SABER], deal 6 dmg.  On [FLAG], gain 3 Tactical Advantage.  On [MEDAL], draw 1 Card.  Then immediately begin an additional Offensive Roll Phase."},
            {"Carpet Bomb II/Strategize (adds)","2","4","0","2 [SABER] 2 [MEDAL] Gain 2 Tactical Advantage.  Then deal 2 collateral dmg to two different chosen opponents. | 4 [MEDAL] Gain 3 Tactical Advantage.  Draw 2 cards."},
            {"Strategic Approach II/Indirect Approach (adds)","2","4","0","3 [SABER] 2 [MEDAL] Gain Tactical Advantage.  Inflict Constrict.  Then deal 7 dmg. | 3 [SABER] 1 [FLAG] Gain 2 Tactical Advantage.  Then deal 2 undefendable dmg."},
            {"FLANK II","2","4","6","SMALL STRAIGHT Gain 2 Tactical Advantage.  Then deal 6 dmg."},
            {"Exploit II/Interdiction (adds)","2","4","8","LARGE STRAIGHT Gain 3 Tactical Advantage.  Inflict Constrict.  Then deal 9 dmg. | 1 [SABER] 2 [FLAG] 1 [MEDAL] Draw 2 Cards.  Inflict Constrict on a chosen player."},
            {"Maneuver II/Reconnaissance (adds)","2","4","5","4 [MEDAL] Gain 5 Tactical Advantage.  Inflict Constrict.  Then deal 5 undefendable dmg. | 3 [MEDAL] Gain 5 Tactical Advantage."},
            {"Counter Measures II","3","4","0", "DEFENSIVE ROLL 5 DICE For every 2 [SABER], deal 1 dmg.  Prevent 1 x [FLAG] dmg.  Gain 1 x [MEDAL] Tactical Advantage."},
            {"Counter Measures III","5","4","0", "DEFENSIVE ROLL 5 DICE For every 2 [SABER], deal 2 dmg.  Prevent 1 x [FLAG] dmg.  Gain 1 x [MEDAL] Tactical Advantage."},
            {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}}, //Tactician
            {{"CULTIVATE!","3","1","3","Gain 3 Spirits."},
            {"HARVEST!","0","1","3","Remove up to 3 Spirits and gain 1 CP per Spirit removed.  If at least 2 were removed, up to two chosen players gain Wellspring."},
            {"DOWN POUR!","2","1","0","You may grow all existing Spirits once each (in any order)."},
            {"WILL O' WISP!", "1","1","1", "Roll 3 Dice: On [BRANCH], deal 1 collateral dmg to all opponents.  On [LEAF], gain Wellspring.  On [SPIRIT], grow 1 Spirit."},
            {"STOMP!", "1","2", "1", "Roll 5 Dice:  Add 1 x [BRANCH] dmg.  If at least 3 dmg was added, inflict Barbed Vine."},
            {"DRINK DEEP!","1","3","1","A chosen player gains Wellspring."},
            {"SPLINTER II","1","4","0","3 [BRANCH] Deal 5 dmg | 4 [BRANCH] Deal 6 dmg | 5 [BRANCH] Deal 7 dmg | Inflict Barbed Vine."},
            {"SPLINTER III","2","4","0","3 [BRANCH] Deal 5 dmg | 4 [BRANCH] Deal 6 dmg | 5 [BRANCH] Deal 7 dmg | On 3-of-a-kind (#'s) grow a Spirit.  Inflict Barbed Vine."},
            {"TEND II/CULTIVATE (adds)","2","4","4","2 [LEAF] 2 [SPIRIT] Draw 1 Card.  A chose player gains Wellspring.  A chosen opponent is inflicted with Barbed Vine. | 2 [BRANCH] 2 [SPIRIT] Grow 6 Spirits."},
            {"Overgrowth II/Plant (adds)","2","4","4","2 [BRANCH] 3 [LEAF] Deal 4 dmg.  You may remove up to 2 Spirits to add 4 dmg per Spirit removed.  You may discard Wellspring to make this Attack undefendable. | 1 [BRANCH] 2 [LEAF] Grow 3 Spirits."},
            {"Vengeful Vines II/Bitterroot (adds)","2","4","8","SMALL STRAIGHT Inflict Barbed Vine. Deal 8 dmg. | 3 [LEAF] Deal 1 pure dmg per Spirit."},
            {"Call Of The Wild II/Enrapture  (adds)","2","4","8","LARGE STRAIGHT Deal 8 dmg & roll 5 Dice:  Add 1 x [BRANCH] dmg.  On [LEAF], gain Wellspring.  Grow 1 x [SPIRIT] Spirits."},
            {"Nature's Grasp II/Nature's Blessing (adds)","2","4","6","4 [SPIRIT] Grow 2 Spirits.  Then deal 6 undefendable dmg + 1 dmg per Spirit. | 3 [SPIRIT] Heal 1. Gain 1 CP.  Draw 1 Card. Grow 1 Spirit."},
            {"ROOTED II","3","4","0", "DEFENSIVE ROLL 4 DICE Prevent 1 x [BRANCH] + 1 x [SPIRIT] dmg.  On 2 [LEAF], grow a Spirit.  On 2 [SPIRIT], a chosen player gains Wellspring."},
            {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}}, //Treeant
            {{"BLOOD LETTING!","0","1","1","Gain Blood Power & 1 CP."},
            {"CONSUME BLOOD!","0","1","2","Spend up to 2 Blood Power.  Then gain 2 CP per Blood Power spent."},
            {"DARK OMEN!","4","1","4","Gain Mesmerize & 2 Blood Power.  Inflict Bleed on a chosen opponent."},
            {"LIMB FROM LIMB", "1","2", "1", "Roll 5 Dice: Add 1 X [CLAW] to the total dmg.  If at least 3 dmg was added, inflict Bleed."},
            {"BLOOD BOIL!", "0","2", "1", "Add 1 dmg.  Additionally, add 1 more dmg per Bleed token the opponent you are Attacking is afflicted with."},
            {"BLOOD FLOW!","1","3","0","Roll 1 Di:  Gain 1/2 the value as Blood Power (rounded up)."},
            {"GOUGE II","1","4","0","3 [CLAW] Deal 3 dmg | 4 [CLAW] Deal 5 dmg | 5 [CLAW] Deal 7 dmg | On 3-of-a-kind (#'s) gain Blood Power."},
            {"GOUGE III","2","4","0","3 [CLAW] Deal 4 dmg | 4 [CLAW] Deal 6 dmg | 5 [CLAW] Deal 8 dmg | On 3-of-a-kind (#'s) gain Blood Power."},
            {"SANGUIMANCY II/PRESENCE (adds)","2","4","3","3 [GAZE] 1 [DROPLET] Heal 3. Gain 3 Blood Power. | 1 [CLAW] 1 [GAZE] 2 [DROPLET] Gain 2 Blood Power. Draw 1 Card."},
            {"GLAMOUR II/INFLUENCE (adds)","2","4","5","3 [GAZE] Gain 1 CP. Gain Mesmerize. Deal 5 undefendable dmg. | 1 [CLAW] 2 [GAZE] Gain Mesmerize. Inflict 2 Bleed on a chosen opponent."},
            {"REND II","2","4","6","3 [CLAW] 2 [DROPLET] Deal 6 dmg.  Then roll 5 Dice: Add 1 x [CLAW] dmg.  On [GAZE], draw 1 Card. On [DROPLET], gain Blood Power."},
            {"Possess II/Draw Blood (adds)","2","4","0","SMALL STRAIGHT Deal 8 dmg: Inflict Bleed - or - gain Mesmerize. | 2 [CLAW] 1 [DROPLET] Gain 2 Blood Power."},
            {"Blood Magic II/Gash (adds)","2","4","0","LARGE STRAIGHT Gain Blood Power.  Inflict Bleed.  Then deal 8 undefendable dmg."},
            {"Blood Thirst II/Hemorrhage (adds)","2","4","0","4 [DROPLET] Gain 3 Blood Power.  Then deal 6 undefendable dmg. | 3 [DROPLET] Inflict 2 Bleed.  Deal 2 collateral dmg to all opponents."},
            {"IMMORTAL FLESH II","2","4","0","DEFENSIVE ROLL 4 DICE On 2 [CLAWS], inflict Bleed.  On 2 [GAZE], gain Blood Power.  Steal 1 Health per 1 [DROPLET] rolled."},
            {null, null, null, null, null}, {null, null, null, null, null}, {null, null, null, null, null}}, //Vampire Lord
        };
        public static string[,,] moves = new string[,,]
         {
            { {null, null, null}, {null, null, null}, {null, null, null},
              {null, null, null}, {null, null, null}, {null, null, null},
              {null, null, null}, {null, null, null}, {null, null, null}, {null, null, null} }, //Artificer    
            { {"SMACK", "6", "3 [SWORD] Deal 4 dmg | 4 [SWORD] Deal 6 dmg | 5 [SWORD] Deal 8 dmg"}, {"FORTITUDE", "6", "3 [LIFE] Heal 4 | 4 [LIFE] Heal 5 | 5 [LIFE] Heal 6"},
              {"STURDY BLOW", "6", "2 [SWORD] 2 [POW] Deal 4 undefendable dmg."}, {"OVERPOWER", "6", "3 [SWORD] 2 [POW] Roll 3 Dice: Then deal dmg equal to the total roll value. If the roll value is at least 14, inflict Concussion."},
              {"MIGHTY BLOW", "6", "SMALL STRAIGHT Deal 9 dmg."}, {"RECKLESS", "6", "LARGE STRAIGHT Deal 15 dmg, receive 4 (und) dmg in return. (Return dmg only applies if at least 1 dmg was dealt successfully."},
              {"CRIT BASH", "6", "4 [POW] Inflict Stun.  Then deal 5 undefendable dmg."}, {"RAGE!", "6", "5 [POW] Inflict Stun. Deal 15 (und) dmg."},
              {"THICK SKIN", "7", "Roll 3 Dice. Heal 2 x [LIFE]"}, {null, null, null} }, // 8 initial Offensive Abilities + 1 Defensive Ability = 9 total moves initially // Barbarian      
            { { null, null, null}, {null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null}, {null, null, null} }, //Cursed Pirate  
            { { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null}, {null, null, null} }, //Gunslinger     
            { { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null}, {null, null, null} }, //Huntress       
            { { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null}, {null, null, null} }, //Monk           
            { { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null}, {null, null, null} }, //Moon Elf       
            { { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null}, {null, null, null} }, //Ninja          
            { { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null}, {null, null, null} }, //Paladin
            { { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null}, {null, null, null} }, //Pyromancer
            { { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null}, {null, null, null} }, //Samurai
            { { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null},
              { null, null, null}, { null, null, null}, { null, null, null}, {null, null, null} }, //Seraph
            { { "DAGGER STRIKE","6","3 [DAGGER] Deal 4 dmg | 4 [DAGGER] Deal 6 dmg | 5 [DAGGER] Deal 8 dmg | If [BAG] was rolled, gain I CP.  If [SHADOW] was rolled, inflict Poison."},
              { "PICKPOCKET","6","2 [BAG] Gain 2 CP | 3 [BAG] Gain 3 CP | 4 [BAG] Gain 4 CP | If [SHADOW] was rolled, up to 1 CP may instead be stolen from your opponent."},
              { "SHIFTY STRIKE","6","SMALL STRAIGHT Gain 3 CP.  Then deal 1/2 CP as dmg (rounded up)."},
              { "INSIDIOUS STRIKE","6","LARGE STRAIGHT Gain 3 CP.  Then deal CP as dmg."},
              { "SHADOW DANCE","6","3 [SHADOW] Roll 1 Die:  Deal 1/2 the value as pure dmg.  Then gain Shadows & Sneak Attack."},
              { "CARDUCOPIA","6","2 [CARD] Draw 1 x [CARD]." },
              { "SHADOW SHANK!","6","5 [SHADOW] Gain 3 CP and gain Shadows.  Then deal CP as dmg + 5."},
              { "SHADOW DEFENSE","7","DEFENSIVE ROLL 4 DICE | On 2 [DAGGER], inflict Poison.  On [SHADOW], gain Sneak Attack.  On 2 [SHADOW], gain Sneak Attack and Shadows immediately (ignoring incoming dmg)."},
              { "COUNTER STRIKE","7","DEFENSIVE ROLL 5 DICE | Deal 1 x [DAGGER] dmg.  On [DAGGER] [SHADOW], inflict Poison."}, { null, null, null} }, // 7 initial Offensive Abilities + 2 Defensive Ability = 9 total moves initially // Shadow Thief
            { { "SABER STRIKE","6","3 [SABER] Deal 4 dmg | 4 [SABER] Deal 5 dmg | 5 [SABER] Deal 6 dmg."},
              { "PROFITEER","6","1 [SABER] 3 [FLAG] Gain Tactical Advantage & roll 1 Die:  On [SABER], deal 5 dmg.  On [FLAG], gain 4 Tactical Advantage.  On [MEDAL], draw 1 Card.  Then immediately begin an additional Offensive Roll Phase."},
              { "CARPET BOMB","6","2 [SABER] 2 [MEDAL] Gain Tactical Advantage.  Then deal 2 collateral dmg to two different chosen opponents."},
              { "Strategic Approach","6","3 [SABER] 2 [MEDAL] Inflict Constrict.  Deal 7 dmg."},
              { "FLANK","6","SMALL STRAIGHT Gain Tactical Advantage.  Then deal 6 dmg."},
              { "EXPLOIT","6","LARGE STRAIGHT Gain 2 Tactical Advantage.  Inflict Constrict.  Then deal 9 dmg."},
              { "MANUEVER","6","4 [MEDAL] Gain 5 Tactical Advantage.  Then deal 5 undefendable dmg."},
              { "HIGHER GROUND!","6","5 [MEDAL] Inflict Targeted & Constrict.  Increase Tactical Advantage stack limit by 1.  Then gain max Tractical Advantage & deal 12 dmg."},
              { "COUNTERMEASURES","7","DEFENSIVE ROLL 4 DICE | For every 2 [SABER], deal 1 dmg.  Prevent 1 x [FLAG] dmg.  Gain 1 x [MEDAL] Tactical Advantage."}, {null, null, null} }, // 8 initial Offesnvie Abilities + 1 Defensive Ability = 9 total moves initially // Tactician
            { { "FERTILIZE","5","During your Upkeep Phase, grow 1 Spirit."},
              { "SPLINTER","6", "3 [BRANCH] Deal 5 dmg | 4 [BRANCH] Deal 6 dmg | 5 [BRANCH] Deal 7 dmg | You may discard 1 Spirit to inflict Barbed Vine."},
              { "TEND", "6", "2 [LEAF] 2 [SPIRIT] 1 DRAW CARD || 3 GAIN SPIRIT || CHOSEN PLAYER 1 GAIN WELLSPRING || CHOSEN OPPONENT 1 INFLICT BARBED VINE"},
              { "OVERGROWTH", "6", "2 [BRANCH] 3 [LEAF] 2 DMG || MAY REMOVE UP TO 2 SPIRIT || MAY DISCARD 1 WELLSPRING TO MAKE ATTACK -UND 4 DMG PER SPIRIT MAKE -UND"},
              { "VENGEFUL VINES", "6", "SMALL STRAIGHT 1 INFLICT BARBED VINE || 7 DMG"},
              { "CALL OF THE WILD","6", "LARGE STRAIGHT 8 DMG || ROLL 4 DICE: Add 1 DMG x [BRANCH]. On [LEAF], 1 GAIN WELLSPRING. On [SPIRIT], 1 GAIN SPIRIT."},
              { "NATURE'S GRASP","6","4 [SPIRIT] 2 GAIN SPIRIT || 5 DMG -UND || 1 DMG -UND PER SPIRIT"},
              { "WAKE THE FOREST!", "6", "6 [SPIRIT] 1 GAIN WELLSPRING || 1 GAIN WELLSPRING CHOSEN TEAMATE || 5 GAIN SPIRITS || 1 INFLECT BARBED VINE || 10 DMG -UND"},
              { "ROOTED", "7","Roll 3 Dice. 1 [BRANCH] PREVENT 1 DMG, 1 [SPIRIT] PREVENT 1 DMG, 2 [LEAF] 1 GAIN SPIRIT"}, {null, null, null} }, // 1 Passive Ability + 7 initial Offensive Abilities + 1 Defensive Ability = 9 total moves initially // Treeant
            { { "GOUGE", "6", "3 [CLAW] Deal 3 dmg | 4 [CLAW] Deal 5 dmg | 5 [CLAW] Deal 7 dmg | On 4-of-a-kind (#'S), gain Blood Power."},
              { "SANGUIMANCY", "6", "3 [GAZE] 1 [DROPLET] Heal 2. Gain 3 Blood Power."},
              { "GLAMOUR", "6", "3 [GAZE] Gain 1 CP. Gain Mesmerize. Deal 4 undefendable dmg."},
              { "REND", "6", "3 [CLAW] 2 [DROPLET] Deal 6 dmg. Then roll 3 dice. Add 1 DMG x [CLAW]. On [GAZE], draw 1 card. On [DROPLET], gain Blood Power."},
              { "POSSESS","6", "SMALL STRAIGHT Deal 7 dmg & roll 1 die: On [CLAW], inflict Bleed.  On [DROPLET] or [BLOOD], gain Mesmerize."},
              { "BLOOD MAGIC", "6", "LARGE STRAIGHT Gain Blood Power. Inflict Bleed. Then deal 8 dmg.  You may spend Mesmerize to make this undefendable dmg."},
              { "BLOOD THIRST", "6","4 [DROPLET] Gain 2 Blood Power. Then deal 5 undefendable dmg."},
              { "EXSANGUINATE!", "6", "5 [DROPLET] Search your deck for any 1 card. Put it into your hand and then shuffle your deck. Gain 2 Blood Power.  Then deal 10 (und) dmg."},
              { "IMMORTAL FLESH", "7", "Roll 3 dice. On 2 [CLAW], inflict Bleed. | On 2 [GAZE], gain Blood Power. | Steal 1 Health per [DROPLET] rolled."}, {null, null, null} } // 8 initial Offensive Abilities + 1 Defensive Ability = 9 total moves initially // Vampire Lord
        };
        public static string[,] dice = new string[,]
        {
            {"WRENCH", "WRENCH", "WRENCH", "GEAR", "GEAR", "BOLT"},           //Artificer
            {"SWORD", "SWORD", "SWORD", "LIFE", "LIFE", "POW"},               //Barbarian
            {"CUTLASS", "CUTLASS", "CUTLASS", "BOOTY", "BOOTY", "SKULL"},     //Cursed Pirate
            {"BULLET", "BULLET", "BULLET", "DASH", "DASH", "BULLSEYE"},       //Gunslinger
            {"SPEAR", "SPEAR", "CLAW", "CLAW", "BONDED SOUL", "SABER TOOTH"}, //Huntress
            {"FIST", "FIST", "PALM", "ZEN", "ZEN", "LOTUS"},                  //Monk
            {"ARROW", "ARROW", "ARROW", "FOOT", "FOOT", "MOON"},              //Moon Elf
            {"NINJATÓ", "NINJATÓ", "NINJATÓ", "SHURIKEN", "SHURIKEN", "MASK"},//Ninja
            {"SWORD", "SWORD", "HELMET", "HELMET", "LIFE", "PRAYER"},         //Paladin
            {"FLAME", "FLAME", "FLAME", "BLAZE", "FIERY SOUL", "METEOR"},     //Pyromancer
            {"KATANA", "KATANA", "KATANA", "KABUTO", "KABUTO", "RISING SUN"}, //Samurai
            {"BLADE", "BLADE", "BLADE", "WINGS", "CROSS", "ANGELIC PENDANT"}, //Seraph
            {"DAGGER", "DAGGER", "BAG", "BAG", "CARD", "SHADOW"},             //Shadow Thief
            {"SABER", "SABER", "SABER", "FLAG", "FLAG", "MEDAL"},             //Tactician
            {"BRANCH", "BRANCH", "BRANCH", "LEAF", "LEAF", "SPIRIT"},         //Treeant
            {"CLAW", "CLAW", "CLAW", "GAZE", "GAZE", "DROPLET"}               //Vampire Lord
        };
        public static List<StatusEffect> generateStatusEffects(int charSelect)
        {
            int i = 0; List<StatusEffect> generatedStatusEffects = new List<StatusEffect>();
            while (statusEffects[charSelect, i, 0] != null)
            {
                StatusEffect statusEffect = new StatusEffect(statusEffects[charSelect, i, 0], int.Parse(statusEffects[charSelect, i, 1]), statusEffects[charSelect, i, 2]);
                generatedStatusEffects.Add(statusEffect); i++;
            }
            return generatedStatusEffects;
        }
        public static List<Cards> generateDeck(int charSelect)
        {
            int i = 0; List<Cards> generatedDeck = new List<Cards>(); charSelect++;
            while (cards[charSelect, i, 0] != null)                     // ALL CARDS be it MOVES / PLAYABLE CARDS HAVE A TITLE (0) == charSelect, i, 0
            {
                switch (cards[charSelect, i, 2])                        // the TWO references the CARD TYPE
                {
                    case "1":
                        Cards card = new MainPhaseCard(int.Parse(cards[charSelect, i, 1]), cards[charSelect, i, 0], cards[charSelect, i, 4]);
                        generatedDeck.Add(card); i++; break;
                    case "2":
                        Cards card2 = new RollPhaseCard(int.Parse(cards[charSelect, i, 1]), cards[charSelect, i, 0], cards[charSelect, i, 4]);
                        generatedDeck.Add(card2); i++; break;
                    case "3":
                        Cards card3 = new InstantActionCard(int.Parse(cards[charSelect, i, 1]), cards[charSelect, i, 0], cards[charSelect, i, 4]);
                        generatedDeck.Add(card3); i++; break;
                    case "4":
                        Cards card4 = new HeroUpgradeCard(int.Parse(cards[charSelect, i, 1]), cards[charSelect, i, 0], cards[charSelect, i, 4]);
                        generatedDeck.Add(card4); i++; break;
                }
            }
            i = 0; while (i < 18)
            {
                switch (cards[0, i, 2])                                 // this generates the stock deck
                {
                    case "1":
                        Cards card = new MainPhaseCard(int.Parse(cards[0, i, 1]), cards[0, i, 0], cards[0, i, 4]);
                        generatedDeck.Add(card); i++; break;
                    case "2":
                        Cards card2 = new RollPhaseCard(int.Parse(cards[0, i, 1]), cards[0, i, 0], cards[0, i, 4]);
                        generatedDeck.Add(card2); i++; break;
                    case "3":
                        Cards card3 = new InstantActionCard(int.Parse(cards[0, i, 1]), cards[0, i, 0], cards[0, i, 4]);
                        generatedDeck.Add(card3); i++; break;
                }
            }
            return generatedDeck;
        }
        public static List<Cards> generateMoves(int charSelect)
        {
            int i = 0; List<Cards> generatedMoves = new List<Cards>();
            while (moves[charSelect, i, 0] != null)                     // ALL CARDS be it MOVES / PLAYABLE CARDS HAVE A TITLE (0) == charSelect, i, 0
            {
                switch (moves[charSelect, i, 1])                        // the TWO references the CARD TYPE
                {
                    case "5":
                        Cards card = new PassiveAbility(moves[charSelect, i, 0], moves[charSelect, i, 2]);
                        generatedMoves.Add(card); i++; break;
                    case "6":
                        Cards card2 = new OffensiveAbility(moves[charSelect, i, 0], moves[charSelect, i, 2]);
                        generatedMoves.Add(card2); i++; break;
                    case "7":
                        Cards card3 = new DefensiveAbility(moves[charSelect, i, 0], moves[charSelect, i, 2]);
                        generatedMoves.Add(card3); i++; break;
                }
            }
            return generatedMoves;
        }
        public static List<Dice> generateDice(int charSelect)
        {
            int y = 1, m = 0; List<Dice> generatedDice = new List<Dice>();

            string[] symbols = new string[6];
            for (int i = 0; i < 6; i++) symbols[i] = dice[charSelect, i];

            while (y < 6)
            {
                Dictionary<int, string> sAS = new Dictionary<int, string>();
                while (m < 6)
                {
                    sAS.Add(m, dice[charSelect, m]); m++;
                }
                Dice die = new Dice(y, symbols, sAS);
                generatedDice.Add(die); y++;
            }
            return generatedDice;
        }
    }

    class StatusEffect
    {
        public string name, status;
        public int quantity, value;
        public bool isPersistent = false;
        public StatusEffect(string n, int q, string s) { this.name = n; this.quantity = q; this.status = s; }
    }

    class Moves
    {
        public string name, diceCombo;
        public bool isInvisibleLocked = true;
    } //Moves are cards == Passive Abilities type 5, Offensive Abilities type 6, Defensive Abilities type 7

    class Dice
    {
        public int number;
        public string[] symbols;
        public Dictionary<int, string> sidesAndSymbols = new Dictionary<int, string>();
        public Dice(int num, string[] sym, Dictionary<int, string> sAS) { this.number = num; this.symbols = sym; this.sidesAndSymbols = sAS; }
    } // NOT USED YET -- mainly a shell class for the 3D dice imagery

    class GameBoard
    {
        public List<StatusEffect> inPlaySEs = new List<StatusEffect>();
    } // NEED TO IMPLEMENT SOON /\/ will house all the in-play Status Effects for the character
}