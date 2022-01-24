using System;
using System.Collections;

namespace diceThroneAR
{
    class Program
    {
    
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to DICE THRONE!");
            /*string[] characterNames = { "Cursed Pirate Test", "Vampire Lord" }; string characterName = characterNames[0].ToString();
            string u = characterName[0].ToString() + characterName[1].ToString(); u += characterName[2].ToString() + characterName[3].ToString();
            u += characterName[4].ToString() + characterName[5].ToString(); string p = characterName[7..]; string p2 = characterName.Substring(7, 6);
            */
            //what we doin? RANGE
            //Random test = new Random();
            //int access = test.Next();
            //Console.WriteLine(access + "\n");
            //Console.WriteLine(characterName);
            //Console.WriteLine(p);
            //Console.WriteLine(p2);
            //int fortyFive = int.Parse(Console.ReadLine());
            Console.WriteLine("Please select your character:");
            ArrayList characters  = new() { "[0] Artificer", "[1] Barbarian", "[2] Cursed Pirate", "[3] Gunslinger", "[4] Huntress", "[5] Monk", "[6] Moon Elf", "[7] Ninja" };
            ArrayList characters2 = new() { "[8] Paladin", "[9] Pyromancer", "[10] Samurai", "[11] Seraph", "[12] Shadow Thief", "[13] Tactician", "[14] Treeant", "[15] Vampire Lord" };
            characters.AddRange(characters2);
            int count = 1; int count2 = 0;
            //foreach (string name in characters) { if (count != characters.Count) { Console.Write(name + ", "); count++; } else { Console.Write(name); } }
            foreach (string name in characters)
            {
                if (count != characters.Count) {
                    if (count2 != 3) {
                        Console.Write(name + ", "); count++; count2++;
                    } else {
                        Console.WriteLine(name); count2 = 0;                   }
                    } 
                else { 
                    Console.Write(name); 
                } 
            }
                Console.WriteLine();
            Console.WriteLine("Enter your character's number to continue: ");
            int charSelect = int.Parse(Console.ReadLine());
            Console.WriteLine($"You chose {characters[charSelect]}");
            
            int control = 0;
            int total = 0;
            string restart = "A";
            ArrayList grades = new();
            int numy;
            for (int x = 0; x < 3; x++) {
                Console.WriteLine();
                numy = int.Parse(Console.ReadLine());
                Console.WriteLine(numy);
            }

            //uint posNum;
            //unlicensed is any positive integer 0-4294967295
            //provides double range of positive numbers
            //char response = Console.ReadLine()[0]; 
            while (restart != "Q")
            {

                Console.WriteLine("Enter a student's grades (0-20 pts) to calculate the average, enter -1 to exit.");

                while (control != -1)
                {
                    if (!int.TryParse(Console.ReadLine(), out int num))
                    {
                        Console.WriteLine("Invalid value entered, try again.");
                    }
                    else if ((num < 0 || num > 20) && num != -1)
                    {
                        Console.WriteLine("Value must be within 0 and 20");
                    }
                    else
                    {
                        if (num == -1) { Console.WriteLine("Processing average ---"); break; }
                        else { Console.WriteLine("You entered {0}", num); control = num; grades.Add(num); }
                    }
                }

                foreach (int value in grades) { total += value; }
                if (grades.Count == 0) { Console.WriteLine("You did not enter any values.\n"); }
                else
                {
                    Console.Write("\nThe student's average is {0} ({1}/{2}) \n[ ", total / grades.Count, total, grades.Count);
                    int tic = 1; int tok = 0;
                    while (tok != grades.Count)
                    {
                        if (tic <= 4) { Console.Write(grades[tok] + " "); tic++; tok++; }
                        else { Console.Write("\n  "); tic = 1; }
                    }

                    Console.Write("]\n");
                }

                total = 0;
                grades = new();
                control = 0;

                Console.WriteLine("Do you wish to calculate another student's average? \n\n" +
                                  "Press any key to enter another student's scores or Q to quit");

                restart = Console.ReadLine().ToUpper();

            }            
        }
    }
        
    class Characters
    {       }

    class GamePlayLogic
    {       }
}