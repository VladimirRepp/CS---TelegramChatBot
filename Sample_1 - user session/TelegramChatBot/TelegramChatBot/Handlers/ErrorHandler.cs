using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Utils;

namespace TelegramChatBot.Handlers
{
    public class ErrorHandler
    {
        public async Task HandleErrorAsync(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
        {
            try
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Logger.Instance.Log(ErrorMessage, LogLevel.Error);
            }
            catch (Exception ex)
            {
                throw new Exception($"ErrorHandler(Exception): {ex.Message}");
            }
        }

    }
}
