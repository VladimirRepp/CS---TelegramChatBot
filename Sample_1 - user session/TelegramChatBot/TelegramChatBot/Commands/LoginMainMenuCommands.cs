using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Managers;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramChatBot.Global;
using TelegramChatBot.Models;

namespace TelegramChatBot.Commands
{
    public class LoginMainMenuCommands : BaseCommands
    {
        public LoginMainMenuCommands(UserSessionManager sessionManager) : base(sessionManager)
        {
            _commands = new Dictionary<string, Func<long, ITelegramBotClient, Update, Task>>
            {
                { CommandNames.START, ViewLoginMenuMessageCommandAsync },
                { CommandNames.MENU, ViewLoginMenuMessageCommandAsync },
                { CommandNames.HELP, ViewHelpMessageCommandAsync },
                { ButtonNames.STUDENT_LOGIN, StudentLoginCallbackCommandAsync },
                { ButtonNames.TEACHER_LOGIN, TeacherLoginCallbackCommandAsync  }
            };
        }

        private async Task ViewLoginMenuMessageCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var t_chat = update.Message.Chat;

            var inlineKeyboard = new InlineKeyboardMarkup(
                          new List<InlineKeyboardButton[]>()
                          {

                                        new InlineKeyboardButton[] {
                                            InlineKeyboardButton.WithCallbackData("Войти как студент", ButtonNames.STUDENT_LOGIN),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Войти как преподаватель", ButtonNames.TEACHER_LOGIN),
                                        }
                          });

            await botClient.SendMessage(
                t_chat.Id,
                "Добро пожаловать в тестовый чат-бот!\n" +
                "Выберите один из режимов входа ...\n" +
                "Меню для входа:\n",
                replyMarkup: inlineKeyboard
                );
        }

        private async Task StudentLoginCallbackCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var t_chat = update.CallbackQuery.Message.Chat;

            UserSession session = _sessionManager.GetOrCreateSession(userId);
            session.Role = RoleNames.STUDENT;
            session.CurrentState = CurrentStateNames.MAIN_MENU;

            await botClient.SendMessage(
               t_chat.Id,
               $"Вы вошли как студент!\n" +
               $"Для просмотра доступных действий вызовите команду \"/menu\" ..."
               );
        }

        public async Task TeacherLoginCallbackCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var t_chat = update.CallbackQuery.Message.Chat;

            UserSession session = _sessionManager.GetOrCreateSession(userId);
            session.Role = RoleNames.TEACHER;
            session.CurrentState = CurrentStateNames.MAIN_MENU;

            await botClient.SendMessage(
              t_chat.Id,
              $"Вы вошли как преподаватель!\n" +
              $"Для просмотра доступных действий вызовите команду \"/menu\" ..."
              );
        }
    }
}
