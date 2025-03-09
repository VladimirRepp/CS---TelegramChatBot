using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Managers;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Utils;

namespace TelegramChatBot.Services
{
    public class TelegramBotService
    {
        private readonly UserSessionManager _sessionManager;
        private CommandsManager _commandsManager = null;

        private Update? _update = null;
        private ITelegramBotClient? _botClient = null;

        public TelegramBotService(ITelegramBotClient client)
        {
            _botClient = client;

            _sessionManager = new();
            _commandsManager = new(_botClient, _sessionManager);
        }

        private async Task ProcessUserMessageRequest(long userId, Message message)
        {
            var t_chat = _update.Message.Chat;
            var t_user = _update.Message.From;

            Logger.Instance.Log($"{t_user.FirstName} ({t_user.Id}) написал сообщение: {message.Text}");

            await _commandsManager.MessageRequest(userId, message);
        }

        private async Task ProcessUserCallbackRequest(long userId, CallbackQuery callbackQuery)
        {
            await _botClient.AnswerCallbackQuery(callbackQuery.Id);

            var t_user = callbackQuery.From;
            var callbackData = callbackQuery.Data;

            Logger.Instance.Log($"{t_user.Username} ({t_user.Id}) нажал кнопку: {callbackData}");

            await _commandsManager.CallbackRequest(userId, callbackQuery);
        }

        public async Task StartupAsync()
        {
            await _commandsManager.CreateMenuCommands();
        }

        public async Task ErrorHandlerAsync(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
        {
            try
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Logger.Instance.Log(ErrorMessage, LogLevel.Error); 
            }
            catch (Exception ex)
            {
                throw new Exception($"ErrorHandler(Exception): {ex.Message}");
            }
        }

        public async Task UpdateHandlerAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {
            _update = update; 

            switch (update.Type)
            {
                case UpdateType.Message:
                    if (update.Message is { } message)
                    {
                        long userId = message.From.Id;
                        await Task.Run(() => ProcessUserMessageRequest(userId, message));
                    }
                    break;

                case UpdateType.CallbackQuery:
                    if (update.CallbackQuery is { } callback)
                    {
                        long userId = callback.From.Id;
                        await Task.Run(() => ProcessUserCallbackRequest(userId, callback));
                    }
                    break;    
            }
        }
    }
}
