using Telegram.Bot.Types;
using Telegram.Bot;
using System;

namespace TelegramChatBot.ChatCommands.Test_Page_Commands.Button_Commands
{
    internal class Button_1_ChatCommand : BaseChatCommand
    {
        public Button_1_ChatCommand(TelegramBotClient client, Update update = null, string name = null) :
        base(client, update)
        {
            _name = string.IsNullOrEmpty(name) ? this.GetType().Name : name;
        }

        public override async Task ExecuteAsync()
        {
            var callbackQuery = Update.CallbackQuery;

            await BotClient.AnswerCallbackQuery(callbackQuery.Id);

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
