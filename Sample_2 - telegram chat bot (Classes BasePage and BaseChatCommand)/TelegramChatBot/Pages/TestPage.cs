using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramChatBot.ChatCommands.Test_Page_Commands;
using TelegramChatBot.ChatCommands.Test_Page_Commands.Button_Commands;
using TelegramChatBot.ChatCommands.Test_Page_Commands.Reply_Commands;

namespace TelegramChatBot.Pages
{
    internal class TestPage : BasePage
    {
        // === Chat Commands === //
        private UnrecognizedCommand_ChatCommand _unrecognizedCommand_CC;
        private VeiwStartMenu_ChatCommand _viewStartMenu_CC;
        private CrateStartInlineButtons_ChatCommand _crateStartInlineButtons_CC;
        private CreateStartReplyKeyboard_ChatCommand _createStartReplyKeyboard_CC;

        // === Chat Commands For Reply Input === //
        private HelloRplayCommandResponder_ChatCommand _helloRplayCommandResponder_CC;
        private ByeRplayCommandResponder_ChatCommand _byeRplayCommandResponder_CC;

        // === Chat Commands For Buttons === //
        private Button_1_ChatCommand _button_1_CC;
        private Button_2_ChatCommand _button_2_CC;
        private Button_3_ChatCommand _button_3_CC;

        public TestPage(int id, TelegramBotClient client, string name = "") : base(id, client)
        {
            _name = string.IsNullOrEmpty(name) ? $"{this.GetType().Name}_{id}" : name;

            _unrecognizedCommand_CC = new(client);

            _viewStartMenu_CC = new(client);
            _viewStartMenu_CC.SetAvailableInputs("/start");

            _crateStartInlineButtons_CC = new(client);
            _crateStartInlineButtons_CC.SetAvailableInputs("/inline");

            _createStartReplyKeyboard_CC = new(client);
            _createStartReplyKeyboard_CC.SetAvailableInputs("/reply");

            _helloRplayCommandResponder_CC = new(client);
            _helloRplayCommandResponder_CC.SetAvailableInputs("Привет!", "Привет", "Привет, чат-бот!", "Привет, чат-бот", "Привет, чат бот!", "Привет, чат-бот");

            _byeRplayCommandResponder_CC = new(client);
            _byeRplayCommandResponder_CC.SetAvailableInputs("Пока!", "Пока", "Пока, чат-бот!", "Пока, чат-бот", "Пока, чат бот!", "Пока, чат бот");

            _button_1_CC = new(client);
            _button_1_CC.SetAvailableInputs("button_1");

            _button_2_CC = new(client);
            _button_2_CC.SetAvailableInputs("button_2");

            _button_3_CC = new(client);
            _button_3_CC.SetAvailableInputs("button_3");
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

            if (callbackQuery == null)
            {
                throw new Exception($"OnCallbackQuery: callbackQuery is null!");
            }

            Console.WriteLine($"{user.Username} ({user.Id}) нажал кнопку: {callbackQuery.Data}");

            if(await _button_1_CC.CheckAsync(callbackQuery.Data))
            {
                await _button_1_CC.ExecuteAsync(update);
                return;
            }

            if (await _button_2_CC.CheckAsync(callbackQuery.Data))
            {
                await _button_2_CC.ExecuteAsync(update);
                return;
            }

            if (await _button_3_CC.CheckAsync(callbackQuery.Data))
            {
                await _button_3_CC.ExecuteAsync(update);
                return;
            }
        }

        public override async Task OnMessageHandler(Update update)
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
                    if (await _viewStartMenu_CC.CheckAsync(message.Text))
                    {
                        await _viewStartMenu_CC.ExecuteAsync(update);
                        return;
                    }

                    if (await _crateStartInlineButtons_CC.CheckAsync(message.Text))
                    {
                        await _crateStartInlineButtons_CC.ExecuteAsync(update);
                        return;
                    }

                    if (await _createStartReplyKeyboard_CC.CheckAsync(message.Text))
                    {
                        await _createStartReplyKeyboard_CC.ExecuteAsync(update);
                        return;
                    }
                    #endregion

                    #region === Обработка replay команд ===
                    if (await _helloRplayCommandResponder_CC.CheckAsync(message.Text))
                    {
                        await _helloRplayCommandResponder_CC.ExecuteAsync(update);
                        return;
                    }

                    if (await _byeRplayCommandResponder_CC.CheckAsync(message.Text))
                    {
                        await _byeRplayCommandResponder_CC.ExecuteAsync(update);
                        return;
                    }
                    #endregion

                    break;

                default:
                    await _unrecognizedCommand_CC.ExecuteAsync();
                    return;
            }
        }
    }
}
