using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;


public class Program
{
    public static void Main()
    {
        int boardSize = 100;
        int numLadders = 6;
        int numSnakes = 6;
        int numPlayers = 2;

        Dictionary<int, int> ladders = GenerateRandomPairs(numLadders, boardSize, null);
        Dictionary<int, int> snakes = GenerateRandomPairs(numSnakes, boardSize, ladders);
        List<int> goldenSlots = GenerateGoldenSlots(ladders, snakes, boardSize);

        List<Player> players = new List<Player>();
        for (int i = 1; i <= numPlayers; i++)
        {
            Console.Write("Enter Player name: ");
            Player Player = new Player(Console.ReadLine());
            players.Add(Player);
        }
        int currentPlayerIndex = 0;
        while (true)
        {
            Player currentPlayer = players[currentPlayerIndex];
            int diceRoll = RollDice();
            int newPosition = currentPlayer.Position + diceRoll;
            if (ladders.ContainsKey(newPosition))
            {
                newPosition = ladders[newPosition];
            }
            else if (snakes.ContainsValue(newPosition))
            {
                newPosition = snakes.FirstOrDefault(x => x.Value == newPosition).Key;
            }
           
            if (newPosition >= boardSize)
            {
                Console.WriteLine($"{currentPlayer.Name} has reached or passed the final position!");
                Console.WriteLine("Game over!");

                break;
            }
            currentPlayer.Position = newPosition;
            Console.WriteLine($"{currentPlayer.Name} rolled a {diceRoll} and moved to position {newPosition}");

            if (currentPlayerIndex == 0)
            {
                if (goldenSlots.Contains(newPosition))
                {
                    int newPosition2 = players[1].Position;
                    players[1].Position = newPosition;
                    players[currentPlayerIndex].Position = newPosition2;
                }
                currentPlayerIndex = 1;
            }
            else if (currentPlayerIndex==1)
            {
                if (goldenSlots.Contains(newPosition))
                {
                    int newPosition2 = players[currentPlayerIndex].Position;
                    players[0].Position = newPosition2;
                    players[currentPlayerIndex].Position = newPosition;
                }
                currentPlayerIndex = 0;
            }
        }
    }
    static int RollDice()
    {
        int result = 0;
        for (int i = 0; i < 2; i++)
        {
            Random random = new Random();
            result += random.Next(1, 7); ;
        }
        return result;
    }
    private static Dictionary<int, int> GenerateRandomPairs(int numSnakesOrLadders, int boardSize, Dictionary<int, int> ladders)
    {
        Dictionary<int, int> pairs = new Dictionary<int, int>();
        Random random = new Random();
        while (numSnakesOrLadders > 0)
        {
            int start;
            int startNextRow;
            int end;
            if (ladders != null)
            {
                do
                {
                    start = random.Next(0, boardSize - 10);
                }
                while (ladders.ContainsKey(start) || ladders.ContainsValue(start) && pairs.ContainsValue(start) && pairs.ContainsKey(start));
                startNextRow = start + ((boardSize - start) % 10) + 1;
                do
                {
                    end = random.Next(startNextRow, boardSize);
                }
                while (ladders.ContainsKey(end) || ladders.ContainsValue(end) && pairs.ContainsValue(end) && pairs.ContainsKey(end));
                numSnakesOrLadders--;
                pairs.Add(start, end);
            }
            else
            {
                if (pairs.Count > 0)
                {
                    do
                    {
                        start = random.Next(0, boardSize - 10);
                    }
                    while (pairs.ContainsKey(start) || pairs.ContainsValue(start));
                    startNextRow = start + ((boardSize - start) % 10) + 1;
                    do
                    {
                        end = random.Next(startNextRow, boardSize);
                    }
                    while (pairs.ContainsKey(end) || pairs.ContainsValue(end));
                    numSnakesOrLadders--;
                    pairs.Add(start, end);
                }
                else
                {
                    start = random.Next(0, boardSize - 10);
                    startNextRow = start + ((boardSize - start) % 10) + 1;
                    end = random.Next(startNextRow, boardSize);
                    numSnakesOrLadders--;
                    pairs.Add(start, end);
                }
            }
        }
        return pairs;
    }
    private static List<int> GenerateGoldenSlots(Dictionary<int, int> ladders, Dictionary<int, int> snakes, int BoardSize)
    {
        var goldenSlots = new List<int>();
        var random = new Random();

        while (goldenSlots.Count < 2)
        {
            int slot = random.Next(1, BoardSize - 1);
            if (!snakes.ContainsKey(slot) && !ladders.ContainsKey(slot) && !goldenSlots.Contains(slot))
                goldenSlots.Add(slot);
        }

        return goldenSlots;
    }
    class Player
    {
        public string Name { get; }
        public int Position { get; set; }

        public Player(string name)
        {
            Name = name;
            Position = 1;
        }
    }
}
