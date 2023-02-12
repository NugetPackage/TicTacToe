using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] gameBoardNumbers = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            DrawInitialGameBoard(gameBoardNumbers);

            List<Player> players = new();
            Player playerOne = new();
            playerOne.Name = "one";
            Player playerTwo = new();
            playerTwo.Name = "two";

            players.Add(playerOne);
            players.Add(playerTwo);

            List<int> occupiedNumbers = new();

            bool playerHasWon = false;
            while (!playerHasWon)
            {
                if (occupiedNumbers.Count > 0)
                {
                    DrawGameBoardWithPlayers(gameBoardNumbers, players);
                }

                Console.WriteLine();

                foreach (var player in players)
                {
                    bool choosingOccupiedSpot = true;
                    bool errorReadingInput = true;
                    while (choosingOccupiedSpot)
                    {
                        int playerChosenNumber = new();
                        try
                        {
                            playerChosenNumber = AskForInput(player.Name);
                            errorReadingInput = false;
                        }
                        catch (NotSupportedException exception)
                        {
                            Console.WriteLine($"Numbers 1 - 9 are valid. You entered: {exception.Message}.");
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Only numbers allowed.");
                        }

                        if (!errorReadingInput)
                        {
                            if (occupiedNumbers.Count == 0)
                            {
                                occupiedNumbers.Add(playerChosenNumber);
                                player.ChosenNumbers.Add(playerChosenNumber);
                                choosingOccupiedSpot = false;
                            }
                            else
                            {
                                choosingOccupiedSpot = occupiedNumbers.Any(p => p == playerChosenNumber);
                                if (!choosingOccupiedSpot)
                                {
                                    player.ChosenNumbers.Add(playerChosenNumber);
                                    occupiedNumbers.Add(playerChosenNumber);
                                }
                            }
                        }
                    }

                    playerHasWon = PlayerHasWon(player);
                    if (playerHasWon)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Congratulations player: {player.Name}. You won!");
                        break;
                    }
                }

                DrawGameBoardWithPlayers(gameBoardNumbers, players);

                if (!playerHasWon)
                {
                    Console.Clear();
                }
            }
        }

        private static bool PlayerHasWon(Player player)
        {
            var playerHasWon = false;
            foreach (var combinations in GetWinningCombinations())
            {
                playerHasWon = combinations.All(p => player.ChosenNumbers.Contains(p));
                if (playerHasWon)
                {
                    break;
                }
            }

            return playerHasWon;
        }

        private static void DrawGameBoardWithPlayers(int[,] gameBoardNumbers, List<Player> players)
        {
            var playerOne = players.First(p => p.Name == "one");
            var playerOnePositions = TranslatePlayerNumbersIntoPositions(playerOne.ChosenNumbers);
            var playerTwo = players.First(p => p.Name == "two");
            var playerTwoPositions = TranslatePlayerNumbersIntoPositions(playerTwo.ChosenNumbers);

            for (int row = 0; row < gameBoardNumbers.GetLength(0); row++)
            {
                for (int column = 0; column < gameBoardNumbers.GetLength(1); column++)
                {
                    var playerOneHasMatchingPosition = playerOnePositions.Any(p => p.Equals(Tuple.Create(row, column)));
                    var playerTwoHasMatchingPosition = playerTwoPositions.Any(p => p.Equals(Tuple.Create(row, column)));
                    if (playerOneHasMatchingPosition)
                    {
                        Console.Write("X");
                    }

                    else if (playerTwoHasMatchingPosition)
                    {
                        Console.Write("O");
                    }

                    else
                    {
                        Console.Write(gameBoardNumbers[row, column]);
                    }
                }

                Console.WriteLine();
            }
        }

        private static List<Tuple<int, int>> TranslatePlayerNumbersIntoPositions(List<int> chosenNumbers)
        {
            List<Tuple<int, int>> positions = new();

            foreach (var number in chosenNumbers)
            {
                switch (number)
                {
                    case 1:
                        positions.Add(Tuple.Create(0, 0));
                        break;
                    case 2:
                        positions.Add(Tuple.Create(0, 1));
                        break;
                    case 3:
                        positions.Add(Tuple.Create(0, 2));
                        break;
                    case 4:
                        positions.Add(Tuple.Create(1, 0));
                        break;
                    case 5:
                        positions.Add(Tuple.Create(1, 1));
                        break;
                    case 6:
                        positions.Add(Tuple.Create(1, 2));
                        break;
                    case 7:
                        positions.Add(Tuple.Create(2, 0));
                        break;
                    case 8:
                        positions.Add(Tuple.Create(2, 1));
                        break;
                    case 9:
                        positions.Add(Tuple.Create(2, 2));
                        break;
                }
            }

            return positions;
        }

        private static List<List<int>> GetWinningCombinations()
        {
            List<List<int>> winningCombinations = new();
            winningCombinations.Add(new() { 1, 2, 3 });
            winningCombinations.Add(new() { 4, 5, 6 });
            winningCombinations.Add(new() { 7, 8, 9 });
            winningCombinations.Add(new() { 1, 4, 7 });
            winningCombinations.Add(new() { 2, 5, 8 });
            winningCombinations.Add(new() { 3, 6, 9 });
            winningCombinations.Add(new() { 1, 5, 9 });
            winningCombinations.Add(new() { 3, 5, 7 });

            return winningCombinations;
        }

        private static int AskForInput(String playerName)
        {
            Console.Write($"Player {playerName}, choose number: ");
            var chosenNumber = int.Parse(Console.ReadLine());
            if (chosenNumber < 1 || chosenNumber > 9)
            {
                throw new NotSupportedException(chosenNumber.ToString());
            }

            return chosenNumber;
        }

        private static void DrawInitialGameBoard(int[,] gameBoardNumbers)
        {
            for (int row = 0; row < gameBoardNumbers.GetLength(0); row++)
            {
                for (int column = 0; column < gameBoardNumbers.GetLength(1); column++)
                {
                    Console.Write(gameBoardNumbers[row, column]);
                }

                Console.WriteLine();
            }
        }

        public class Player
        {
            public string Name { get; set; }
            public List<int> ChosenNumbers { get; set; } = new();
        }
    }
}
