using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramChatBot.Commands;
using TelegramChatBot.Models;

namespace TelegramChatBot.Managers
{
    /// <summary>
    /// Not Using - old version
    /// </summary>
    public class OLD_CommandsManager
    {
        private ITelegramBotClient _botClient;
        private UserSessionManager _sessionManager;
        private TeacherCommands _teacherCommands;
        private StudentCommands _studentCommands;

        private bool _isCatchUserInput = false;

        public OLD_CommandsManager(ITelegramBotClient botClient, UserSessionManager sessionManager)
        {
            _botClient = botClient;
            _sessionManager = sessionManager;

            //_teacherCommands = new(_botClient);
            //_studentCommands = new(_botClient);
        }

        private async Task ViewLoginMenu(Chat chat)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(
                          new List<InlineKeyboardButton[]>()
                          {

                                        new InlineKeyboardButton[] {
                                            InlineKeyboardButton.WithCallbackData("Войти как студент", "button_student_login"),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Войти как преподаватель", "button_teacher_login"),
                                        }
                          });

            await _botClient.SendMessage(
                chat.Id,
                "Меню для входа:\n",
                replyMarkup: inlineKeyboard
                );
        }

        private async Task UnrecognizedMessage(Chat chat)
        {
            await _botClient.SendMessage(
                  chat.Id,
                  "Нераспознанный ввод!\n" +
                  "Попробуйте другую команду или текст сообщения ..."
                  );
        }

        private async Task CatchUserInput(Message message)
        {
            _isCatchUserInput = false;

            // todo: do some
        }

        public async Task CreateMenuCommands()
        {
            var bot_commands = new[]
            {
                new BotCommand { Command = "menu", Description = "Показать меню" },
                new BotCommand { Command = "help", Description = "Помощь" }
            };

            await _botClient.SetMyCommands(bot_commands);
        }

        public async Task CallbackRequest(long userId, CallbackQuery callback)
        {
            var t_chat = callback.Message.Chat;
            var callbackData = callback.Data;

            if (callbackData == "button_student_login")
            {
                //await _studentCommands.LoginCommandAsync(userId, _sessionManager, t_chat);
            }
            else if (callbackData == "button_teacher_login")
            {
                //await _teacherCommands.LoginCommandAsync(userId, _sessionManager, t_chat);
            }
        }

        public async Task MessageRequest(long userId, Message message)
        {
            UserSession session = _sessionManager.GetOrCreateSession(userId);

            var t_chat = message.Chat;
            var t_user = message.From;
            var messageText = message.Text;

            if (_isCatchUserInput)
            {
                await CatchUserInput(message);
                return;
            }

            if (messageText.ToLower() == "/start")
            {
                await _botClient.SendMessage(
                    t_chat.Id,
                    "Добро пожаловать в чат-бот!\n" +
                    "Для просмотра доступных действий вызовите команду \"/menu\" ..."
                    );
            }
            else if (session.Role == "Guest" && messageText.ToLower() == "/menu")
            {
                await ViewLoginMenu(t_chat);
            }
            else if (session.CurrentState == "MainMenu" && session.Role == "Teacher" &&
                messageText.ToLower() == "/menu")
            {
                //await _teacherCommands.VeiwMenuMessageCommandAsync(t_chat);
            }
            else if (session.CurrentState == "MainMenu" && session.Role == "Student" &&
                messageText.ToLower() == "/menu")
            {
                //await _studentCommands.VeiwMenuMessageCommandAsync(t_chat);
            }
            else
            {
                await UnrecognizedMessage(t_chat);
            }
        }
    }
}
