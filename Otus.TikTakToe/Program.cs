
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Otus.TikTakToe
{
    internal class Program
    {
        private const string _botToken = "7017870435:AAH0bgGZeg8YCLvUQOhnJPGG_HiTvtsZUoY";

        private static TikTakToeGame _game;


        private static async Task Main()
        {
            using var cts = new CancellationTokenSource();

            var botClient = new TelegramBotClient(_botToken);
            
            var me = await botClient.GetMeAsync();
            var username = me.Username ?? "My Bot";
            Console.WriteLine($"My bot: {username}");


            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandleErrorAsync,
                receiverOptions: new ReceiverOptions()
                {
                    AllowedUpdates = Array.Empty<UpdateType>()
                
                },
                cancellationToken: cts.Token);

            Console.WriteLine($"Start listening for @{username}");

            Console.ReadKey();

            cts.Cancel();
        }

        private static Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine($"Error: {exception.Message}");
            return Task.CompletedTask;
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        await BotOnMessageReceived(client, update.Message);
                        break;

                    case UpdateType.CallbackQuery:
                        BotOnCallbackQuery(client, update.CallbackQuery);
                        break;
                }
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(client, exception, token);
            }
        }

        private static void BotOnCallbackQuery(ITelegramBotClient client, CallbackQuery? callbackQuery)
        {
            var text = callbackQuery.Data;

            if (text.Length == 1 && int.TryParse(text, out int move) && move >= 1 && move <= 9)
            {
                int row = (move - 1) / 3;
                int col = (move - 1) % 3;

                _game.MakeMove(row, col);
            }

            client.AnswerCallbackQueryAsync(callbackQuery.Id);
        }

        private static async Task BotOnMessageReceived(ITelegramBotClient client, Message message)
        {
            Console.WriteLine($"Receive message type: {message.Type}");

            if (message.Type != MessageType.Text)
                return;

            var action = message.Text!.Split(' ')[0];
            switch (action)
            {
                case "/start":
                    await StartGame(client, message);
                    break;
                default:
                    await Echo(client, message);
                    break;
            }

        }

        private static async Task Echo(ITelegramBotClient client, Telegram.Bot.Types.Message message)
        {
            await client.SendTextMessageAsync(chatId: message.Chat.Id, text: $"{message.Text}");
        }

        private static async Task StartGame(ITelegramBotClient client, Telegram.Bot.Types.Message message)
        {
            // var userName = $"{message.From.LastName} {message.From.FirstName}";
            // await client.SendTextMessageAsync(chatId: message.Chat.Id, text: $"Hello, {userName}");

           _game = new TikTakToeGame(client, message.Chat.Id);
           _game.InitializeBoard();

            var currentPlayer = _game.GetCurrentPlayer();

            await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                $"Player {currentPlayer}, your turn!",
                replyMarkup: _game.GetKeyboard());
        }
    }
}