using Telegram.Bot;

namespace TelegramChatBot.Commands
{
    public abstract class BaseCommands
    {
        protected ITelegramBotClient _botClient;

        public ITelegramBotClient BotClient => _botClient;

        public BaseCommands(ITelegramBotClient client)
        {
            _botClient = client;
        }
    }
}
