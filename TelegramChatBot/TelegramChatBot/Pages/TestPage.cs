using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramChatBot.Pages
{
    internal class TestPage : BasePage
    {
        public TestPage(int id, TelegramBotClient client, string name = "") : base(id, client)
        {
            _name = string.IsNullOrEmpty(name) ? $"{this.GetType().Name}_{id}" : name;
        }

        public override void Open()
        {
            Program.OnMessageHandlerEvent += OnMessageHandler;
            Program.OnCallbackQueryEvent += OnCallbackQueryHandler;
        }

        public override void Close()
        {
            Program.OnMessageHandlerEvent -= OnMessageHandler;
            Program.OnCallbackQueryEvent -= OnCallbackQueryHandler;
        }

        public override void Close(BasePage page)
        {
            Close();
            page.Open();
        }

        // === Use case examples === //

        public override async Task OnCallbackQueryHandler(Update update)
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
                    await BotClient.AnswerCallbackQuery(callbackQuery.Id);

                    await BotClient.SendMessage(
                        chat.Id,
                        $"Вы нажали на {callbackQuery.Data}"
                        );
                    break;

                case "button_3":
                    await BotClient.AnswerCallbackQuery(callbackQuery.Id, "Пример возможного текста!");

                    await BotClient.SendMessage(
                        chat.Id,
                        $"Вы нажали на {callbackQuery.Data}"
                        );
                    break;

                case "button_4":
                    await BotClient.AnswerCallbackQuery(callbackQuery.Id, "Пример полноэкранного текста!", showAlert: true);

                    await BotClient.SendMessage(
                        chat.Id,
                        $"Вы нажали на {callbackQuery.Data}"
                        );
                    break;
            }
        }

        public override async Task OnMessageHandler(Update update)
        {
            Message? message = update.Message;
            User? user = message?.From;
            var chat = message.Chat;
            
            if (message == null)
            {
                throw new Exception("OnMessage: message is null!");
            }

            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

            switch (message.Type)
            {
                case MessageType.Text:

                    #region === Обработка текстовых команд ===
                    if (message.Text == "/start")
                    {
                        await BotClient.SendMessage(
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

                        await BotClient.SendMessage(
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

                        await BotClient.SendMessage(
                            chat.Id,
                            "Выбрана reply клавиатура!",
                            replyMarkup: replyKeyboard);

                        return;
                    }
                    #endregion

                    #region === Обработка replay ввода ===
                    if (message.Text == "Привет, чат-бот!")
                    {
                        await BotClient.SendMessage(
                            chat.Id,
                            $"Привет, {user.FirstName}!",
                            replyParameters: message.MessageId);

                        return;
                    }

                    if (message.Text == "Пока, чат-бот!")
                    {
                        await BotClient.SendMessage(
                            chat.Id,
                            $"До встречи, {user.FirstName}!",
                            replyParameters: message.MessageId);

                        return;
                    }
                    #endregion

                    break;

                default: 
                    await BotClient.SendMessage(
                        chat.Id,
                        "Нераспознанный ввод ...");
                    return;
            }
        }
    }
}
