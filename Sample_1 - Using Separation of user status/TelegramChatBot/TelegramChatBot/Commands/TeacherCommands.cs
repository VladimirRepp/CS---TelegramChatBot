using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Managers;
using TelegramChatBot.Models;

namespace TelegramChatBot.Commands
{
    public class TeacherCommands : BaseCommands
    {
        public TeacherCommands(ITelegramBotClient client) : base(client) { }

        public async Task VeiwMenu(Chat chat)
        {
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

            await _botClient.SendMessage(
                chat.Id,
                "Меню для преподавателя:\n",
                replyMarkup: inlineKeyboard
                );
        }

        public async Task Login(long userId, UserSessionManager sessionManager, Chat chat)
        {
            UserSession session = sessionManager.GetOrCreateSession(userId);
            session.Role = "Teacher";
            session.CurrentState = "MainMenu";

            await _botClient.SendMessage(
              chat.Id,
              $"Вы вошли как преподаватель!\n" +
              $"Для просмотра доступных действий вызовите команду \"/menu\" ..."
              );
        }
    }
}
