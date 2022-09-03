using System; using System.Collections.Generic; using System.Linq;
using System.Text; using System.Threading.Tasks; using System.Threading;

namespace diceThroneAR
{
    class Battleground //WHAT IS BATTLEGROUND? -- Functions that help with flow of game.
    {
        public static void printEachChar(string word)
        { for (int counter = 0; counter < word.Length; counter++)
            { Console.Write(word[counter].ToString()); Thread.Sleep(10); } }

        public static void rollForFirst(Character p1, Character p2) // TODO : Update parameters to take List<Characters> and add TIEBREAKER protocol
        {
            int player = 1, highestRoll = 0, whoRolledHighest = 0;
            while (GameFlow.numberOfPlayers != 0)
            {
                Console.WriteLine($"\nPlaya {player}: Press enter to roll:");
                Console.ReadLine();
                Random roll = new Random(); int rollResult = roll.Next(0, 7);
                Console.WriteLine($"Playa {player}: You rolled a got damn {rollResult}!");
                if (rollResult > highestRoll) { highestRoll = rollResult; whoRolledHighest = player; };
                player++; GameFlow.numberOfPlayers--; 
            };
            switch (whoRolledHighest)
            {
                case 1: p1.winFirstRoll = true; break;
                case 2: p2.winFirstRoll = true; break;
            }
            if (p1.winFirstRoll == true) Console.WriteLine($"\n{p1.name} rolls first!");
                else if (p2.winFirstRoll == true) Console.WriteLine($"\n{p2.name} rolls first!");
        }
    }

    class Cards
    {
        public int CPCost { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Type { get; set; }

        public Cards(int cpcost, string name, string desc)
        {
            this.CPCost = cpcost; this.Name = name.ToUpper(); this.Desc = desc;
        }

        public virtual void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + "\nDescription: " + Desc );
        }

        public void Action() { }
    }

    class MainPhaseCard : Cards
    {
        new public string Type = "Main Phase Card"; // 1

        public MainPhaseCard(int cpcost, string name, string desc) : base(cpcost, name, desc)
        { }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + "\n" +
                "Type: " + Type + " Description: " + Desc + "\n");
        }
    }

    class RollPhaseCard : Cards
    {
        new public string Type = "Roll Phase Card"; // 2

        public RollPhaseCard(int cpcost, string name, string desc) : base(cpcost, name, desc)
        { }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + "\n" +
                "Type: " + Type + " Description: " + Desc + "\n");
        }
    }

    class InstantActionCard : Cards
    {
        new public string Type = "Instant Action Card"; //3

        public InstantActionCard(int cpcost, string name, string desc) : base(cpcost, name, desc)
        { }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + "\n" +
                "Type: " + Type + " Description: " + Desc + "\n");
        }
    }

    class HeroUpgradeCard : Cards
    {
        new public string Type = "Hero Upgrade Card"; //4

        public HeroUpgradeCard(int cpcost, string name, string desc) : base(cpcost, name, desc)
        { }
        public override void ShowDetails()
        {
            Console.WriteLine(Name + " CP Cost: " + CPCost + "\n" +
                "Type: " + Type + " Description: " + Desc + "\n");
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