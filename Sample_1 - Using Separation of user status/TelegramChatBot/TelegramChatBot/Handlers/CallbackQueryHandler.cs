using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Managers;

namespace TelegramChatBot.Handlers
{
    public class CallbackQueryHandler
    {
        private readonly CommandHandlerManager _commandHandler;

        public CallbackQueryHandler(CommandHandlerManager commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task HandleCallbackAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            string data = update.CallbackQuery.Data;

            if (data.StartsWith("button_"))
            {
                await _commandHandler.ExecuteCommandForCallbackAsync(userId, botClient, update);
            }
        }
    }
}
