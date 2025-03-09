using System.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramChatBot.Managers;
using TelegramChatBot.Services;
using Utils;

namespace TelegramChatBot
{
    internal class Program
    {
        private static TelegramBotService _botService;
        private static TelegramBotClient _botClient;

        static async Task Main(string[] args)
        {
            string token = ConfigurationManager.ConnectionStrings["TelegramBotToken"].ConnectionString;
            _botClient = new(token);
            _botService = new(_botClient);

            var me = await _botClient.GetMe();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery },
                DropPendingUpdates = true
            };
            var cts = new CancellationTokenSource();

            try
            {
                _botClient.StartReceiving(_botService.UpdateHandlerAsync, _botService.ErrorHandlerAsync, receiverOptions, cts.Token);
                await _botService.StartupAsync();

                Logger.Instance.Log($"Бот {me.Username} запущен ...");
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex.Message, LogLevel.Error);
            }

            while (!ConsoleHelper.IsExit());
            Exiting();
        }

        private static void Exiting()
        {
            Console.WriteLine("Command: stop called");
            Console.WriteLine("Startup exit ...");

            // todo for exit

            Console.WriteLine("Exited");
        }
    }
}
