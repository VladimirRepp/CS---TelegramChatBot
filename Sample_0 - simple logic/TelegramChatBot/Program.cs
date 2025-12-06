using System.Configuration;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramChatBot.Utils;

namespace TelegramChatBot
{
    internal class Program
    {
        private static TelegramBotClient _botClient;

        private static async Task Main(string[] args)
        {
            string token = ConfigurationManager.ConnectionStrings["TelegramBotToken"].ConnectionString;
            _botClient = new(token);

            var me = await _botClient.GetMe();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery },
                DropPendingUpdates = true
            };
            var cts = new CancellationTokenSource();

            // === Testing ===
            await CreateBotCommands();
            // ===============

            _botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cts.Token);

            Console.WriteLine($"Бот {me.Username} запущен ...");

            while (!ConsoleHelper.IsExit()) ;
            Exiting();
        }

        private static async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        MessageHandler(update);
                        break;

                    case UpdateType.CallbackQuery:
                        CallbackQueryHandler(update);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"UpdateHandler(Exception): {ex.Message}");
            }
        }

        private static async Task ErrorHandler(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
        {
            try
            {
                var errorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine($"ErrorHandler: {errorMessage}");
            }
            catch (Exception ex)
            {
                throw new Exception($"ErrorHandler(Exception): {ex.Message}");
            }
        }

        private static async Task CallbackQueryHandler(Update update)
        {
            var callbackQuery = update.CallbackQuery;
            var user = callbackQuery.From;
            var chat = callbackQuery.Message.Chat;
            
            if (callbackQuery == null)
            {
                throw new Exception($"OnCallbackQuery: callbackQuery is null!");
            }

            Console.WriteLine($"{user.Username} ({user.Id}) нажал кнопку: {callbackQuery.Data}");


            switch (callbackQuery.Data)
            {
                case "button_2":
                await _botClient.AnswerCallbackQuery(callbackQuery.Id);
                    

                    await _botClient.SendMessage(
                        chat.Id,
                        $"Вы нажали на {callbackQuery.Data}"
                        );
                    break;

                case "button_3":
                    await _botClient.AnswerCallbackQuery(callbackQuery.Id, "Пример возможного текста!");

                    await _botClient.SendMessage(
                        chat.Id,
                        $"Вы нажали на {callbackQuery.Data}"
                        );
                    break;

                case "button_4":
                    await _botClient.AnswerCallbackQuery(callbackQuery.Id, "Пример полноэкранного текста!", showAlert: true);

                    await _botClient.SendMessage(
                        chat.Id,
                        $"Вы нажали на {callbackQuery.Data}"
                        );
                    break;
            }
        }

        private static async Task MessageHandler(Update update)
        {
            Message? message = update.Message;
            User? user = message?.From;

            if (message == null)
            {
                throw new Exception("OnMessage: message is null!");
            }

            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

            var chat = message.Chat;

            switch (message.Type)
            {
                case MessageType.Text:

                    #region === Обработка текстовых команд ===
                    if (message.Text == "/start")
                    {
                        await _botClient.SendMessage(
                            chat.Id,
                            "Выберите тип клавиатуру: \n" +
                            "/inline\n" +
                            "/reply\n");

                        return;
                    }

                    if (message.Text == "/inline")
                    {
                        var inlineKeyboard = new InlineKeyboardMarkup(
                            new List<InlineKeyboardButton[]>()
                            {
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithUrl("Кнопка №1: гиперссылка на сайт", "https://habr.com/"),
                                            InlineKeyboardButton.WithCallbackData("Кнопка №2: некоторое действие", "button_2"),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Кнопка №3: пример текста", "button_3"),
                                            InlineKeyboardButton.WithCallbackData("Кнопка №4: пример полноэкранного текста", "button_4"),
                                        },
                            });

                        await _botClient.SendMessage(
                             chat.Id,
                            "Выбрана inline клавиатура!",
                            replyMarkup: inlineKeyboard);

                        return;
                    }

                    if (message.Text == "/reply")
                    {
                        var replyKeyboard = new ReplyKeyboardMarkup(
                            new List<KeyboardButton[]>()
                            {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("/start"),
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("/inline"),
                                            new KeyboardButton("/reply")
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Привет, чат-бот!"),
                                            new KeyboardButton("Пока, чат-бот!")
                                        }
                            })
                        {
                            ResizeKeyboard = true,
                        };

                        await _botClient.SendMessage(
                            chat.Id,
                            "Выбрана reply клавиатура!",
                            replyMarkup: replyKeyboard);

                        return;
                    }
                    #endregion

                    #region === Обработка replay ввода ===
                    if (message.Text == "Привет, чат-бот!")
                    {
                        await _botClient.SendMessage(
                            chat.Id,
                            $"Привет, {user.FirstName}!",
                            replyParameters: message.MessageId);

                        return;
                    }

                    if (message.Text == "Пока, чат-бот!")
                    {
                        await _botClient.SendMessage(
                            chat.Id,
                            $"До встречи, {user.FirstName}!",
                            replyParameters: message.MessageId);

                        return;
                    }
                    #endregion

                    break;

                default:
                    await _botClient.SendMessage(
                        chat.Id,
                        "Нераспознанный ввод ...");
                    return;
            }
        }

        private static async Task CreateBotCommands()
        {
            var bot_commands = new[]
          {
                new BotCommand { Command = "start", Description = "Начать" },
                new BotCommand { Command = "stop", Description = "Закончить" }
            };

            await _botClient.SetMyCommands(bot_commands);
        }

        private static void Exiting()
        {
            Console.WriteLine("Command: stop called");
            Console.WriteLine("Startup exit ...");

            // todo for exit ...

            Console.WriteLine("Exited");
        }
    }
}



