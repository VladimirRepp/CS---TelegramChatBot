using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChatBot.Managers;

namespace TelegramChatBot.Handlers
{
    public class MessageHandler
    {
        private readonly CommandHandlerManager _commandHandler;

        public MessageHandler(CommandHandlerManager commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task HandleMessageAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            if(update.Message != null)
            {
                await _commandHandler.ExecuteCommandForMessageAsync(userId, botClient, update);
            }
        }
    }
}
