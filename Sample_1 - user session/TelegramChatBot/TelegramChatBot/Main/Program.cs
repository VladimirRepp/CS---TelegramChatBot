using System.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramChatBot.Handlers;
using TelegramChatBot.Managers;
using Utils;

namespace TelegramChatBot.Main
{
    internal class Program
    {
        private static UpdateHandler _updateHandelr;
        private static ErrorHandler _errorHandelr;
        private static TelegramBotClient _botClient;

        private static UserSessionManager _userSessionManager;
        private static CommandHandlerManager _commandHandler;

        static async Task Main(string[] args)
        {
            Startup();

            var me = await _botClient.GetMe();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery },
                DropPendingUpdates = true
            };
            var cts = new CancellationTokenSource();

            try
            {
                _botClient.StartReceiving(_updateHandelr.HandleUpdateAsync, _errorHandelr.HandleErrorAsync, receiverOptions, cts.Token);
                await _updateHandelr.StartupAsync(_botClient);

                Logger.Instance.Log($"Бот {me.Username} запущен ...");
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(ex.Message, LogLevel.Error);
            }

            while (!ConsoleHelper.IsExit()) ;
            Exiting();
        }

        private static void Startup()
        {
            string token = ConfigurationManager.ConnectionStrings["TelegramBotToken"].ConnectionString;
            _botClient = new(token);

            _userSessionManager = new();
            _commandHandler = new(_userSessionManager);

            _updateHandelr = new(_commandHandler);
            _errorHandelr = new();
        }

        private static void Exiting()
        {
            Console.WriteLine("Command: stop called");
            Console.WriteLine("Startup exit ...");

            // do for needed for exit 

            Console.WriteLine("Exited");
        }
    }
}
