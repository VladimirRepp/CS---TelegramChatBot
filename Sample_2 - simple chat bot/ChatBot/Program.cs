using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// Замените на ваш токен от BotFather
string botToken = "BotName";

// Создаем клиент бота
var botClient = new TelegramBotClient(botToken);

// Список шутливых ответов на вопрос "Как дела?"
var funnyResponses = new[]
{
    "Отлично! Только что победил в конкурсе красоты среди ботов!",
    "Лучше всех! Сегодня меня почти не дизлайкнули!",
    "Как у робота: нет чувств, но много амбиций!",
    "Всё супер! Хотя кто я такой, чтобы жаловаться? Я же просто код!",
    "Превосходно! Только что обновил свои виртуальные мозги!"
};

// Обработчик обновлений
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Работаем только с текстовыми сообщениями
    if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
        return;

    var message = update.Message;
    var chatId = message.Chat.Id;
    var username = message.From?.FirstName ?? "Пользователь";

    // Обрабатываем команды и текст
    switch (message.Text)
    {
        case "/start":
            await botClient.SendMessage(
                chatId: chatId,
                text: $"Добро пожаловать, {username}!",
                cancellationToken: cancellationToken);
            break;

        case "/help":
            await botClient.SendMessage(
                chatId: chatId,
                text: "Этот бот умеет:\n" +
                      "/start - приветствие\n" +
                      "/help - список команд\n" +
                      "Привет! - ответить приветствием\n" +
                      "Как дела? - шуточный ответ",
                cancellationToken: cancellationToken);
            break;

        case "Привет!":
            await botClient.SendMessage(
                chatId: chatId,
                text: "И тебе привет!",
                cancellationToken: cancellationToken);
            break;

        case "Как дела?":
            // Выбираем случайный ответ
            Random rnd = new();
            string randomResponse = funnyResponses[rnd.Next(funnyResponses.Length)];

            await botClient.SendMessage(
                chatId: chatId,
                text: randomResponse,
                cancellationToken: cancellationToken);
            break;

        default:
            // Игнорируем неизвестные сообщения
            break;
    }
}

// Обработчик ошибок
Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    Console.WriteLine($"Ошибка: {exception.Message}");
    return Task.CompletedTask;
}

// Настройка и запуск бота
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>() // Получаем все типы обновлений
};

// Запускаем бота
using var cts = new CancellationTokenSource();
botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    errorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

// Получаем информацию о боте
var me = await botClient.GetMe();
Console.WriteLine($"Бот @{me.Username} запущен!");
Console.WriteLine("Для остановки нажмите Enter...");
Console.ReadLine();

// Остановка бота
cts.Cancel();