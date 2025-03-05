using System.Configuration;
using TelegramChatBot.Pages;
using TelegramChatBot.Utils;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramChatBot
{
    internal class Program
    {
        #region === Evens For StartReceiving ===
        public delegate Task OnMessageHandlerDelegate(Update update);
        public delegate Task OnCallbackQueryDelegate(Update update);
        public static OnMessageHandlerDelegate? OnMessageHandlerEvent;
        public static OnMessageHandlerDelegate? OnCallbackQueryEvent;
        #endregion

        private static TelegramBotClient _botClient;
        private static TestPage _testPage;

        private static async Task Main(string[] args)
        {
            string token = ConfigurationManager.ConnectionStrings["TelegramBotToken"].ConnectionString;
            _botClient = new(token);

            var me = await _botClient.GetMe();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery },
                DropPendingUpdates = true
            };
            var cts = new CancellationTokenSource();
            _botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cts.Token);
        
            Console.WriteLine($"Бот {me.Username} запущен ...");
        
            // === Testing ===
            _testPage = new TestPage(0, _botClient);
            _testPage.Open();
            // ===============

            Console.WriteLine($"Бот {me.Username} запущен ...");

            while (!ConsoleHelper.IsExit()) ;
            Exiting();
        }

        private static async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        OnMessageHandlerEvent?.Invoke(update);
                        break;

                    case UpdateType.CallbackQuery:
                        OnCallbackQueryEvent?.Invoke(update);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"UpdateHandler(Exception): {ex.Message}");
            }
        }

        private static async Task ErrorHandler(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
        {
            try
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine($"ErrorHandler: {ErrorMessage}");
            }
            catch (Exception ex)
            {
                throw new Exception($"ErrorHandler(Exception): {ex.Message}");
            }
        }

        private static void Exiting()
        {
            Console.WriteLine("Command: stop called");
            Console.WriteLine("Startup exit ...");

            // todo for exit
            _testPage.Close();

            Console.WriteLine("Exited");
        }
    }
}
