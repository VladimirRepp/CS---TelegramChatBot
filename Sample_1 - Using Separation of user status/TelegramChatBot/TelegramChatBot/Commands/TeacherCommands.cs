using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Managers;
using TelegramChatBot.Models;
using TelegramChatBot.Global;

namespace TelegramChatBot.Commands
{
    public class TeacherCommands : BaseCommands
    {
        public TeacherCommands(UserSessionManager sessionManager) : base(sessionManager) 
        {
            _commands = new Dictionary<string, Func<long, ITelegramBotClient, Update, Task>>
            {
                { "/menu", VeiwMenuMessageCommandAsync }
            };
        }

        public async Task VeiwMenuMessageCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var t_chat = update.Message.Chat;

            var inlineKeyboard = new InlineKeyboardMarkup(
                            new List<InlineKeyboardButton[]>()
                            {

                                        new InlineKeyboardButton[] {
                                            InlineKeyboardButton.WithCallbackData("Кнопка 1", "button_1_teacher"),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Кнопка 2", "button_2_teacher"),
                                            InlineKeyboardButton.WithCallbackData("Кнопка 3", "button_3_teacher")
                                        },
                                        new InlineKeyboardButton[] {
                                            InlineKeyboardButton.WithCallbackData("Кнопка 4", "button_4_teacher"),
                                        }
                            });

            await botClient.SendMessage(
                t_chat.Id,
                "Меню для преподавателя:\n",
                replyMarkup: inlineKeyboard
                );
        }
    }
}
