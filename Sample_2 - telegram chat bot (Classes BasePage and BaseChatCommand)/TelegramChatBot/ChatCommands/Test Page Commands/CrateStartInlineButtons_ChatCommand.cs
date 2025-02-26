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
    internal class CrateStartInlineButtons_ChatCommand : BaseChatCommand
    {
        public CrateStartInlineButtons_ChatCommand(TelegramBotClient client, Update update = null, string name = null) :
          base(client, update)
        {
            _name = string.IsNullOrEmpty(name) ? this.GetType().Name : name;
        }

        public override async Task ExecuteAsync()
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                             new List<InlineKeyboardButton[]>()
                             {
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithUrl("Кнопка №1: гиперссылка на сайт", "https://habr.com/"),
                                        },
                                        new InlineKeyboardButton[] { 
                                            InlineKeyboardButton.WithCallbackData("Кнопка №2: некоторое действие", "button_1"),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Кнопка №3: пример текста", "button_2"),
                                            InlineKeyboardButton.WithCallbackData("Кнопка №4: пример полноэкранного текста", "button_3"),
                                        },
                             });

            await BotClient.SendMessage(
                 Chat.Id,
                "Выбрана inline клавиатура!",
                replyMarkup: inlineKeyboard);
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
