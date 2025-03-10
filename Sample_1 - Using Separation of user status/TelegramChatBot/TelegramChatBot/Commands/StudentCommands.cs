using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Models;
using TelegramChatBot.Managers;
using System.Collections.Concurrent;
using TelegramChatBot.Global;

namespace TelegramChatBot.Commands
{
    public class StudentCommands : BaseCommands
    {
        public StudentCommands(UserSessionManager sessionManager) : base(sessionManager) 
        {
            _commands = new Dictionary<string, Func<long, ITelegramBotClient, Update, Task>>
            {
                { "/menu", VeiwMenuMessageCommandAsync }
            };
        }
        
        private async Task VeiwMenuMessageCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var t_chat = update.Message.Chat;

            var inlineKeyboard = new InlineKeyboardMarkup(
                            new List<InlineKeyboardButton[]>()
                            {

                                        new InlineKeyboardButton[] {
                                            InlineKeyboardButton.WithCallbackData("Кнопка 1", "button_1_student"),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Кнопка 2", "button_2_student"),
                                        },
                                        new InlineKeyboardButton[] {
                                            InlineKeyboardButton.WithCallbackData("Кнопка 3", "button_3_student"),
                                            InlineKeyboardButton.WithCallbackData("Кнопка 4", "button_4_student"),
                                        }
                            });

            await botClient.SendMessage(
                t_chat.Id,
                "Меню для студента:\n",
                replyMarkup: inlineKeyboard
                );
        }

    }
}
