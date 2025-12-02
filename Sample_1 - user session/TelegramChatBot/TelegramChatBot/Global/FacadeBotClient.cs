using Telegram.Bot;
namespace ChatBot.Global
{
    /// <summary>
    /// Так идет жесткая зависимость - ПЛОХО <br/>
    /// Фасад для глобального доступа к клиенту <br/>
    /// Паттерны: фасад + одинчка 
    /// </summary>
    internal static class FacadeBotClient
    {
        private static TelegramBotClient INSTANCE;
      
        private static object _padlockStartup = new();
        private static object _padlockGetInstance = new();

        /// <summary>
        /// Получить экземпляр 
        /// </summary>
        public static TelegramBotClient GetInstance
        {
            get
            {
                lock (_padlockGetInstance)
                {
                    return INSTANCE; 
                }
            }
        }

        /// <summary>
        /// Инициализация TelegramBotClient <br/>
        /// TelegramBotClient - может быть только в одном экземпляре 
        /// </summary>
        /// <param name="token"></param>
        /// <exception cref="Exception"></exception>
        public static void Startup(string token)
        {
            if (IsInitialated())
                throw new Exception("BotClient.Startup: INSTANCE already initialized!");

            lock (_padlockStartup)
            {
                INSTANCE = new(token);
            }
        }

        /// <summary>
        /// Проверка на инициализацию 
        /// </summary>
        /// <returns></returns>
        public static bool IsInitialated() => INSTANCE != null;
    }
}
