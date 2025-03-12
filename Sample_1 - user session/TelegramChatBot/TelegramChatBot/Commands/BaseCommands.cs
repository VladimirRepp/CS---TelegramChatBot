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

        protected virtual async Task ViewHelpMessageCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var t_chat = update.Message.Chat;
            await botClient.SendMessage(
                t_chat.Id,
                "Данный чат-бот является тестовым!\n" +
                "При написании кода и базовой архитектруры старались учитываться разные аспекты. " +
                "Наприме, чтобы удобно можно было совершать поддержку, отладку и расширений функционала.\n" +
                "Чат-бот умеет обрабатывать введенный тобой текст и реагировать на него так, " +
                "как прописано в коде, все удобно и лаконично.\n" +
                "Для проверки возможностей чат-бота потыкай на кнопочки.\n" +
                "PS: функционал простой, скайнет твои данные не отравляем, код весь открыт, так что " +
                "добавь свой токен и поехали ..."
                );
        }

        protected async Task DeleteTimerMessage(int waitMilliseconds, ITelegramBotClient botClient, long chatId, int messageId)
        {
          await Task.Delay(waitMilliseconds);
          await botClient.DeleteMessage(chatId, messageId);
        }
        
        public virtual async Task ExecuteMessageCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
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
