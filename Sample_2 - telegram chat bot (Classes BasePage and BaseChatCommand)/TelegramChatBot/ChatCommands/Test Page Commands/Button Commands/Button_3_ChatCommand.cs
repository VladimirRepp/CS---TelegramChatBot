using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.ChatCommands.Test_Page_Commands.Button_Commands
{
    internal class Button_3_ChatCommand : BaseChatCommand
    {
        public Button_3_ChatCommand(TelegramBotClient client, Update update = null, string name = null) :
            base(client, update)
        {
            _name = string.IsNullOrEmpty(name) ? this.GetType().Name : name;
        }

        public override async Task ExecuteAsync()
        {
            var callbackQuery = _update.CallbackQuery;

            await BotClient.AnswerCallbackQuery(callbackQuery.Id, "Пример полноэкранного текста!", showAlert: true);

            await BotClient.SendMessage(
                Chat.Id,
                $"Вы нажали на {callbackQuery.Data}"
                );
        }
        public override async Task ExecuteAsync(Update update)
        {
            _update = update;
            _chat = update.CallbackQuery.Message.Chat;
            await ExecuteAsync();
        }

        public override Task UndoAsync()
        {
            throw new NotImplementedException();
        }
    }
}
