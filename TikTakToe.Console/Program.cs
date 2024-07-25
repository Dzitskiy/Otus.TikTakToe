using System;

namespace TicTacToe
{
    class Program
    {
        static char[,] board = new char[3, 3]; // Игровое поле
        static char currentPlayer = 'X'; // Текущий игрок

        static void Main(string[] args)
        {
            InitializeBoard(); // Инициализация игрового поля
            PlayGame(); // Начало игры
        }

        static void InitializeBoard()
        {
            // Заполняем игровое поле пустыми ячейками
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }

        static void PlayGame()
        {
            bool gameOver = false;
            while (!gameOver)
            {
                DrawBoard(); // Отображаем текущее состояние игрового поля
                GetPlayerMove(); // Получаем ход игрока
                CheckWin(); // Проверяем, есть ли победитель
                CheckTie(); // Проверяем, есть ли ничья
                ChangePlayer(); // Меняем текущего игрока
            }
        }

        static void DrawBoard()
        {
            Console.Clear();
            Console.WriteLine("   0   1   2");
            Console.WriteLine("0  {0} | {1} | {2}", board[0, 0], board[0, 1], board[0, 2]);
            Console.WriteLine("  ---+---+---");
            Console.WriteLine("1  {0} | {1} | {2}", board[1, 0], board[1, 1], board[1, 2]);
            Console.WriteLine("  ---+---+---");
            Console.WriteLine("2  {0} | {1} | {2}", board[2, 0], board[2, 1], board[2, 2]);
            Console.WriteLine("\nCurrent player: {0}", currentPlayer);
        }

        static void GetPlayerMove()
        {
            bool validMove = false;
            while (!validMove)
            {
                Console.Write("Enter row (0-2): ");
                int row = int.Parse(Console.ReadLine());
                Console.Write("Enter column (0-2): ");
                int col = int.Parse(Console.ReadLine());

                if (board[row, col] == ' ')
                {
                    board[row, col] = currentPlayer;
                    validMove = true;
                }
                else
                {
                    Console.WriteLine("That square is already taken. Try again.");
                }
            }
        }

        static void CheckWin()
        {
            // Проверяем строки
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != ' ')
                {
                    Console.WriteLine("{0} wins!", currentPlayer);
                    Environment.Exit(0);
                }
            }

            // Проверяем столбцы
            for (int i = 0; i < 3; i++)
            {
                if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != ' ')
                {
                    Console.WriteLine("{0} wins!", currentPlayer);
                    Environment.Exit(0);
                }
            }

            // Проверяем диагонали
            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != ' ')
            {
                Console.WriteLine("{0} wins!", currentPlayer);
                Environment.Exit(0);
            }
            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != ' ')
            {
                Console.WriteLine("{0} wins!", currentPlayer);
                Environment.Exit(0);
            }
        }

        static void CheckTie()
        {
            bool isTie = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        isTie = false;
                        break;
                    }
                }
            }

            if (isTie)
            {
                Console.WriteLine("It's a tie!");
                Environment.Exit(0);
            }
        }

        static void ChangePlayer()
        {
            if (currentPlayer == 'X')
            {
                currentPlayer = 'O';
            }
            else
            {
                currentPlayer = 'X';
            }
        }
    }
}