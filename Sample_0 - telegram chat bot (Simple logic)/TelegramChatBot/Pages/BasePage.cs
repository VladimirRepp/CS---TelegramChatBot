using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.Pages
{
    public abstract class BasePage
    {
        protected int _id = 0;
        protected string _name = "";
        protected TelegramBotClient? _botClient = null;

        public int Id => _id;
        public string Name => _name;
        public TelegramBotClient? BotClient => _botClient;

        protected BasePage(int id, string name, TelegramBotClient client)
        {
            _id = id; 
            _name = name;
            _botClient = client;
        }

        protected BasePage(int id, TelegramBotClient client)
        {
            _id = id;
            _botClient = client;
        }

        /// <summary>
        /// Открыть страницу: вывести начальную информацию страницы
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// Закрыть страницу: закрыть все необходимые связи
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Закрыть страницу с переходом на следующую
        /// </summary>
        /// <param name="page"></param>
        public abstract void Close(BasePage page);

        /// <summary>
        /// Обработчик текстовых сообщений
        /// </summary>
        /// <param name="update"></param>
        public abstract Task OnMessageHandler(Update update);

        /// <summary>
        /// Обработчик inline команд
        /// </summary>
        /// <param name="update"></param>
        public abstract Task OnCallbackQueryHandler(Update update);
    }
}
