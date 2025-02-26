using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramChatBot.ChatCommands
{
    public abstract class BaseChatCommand
    {
        protected string _name = "";
        protected string[] _availableInputs = null;
        protected Update? _update = null;
        protected TelegramBotClient? _botClient = null;
        protected Chat? _chat = null;

        public string Name { get => _name; set => _name = value; }
        public Update? Update { get => _update; set => _update = value; }
        public Chat? Chat { get => _chat; protected set => _chat = value; }
        public TelegramBotClient? BotClient { get => _botClient; set => _botClient = value; }
        
        public BaseChatCommand(string name, TelegramBotClient client, Update update = null)
        {
            _name = name;
            _botClient = client;
            _update = update;

            _chat = _update?.Message.Chat;
        }

        public BaseChatCommand(TelegramBotClient client, Update update = null)
        {
            _botClient = client;
            _update = update;

            _chat = _update?.Message.Chat;
        }

        /// <summary>
        /// Задать список доступного ввода для проверки вызова действия
        /// </summary>
        /// <param name="param"></param>
        public virtual void SetAvailableInputs(params string[] new_params)
        {
            _availableInputs = new string[new_params.Length];

            int i = 0;
            foreach (var param in new_params) {
                _availableInputs[i++] = param;
            }
        }

        /// <summary>
        /// Асинхронная проверка введенной строковой команды на наличие в списке доступных команд для ввода
        /// </summary>
        /// <param name="inputed_command"></param>
        /// <returns></returns>
        public virtual async Task<bool> CheckAsync(string inputed_command)
        {
            Task<bool> task = Task.Run(() => Check(inputed_command));
            await task;
            bool isFound = task.Result;
            return isFound;
        }

        /// <summary>
        /// Проверка введенной строковой команды на наличие в списке доступных команд для ввода
        /// </summary>
        /// <param name="inputed_command"></param>
        /// <returns></returns>
        public virtual bool Check(string inputed_command)
        {
            return _availableInputs.Contains(inputed_command);
        }

        public virtual void SetParams(params object[] my_params) { }

        public abstract Task ExecuteAsync();
        public abstract Task ExecuteAsync(Update update);

        public abstract Task UndoAsync();
    }
}
