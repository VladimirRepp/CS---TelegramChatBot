using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramChatBot.ChatCommands.Test_Page_Commands
{
    internal class CreateStartReplyKeyboard_ChatCommand : BaseChatCommand
    {
        public CreateStartReplyKeyboard_ChatCommand(TelegramBotClient client, Update update = null, string name = null) :
         base(client, update)
        {
            _name = string.IsNullOrEmpty(name) ? this.GetType().Name : name;
        }

        public override async Task ExecuteAsync()
        {
            var replyKeyboard = new ReplyKeyboardMarkup(
                             new List<KeyboardButton[]>()
                             {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("/start"),
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("/inline"),
                                            new KeyboardButton("/reply")
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Привет, чат-бот!"),
                                            new KeyboardButton("Пока, чат-бот!")
                                        }
                             })
            {
                ResizeKeyboard = true,
            };

            await BotClient.SendMessage(
                Chat.Id,
                "Выбрана reply клавиатура!",
                replyMarkup: replyKeyboard);
        }

        public override async Task ExecuteAsync(Update update)
        {
            _update = update;
            _chat = update.Message.Chat;

            await ExecuteAsync();
        }

        public override Task UndoAsync()
        {
            throw new NotImplementedException();
        }
    }
}
