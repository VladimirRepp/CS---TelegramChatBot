using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.Pages
{
    public interface IPage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TelegramBotClient BotClient { get; set; }

        /// <summary>
        /// Вывести начальную информацию страницы
        /// </summary>
        public void Open();

        /// <summary>
        /// Закрыть страницу 
        /// </summary>
        public void Close();

        /// <summary>
        /// Закрыть страницу с переходом на следующую
        /// </summary>
        /// <param name="page"></param>
        public void Close(IPage page);

        /// <summary>
        /// Обработчик текстовых сообщений
        /// </summary>
        /// <param name="update"></param>
        public Task OnMessageHandler(Update update);

        /// <summary>
        /// Обработчик inline команд
        /// </summary>
        /// <param name="update"></param>
        public Task OnCallbackQueryHandler(Update update);
    }
}
