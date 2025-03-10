using System.Collections.Concurrent;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Managers;
using System.Windows.Input;

namespace TelegramChatBot.Commands
{
    public abstract class BaseCommands
    {
        protected readonly UserSessionManager _sessionManager;
        protected Dictionary<string, Func<long, ITelegramBotClient, Update, Task>> _commands = null;

        public BaseCommands(UserSessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public virtual async Task ExecuteMessageCommandAsync(long userId, ITelegramBotClient botClient, Update update){
            var messaage = update.Message;

            if (_commands.TryGetValue(messaage.Text, out var command))
            {
                await command(userId, botClient, update);
            }
            else
            {
                var t_chat = update.Message.Chat;
                await UnrecognizedMessage(botClient, t_chat);
            }
        }

        public virtual async Task ExecuteCallbackCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var callback = update.CallbackQuery;

            if (_commands.TryGetValue(callback.Data, out var command))
            {
                await command(userId, botClient, update);
            }
            else
            {
                var t_chat = update.CallbackQuery.Message.Chat;
                await UnrecognizedMessage(botClient, t_chat);
            }
        }

        public async Task UnrecognizedMessage(ITelegramBotClient botClient, Chat chat)
        {
            await botClient.SendMessage(
                  chat.Id,
                  "Нераспознанный ввод!\n" +
                  "Попробуйте другую команду или текст сообщения ..."
                  );
        }
    }
}
