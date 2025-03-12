using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChatBot.Commands;
using TelegramChatBot.Global;

namespace TelegramChatBot.Managers
{
    public class CommandHandlerManager
    {
        private readonly UserSessionManager _sessionManager;

        private readonly CommonCommands _commonCommands;
        private readonly LoginMainMenuCommands _loginMainMenuCommands;
        private readonly StudentMainMenuCommands _studentMainMenuCommands;
        private readonly TeacherMainMenuCommands _teacherMainMenuCommands;

        public CommandHandlerManager(UserSessionManager sessionManager)
        {
            _sessionManager = sessionManager;

            _commonCommands = new(sessionManager);
            _loginMainMenuCommands = new(sessionManager);
            _studentMainMenuCommands = new(sessionManager);
            _teacherMainMenuCommands = new(sessionManager);
        }

        private async Task<bool> TryCatchUserInput(long userId, ITelegramBotClient botClient, Message message)
        {
            return await CatcherUserInput.Instance.TryInvokeHandler(userId, botClient, message); 
        }

        public async Task ExecuteCommandForMessageAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var message = update.Message;
            if (message == null)
            {
                var t_chat = message.Chat;
                await _commonCommands.UnrecognizedMessage(botClient, t_chat);
                return;
            }

            if (CatcherUserInput.Instance.IsCatch)
            {
               if(await TryCatchUserInput(userId, botClient, message))
                    return;
            }

            var userSession = _sessionManager.GetOrCreateSession(userId);

            if(userSession.Role == RoleNames.GUEST && userSession.CurrentState == CurrentStateNames.START)
            {
                await _loginMainMenuCommands.ExecuteMessageCommandAsync(userId, botClient, update);
            }
            else if (userSession.Role == RoleNames.TEACHER && userSession.CurrentState == CurrentStateNames.MAIN_MENU)
            {
                await _teacherMainMenuCommands.ExecuteMessageCommandAsync(userId, botClient, update);
            }
            else if (userSession.Role == RoleNames.STUDENT && userSession.CurrentState == CurrentStateNames.MAIN_MENU)
            {
               await _studentMainMenuCommands.ExecuteMessageCommandAsync(userId, botClient, update);
            }
            else
            {
                await _commonCommands.AnUnrecognizedUserCommandAsync(userId, botClient, update);
            }
        }

        public async Task ExecuteCommandForCallbackAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var callback = update.CallbackQuery;
            if (callback == null)
                return;

            string data = callback.Data;
            if (!data.StartsWith(ButtonNames.START_NAME_BUTTON))
            {
                var t_chat = callback.Message.Chat;
                await _commonCommands.UnrecognizedMessage(botClient, t_chat);
                return;
            }

            var userSession = _sessionManager.GetOrCreateSession(userId);

            if (userSession.Role == RoleNames.GUEST && userSession.CurrentState == CurrentStateNames.START)
            {
                await _loginMainMenuCommands.ExecuteCallbackCommandAsync(userId, botClient, update);
            }
            else if (userSession.Role == RoleNames.TEACHER && userSession.CurrentState == CurrentStateNames.MAIN_MENU)
            {
                await _teacherMainMenuCommands.ExecuteCallbackCommandAsync(userId, botClient, update);
            }
            else if (userSession.Role == RoleNames.STUDENT && userSession.CurrentState == CurrentStateNames.MAIN_MENU)
            {
                await _studentMainMenuCommands.ExecuteCallbackCommandAsync(userId, botClient, update);
            }
            else
            {
                await _commonCommands.AnUnrecognizedUserCommandAsync(userId, botClient, update);
            }
        }

        public async Task CreateMenuCommands(ITelegramBotClient botClient)
        {
            await _commonCommands.CreateMenuCommandsAsync(botClient);
        }

        internal async Task UnrecognizedMessage(ITelegramBotClient botClient, Chat t_chat)
        {
            await _commonCommands.UnrecognizedMessage(botClient, t_chat);
        }
    }
}
