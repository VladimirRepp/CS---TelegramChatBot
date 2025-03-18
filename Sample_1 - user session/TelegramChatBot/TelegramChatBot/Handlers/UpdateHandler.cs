using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Managers;
using Utils;

namespace TelegramChatBot.Handlers
{
    public class UpdateHandler
    {
        private CommandHandlerManager? _commandHandlerManager = null;

        public UpdateHandler(CommandHandlerManager commandHandler)
        {
            _commandHandlerManager = commandHandler;
        }

        private async Task ProcessUserMessageRequest(long userId, ITelegramBotClient botClient, Update update)
        {
            var t_chat = update.Message.Chat;
            var t_user = update.Message.From;

            Logger.Instance.Log($"{t_user.FirstName} ({t_user.Id}) написал сообщение: {update.Message.Text}");

            await _commandHandlerManager.ExecuteCommandForMessageAsync(userId, botClient, update);
        }

        private async Task ProcessUserCallbackRequest(long userId, ITelegramBotClient botClient, Update update)
        {
            var callbackQuery = update.CallbackQuery;
            await botClient.AnswerCallbackQuery(callbackQuery.Id);

            var t_user = callbackQuery.From;
            var callbackData = callbackQuery.Data;

            Logger.Instance.Log($"{t_user.Username} ({t_user.Id}) нажал кнопку: {callbackData}");

            await _commandHandlerManager.ExecuteCommandForCallbackAsync(userId, botClient, update);
        }

        public async Task StartupAsync(ITelegramBotClient botClient)
        {
            await _commandHandlerManager.CreateMenuCommands(botClient);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            Chat? t_chat = null;

            try
            {
                if (update.Message is not null)
                {
                    long userId = update.Message.From.Id;
                    t_chat = update.Message.Chat;

                    await Task.Run(() => ProcessUserMessageRequest(userId, botClient, update));
                }
                else if (update.CallbackQuery is not null)
                {
                    long userId = update.CallbackQuery.From.Id; 
                    t_chat = update.CallbackQuery.Message.Chat;

                    await Task.Run(() => ProcessUserCallbackRequest(userId, botClient, update));
                }
                else
                {
                    t_chat = update.Message.Chat;
                    await _commandHandlerManager.UnrecognizedMessage(botClient, t_chat);
                }
            }
            catch (Exception ex) 
            {
                Logger.Instance.Log($"UpdateHandler.HandleUpdateAsync(Exception): {ex.Message}", LogLevel.Error);

                if (t_chat == null)
                    return;

                await botClient.SendMessage(
                    t_chat.Id,
                    "Возникла ошибка на сервере ..."
                    );
            }
        }
    }
}
