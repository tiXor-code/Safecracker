/*
Hello! For this test we would like you to re-create the “Safe Cracker” mini-game.
Please see the next page for an explanation of the game’s logic.
Technical Specifications
The game must be written using c#, making full use of OOP principles.
It will be reviewed using VSCode and should run and display output through the console.
Game Logic
There are 9 safes that can be opened, each one containing a multiplier. The range of possible
multipliers are x15, x16, x17, x18, x19 and x20.
In any game, only 3 of the above range will be selected and each of these will be randomly
distributed 3 times in the grid. An example outcome is shown below:
-------------------------------------
|  1  |  2  |  3  |
| x18 | x15 | x20 |
-------------------------------------
|  4  |  5  |  6  |
| x15 | x15 | x18 |
-------------------------------------
|  7  |  8  |  9  |
| x20 | x20 | x18 |
-------------------------------------
You will use ascii characters to draw an empty, numbered grid in the console and prompt for a ‘Spin’
which will be activated with a key press.
E.g.:
-------------------------------------
| 1 | 2 | 3 |
-------------------------------------
| 4 | 5 | 6 |
-------------------------------------
| 7 | 8 | 9 |
-------------------------------------
Press [SPACE] to spin
You will then randomly select and display a ‘safe’ number (1-9) With a message to say what number
was 'spun'.
You will then reveal the multiplier contained in that safe by displaying the grid again.
E.g:
-------------------------------------
| 1 | 2 | 3 |
-------------------------------------
| 4 | 5 | x18 |
-------------------------------------
| 7 | 8 | 9 |
-------------------------------------
Press [SPACE] to spin
There are up to 4 spins to be completed. The player will continue to 'spin' and open safes until the
win condition is met. The game is won when a player matches 2 multipliers e.g.
- Spin 1 opens safe 6, a x18 multiplier is revealed
- Spin 2 opens safe 4, a x15 multiplier is revealed
- Spin 3 opens safe 9, a x18 multiplier is revealed
- The game is complete, the player has won x18 their initial bet amount
Given 4 spins, a win will always occur. This is intentional. This game doesn’t have a lose condition.
You cannot 'spin' the same safe number(and therefore reveal the contents) twice.
The multiplier is applied to the player’s initial bet amount to provide a final win amount, which you
will display to the player on completion of the game.
*/

using System;
using System.Collections.Generic;

namespace SafeCrackerGame
{
    class SafeCracker
    {
        private const int NumSafes = 9;
        private const int NumSpins = 4;
        private const int WinThreshold = 2;

        private static readonly string[] multipliers = { "x15", "x16", "x17", "x18", "x19", "x20" };

        private readonly List<int> availableSafes;
        private readonly List<int> openedSafes;
        private readonly List<int> multipliersSelected;

        private int spinsLeft;
        private int betAmount;
        private bool gameWon;

        public SafeCracker()
        {
            availableSafes = new List<int>();
            openedSafes = new List<int>();
            multipliersSelected = new List<int>();
        }

        public void PlayGame()
        {
            InitializeGame();

            while (!gameWon && spinsLeft > 0)
            {
                Console.Clear();
                DisplayGrid();
                Console.WriteLine("Press [SPACE] to spin");
                Console.ReadKey(true);

                SpinSafe();
                Console.Clear();
                DisplayGrid();

                if (openedSafes.Count > 0)
                {
                    Console.WriteLine("\nMultiplier revealed: " + multipliers[multipliersSelected[openedSafes[openedSafes.Count - 1]]]);
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                }

                CheckWinCondition();
                spinsLeft--;
            }

            if (gameWon)
            {
                int multiplier = int.Parse(multipliers[multipliersSelected[openedSafes[0]]].Substring(1));
                int finalWinAmount = betAmount * multiplier;

                //Console.WriteLine("\nCongratulations! You won " + finalWinAmount + " your initial bet amount.");
            }
            else
            {
                Console.WriteLine("\nGame over. You didn't win this time.");
            }
        }

        private void InitializeGame()
        {
            availableSafes.Clear();
            openedSafes.Clear();
            multipliersSelected.Clear();

            for (int i = 0; i < NumSafes; i++)
            {
                availableSafes.Add(i);
            }

            for (int i = 0; i < NumSafes; i++)
            {
                int randomMultiplierIndex = new Random().Next(0, multipliers.Length);
                multipliersSelected.Add(randomMultiplierIndex);
            }

            spinsLeft = NumSpins;
            betAmount = 10; // Change this value to set the initial bet amount
            gameWon = false;
        }

        private void DisplayGrid()
        {
            Console.WriteLine("-----------------");
            for (int i = 0; i < NumSafes; i++)
            {
                if (openedSafes.Contains(i))
                {
                    Console.Write("| " + multipliers[multipliersSelected[i]] + " ");
                }
                else
                {
                    Console.Write("| " + (i + 1) + " ");
                }

                if ((i + 1) % (int)Math.Sqrt(NumSafes) == 0)
                {
                    Console.WriteLine("|\n-----------------");
                }
            }
        }

        private void SpinSafe()
        {
            Console.WriteLine("\nSpinning...");
            int randomSafeIndex = new Random().Next(0, availableSafes.Count);
            int selectedSafe = availableSafes[randomSafeIndex];
            openedSafes.Add(selectedSafe);
            availableSafes.RemoveAt(randomSafeIndex);
            Console.WriteLine("Safe " + (selectedSafe + 1) + " opened!");
        }

        private void CheckWinCondition()
{
    if (openedSafes.Count >= WinThreshold)
    {
        Dictionary<int, int> multiplierCount = new Dictionary<int, int>();

        foreach (int openedSafe in openedSafes)
        {
            int multiplierIndex = multipliersSelected[openedSafe];

            if (multiplierCount.ContainsKey(multiplierIndex))
            {
                multiplierCount[multiplierIndex]++;
            }
            else
            {
                multiplierCount[multiplierIndex] = 1;
            }
        }

        foreach (KeyValuePair<int, int> pair in multiplierCount)
        {
            if (pair.Value >= WinThreshold)
            {
                int multiplierIndex = pair.Key;
                int revealedMultiplier = int.Parse(multipliers[multiplierIndex].Substring(1));
                int finalWinAmount = betAmount * revealedMultiplier;
                gameWon = true;
                Console.WriteLine("\nCongratulations! You won " + finalWinAmount + " your initial bet amount.");
                break;
            }
        }
    }
}

    }

    class Program
    {
        static void Main(string[] args)
        {
            SafeCracker game = new SafeCracker();
            game.PlayGame();
        }
    }
}