using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Managers;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramChatBot.Global;

namespace TelegramChatBot.Commands
{
    public class CommonCommands : BaseCommands
    {
        public CommonCommands(UserSessionManager sessionManager) : base(sessionManager)
        {
            _commands = null;
        }

        public async Task CreateMenuCommandsAsync(ITelegramBotClient botClient)
        {
            var bot_commands = new[]
            {
                new BotCommand { Command = "menu", Description = "Показать меню" },
                new BotCommand { Command = "help", Description = "Помощь" }
            };

            await botClient.SetMyCommands(bot_commands);
        }

        public async Task AnUnrecognizedUserCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var t_chat = update.Message.Chat;
            await botClient.SendMessage(
                t_chat.Id,
                "Ошибка: ваша роля и текущий статус не распознан ...\n"
                );
        }
    }
}
