using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;

namespace Otus.TikTakToe
{
    internal class TikTakToeGame
    {
        static char[,] board = new char[3, 3]; // Игровое поле
        static char currentPlayer = 'X'; // Текущий игрок


        private ITelegramBotClient _botClient;
        private long _chatId;


        public TikTakToeGame(ITelegramBotClient botClient, long chatId)
        {
            this._botClient = botClient;
            this._chatId = chatId;
        }


        internal void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }

        internal char GetCurrentPlayer()
        {
            return currentPlayer;
        }


        internal bool CheckWin()
        {
            // Проверяем строки
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != ' ')
                {
                    return true;
                }
            }

            // Проверяем столбцы
            for (int i = 0; i < 3; i++)
            {
                if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != ' ')
                {
                    return true;
                }
            }

            // Проверяем диагонали
            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != ' ')
            {
                return true;
            }
            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != ' ')
            {
                return true;
            }

            return false;
        }

        internal bool CheckTie()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        internal void ChangePlayer()
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

        internal async Task MakeMove(int row, int col)
        {
            if (board[row, col] == ' ')
            {
                board[row, col] = currentPlayer;

                if (CheckWin())
                {
                    await _botClient.SendTextMessageAsync(_chatId, $"Player {currentPlayer} wins!", replyMarkup: GetKeyboard());
                    InitializeBoard();
                }
                else if (CheckTie())
                {
                    await _botClient.SendTextMessageAsync(_chatId, "It's a tie!", replyMarkup: GetKeyboard());
                    InitializeBoard();
                }
                else
                {
                    ChangePlayer();
                    await _botClient.SendTextMessageAsync(_chatId, $"Player {currentPlayer}, your turn!", replyMarkup: GetKeyboard());
                }
            }
            else
            {
                await _botClient.SendTextMessageAsync(_chatId, "That square is already taken. Try again.", replyMarkup: GetKeyboard());
            }
        }

        internal InlineKeyboardMarkup GetKeyboard()
        {
            InlineKeyboardButton[][] buttons = new InlineKeyboardButton[3][];
            for (int i = 0; i < 3; i++)
            {
                buttons[i] = new InlineKeyboardButton[3];
                for (int j = 0; j < 3; j++)
                {
                    int move = i * 3 + j + 1;
                    buttons[i][j] = InlineKeyboardButton.WithCallbackData(board[i, j] == ' ' ? move.ToString() : board[i, j].ToString(), move.ToString());
                }
            }
            return new InlineKeyboardMarkup(buttons);
        }

    }
}
