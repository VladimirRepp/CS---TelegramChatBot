using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Types;
using Utils;

namespace TelegramChatBot.Global
{
    public class CatcherUserMessageInput
    {
        private static CatcherUserMessageInput INSTANCE;
        private static readonly object PADLOCK = new object();
        private ConcurrentDictionary<long, Func<long, ITelegramBotClient, Message, Task>> _userHandlerNames;

        public bool IsCatch;

        public static CatcherUserMessageInput Instance
        {
            get
            {
                lock (PADLOCK)
                {

                    if (INSTANCE == null)
                        INSTANCE = new();
                }

                return INSTANCE;
            }
        }

        private CatcherUserMessageInput() {
            _userHandlerNames = new ConcurrentDictionary<long, Func<long, ITelegramBotClient, Message, Task>>();
        }

        private void HandlerReset(long userId)
        {
            _userHandlerNames.TryRemove(userId, out var handler);

            if (_userHandlerNames.Count == 0)
                IsCatch = false;
        }

        public async Task<bool> TryInvokeHandler(long userId, ITelegramBotClient botClient, Message message)
        {
            if (_userHandlerNames.TryGetValue(userId, out var handler))
            {
                await handler.Invoke(userId, botClient, message);
                HandlerReset(userId);
            }

            return handler != null;
        }

        public void HandlerAdd(long userId, Func<long, ITelegramBotClient, Message, Task> handler)
        {
            _userHandlerNames.TryAdd(userId, handler);
            IsCatch = true;
        }
    }
}
