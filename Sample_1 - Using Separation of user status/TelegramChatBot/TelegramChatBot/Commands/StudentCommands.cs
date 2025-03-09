using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Models;
using TelegramChatBot.Managers;

namespace TelegramChatBot.Commands
{
    public class StudentCommands : BaseCommands
    {
        public StudentCommands(ITelegramBotClient client) : base(client) { }

        public async Task VeiwMenu(Chat chat)
        {
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

            await _botClient.SendMessage(
                chat.Id,
                "Меню для студента:\n",
                replyMarkup: inlineKeyboard
                );
        }

        public async Task Login(long userId, UserSessionManager sessionManager, Chat chat)
        {
            UserSession session = sessionManager.GetOrCreateSession(userId);
            session.Role = "Student";
            session.CurrentState = "MainMenu";

            await _botClient.SendMessage(
               chat.Id,
               $"Вы вошли как студент!\n" +
               $"Для просмотра доступных действий вызовите команду \"/menu\" ..."
               );
        }
    }
}
