using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SnakeGame
{
    internal class Program
    {
        static int score = 0;
        static int highScore = 0;
        static int frameDelay = 120;
        static bool isPaused = false;
        static Random random = new Random();

        static void Main()
        {
            Console.Title = "Advanced Snake Game 🐍";
            Console.CursorVisible = false;
            LoadHighScore();

            while (true)
            {
                ShowMainMenu();
                RunGame();
            }
        }

        static void ShowMainMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("===================================");
            Console.WriteLine("         ADVANCED SNAKE 🐍        ");
            Console.WriteLine("===================================");
            Console.WriteLine("1. Start Game");
            Console.WriteLine("2. View High Score");
            Console.WriteLine("3. Exit");
            Console.Write("Select an option: ");
            string choice = Console.ReadLine();

            if (choice == "2")
            {
                Console.Clear();
                Console.WriteLine($"High Score: {highScore}");
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                ShowMainMenu();
            }
            else if (choice == "3")
            {
                Environment.Exit(0);
            }
        }

        static void RunGame()
        {
            Console.Clear();
            Console.CursorVisible = false;

            Coords gridSize = new Coords(50, 20);
            List<Coords> snake = new List<Coords> { new Coords(10, 10) };
            Coords apple = new Coords(random.Next(1, gridSize.X - 1), random.Next(1, gridSize.Y - 1));
            List<Coords> obstacles = GenerateObstacles(gridSize, 5);

            Direction direction = Direction.Right;
            score = 0;
            frameDelay = 120;

            while (true)
            {
                if (!isPaused)
                {
                    Console.Clear();
                    Coords newHead = new Coords(snake[0].X, snake[0].Y);
                    newHead.applyDirection(direction);
                    snake.Insert(0, newHead);

                    if (CheckCollision(newHead, gridSize, snake, obstacles))
                    {
                        GameOver();
                        return;
                    }

                    if (newHead.Equals(apple))
                    {
                        score += 10;
                        apple = new Coords(random.Next(1, gridSize.X - 1), random.Next(1, gridSize.Y - 1));

                        if (score % 50 == 0)
                        {
                            // Increase difficulty
                            frameDelay = Math.Max(50, frameDelay - 10); 
                        }
                    }
                    else
                    {
                        snake.RemoveAt(snake.Count - 1);
                    }

                    DrawGame(gridSize, snake, apple, obstacles);
                    Thread.Sleep(frameDelay);
                }

                direction = GetUserInput(direction);
            }
        }

        static bool CheckCollision(Coords head, Coords gridSize, List<Coords> snake, List<Coords> obstacles)
        {
            if (head.X == 0 || head.Y == 0 || head.X == gridSize.X - 1 || head.Y == gridSize.Y - 1)
                return true;

            if (snake.GetRange(1, snake.Count - 1).Contains(head))
                return true;

            if (obstacles.Contains(head))
                return true;

            return false;
        }

        static List<Coords> GenerateObstacles(Coords gridSize, int count)
        {
            List<Coords> obstacles = new List<Coords>();
            for (int i = 0; i < count; i++)
            {
                obstacles.Add(new Coords(random.Next(1, gridSize.X - 1), random.Next(1, gridSize.Y - 1)));
            }
            return obstacles;
        }

        static void DrawGame(Coords gridSize, List<Coords> snake, Coords apple, List<Coords> obstacles)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Score: {score}  |  High Score: {highScore}");

            for (int y = 0; y < gridSize.Y; y++)
            {
                for (int x = 0; x < gridSize.X; x++)
                {
                    Coords currentCoord = new Coords(x, y);

                    if (snake.Contains(currentCoord))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("■");
                    }
                    else if (apple.Equals(currentCoord))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("🍎");
                    }
                    else if (obstacles.Contains(currentCoord))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("▓");
                    }
                    else if (x == 0 || y == 0 || x == gridSize.X - 1 || y == gridSize.Y - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

        static void GameOver()
        {
            if (score > highScore)
            {
                highScore = score;
                SaveHighScore();
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("===================================");
            Console.WriteLine("         GAME OVER 💀             ");
            Console.WriteLine($"Your Score: {score}");
            Console.WriteLine($"High Score: {highScore}");
            Console.WriteLine("Press any key to restart...");
            Console.ReadKey();
        }

        static Direction GetUserInput(Direction currentDirection)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (currentDirection != Direction.Down) return Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (currentDirection != Direction.Up) return Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (currentDirection != Direction.Right) return Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (currentDirection != Direction.Left) return Direction.Right;
                        break;
                    case ConsoleKey.P:
                        isPaused = !isPaused;
                        break;
                }
            }
            return currentDirection;
        }

        static void SaveHighScore()
        {
            File.WriteAllText("highscore.txt", highScore.ToString());
        }

        static void LoadHighScore()
        {
            if (File.Exists("highscore.txt"))
            {
                int.TryParse(File.ReadAllText("highscore.txt"), out highScore);
            }
        }
    }

}
