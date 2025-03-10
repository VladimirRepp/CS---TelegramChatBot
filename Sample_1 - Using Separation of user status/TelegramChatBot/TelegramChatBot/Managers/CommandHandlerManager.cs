using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChatBot.Commands;
using TelegramChatBot.Models;

namespace TelegramChatBot.Managers
{
    public class CommandHandlerManager
    {
        private readonly UserSessionManager _sessionManager;

        private readonly CommonCommands _commonCommands;
        private readonly LoginCommands _loginCommands;
        private readonly StudentCommands _studentCommands;
        private readonly TeacherCommands _teacherCommands;

        private bool _isCatchUserInput = false;

        public CommandHandlerManager(UserSessionManager sessionManager)
        {
            _sessionManager = sessionManager;

            _commonCommands = new(sessionManager);
            _loginCommands = new(sessionManager);
            _studentCommands = new(sessionManager);
            _teacherCommands = new(sessionManager);
        }

        private async Task CatchUserInput(Message message)
        {
            _isCatchUserInput = false;

            // todo: do for catch message
        }

        public async Task ExecuteCommandForMessageAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var message = update.Message;
            if (message == null)
                return;

            if (_isCatchUserInput)
            {
                await CatchUserInput(message);
                return;
            }

            var userSession = _sessionManager.GetOrCreateSession(userId);

            if(userSession.Role == "Guest")
            {
                await _loginCommands.ExecuteMessageCommandAsync(userId, botClient, update);
            }
            else if (userSession.Role == "Teacher")
            {
                await _teacherCommands.ExecuteMessageCommandAsync(userId, botClient, update);
            }
            else if (userSession.Role == "Student")
            {
               await _studentCommands.ExecuteMessageCommandAsync(userId, botClient, update);
            }
            else
            {
                return;
            }
        }

        public async Task ExecuteCommandForCallbackAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var callback = update.CallbackQuery;
            if (callback == null)
                return;

            var userSession = _sessionManager.GetOrCreateSession(userId);

            if (userSession.Role == "Guest")
            {
                await _loginCommands.ExecuteCallbackCommandAsync(userId, botClient, update);
            }
            else if (userSession.Role == "Teacher")
            {
                await _teacherCommands.ExecuteCallbackCommandAsync(userId, botClient, update);
            }
            else if (userSession.Role == "Student")
            {
                await _studentCommands.ExecuteCallbackCommandAsync(userId, botClient, update);
            }
            else
            {
                return;
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
