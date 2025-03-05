using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.ChatCommands.Test_Page_Commands
{
    internal class VeiwStartMenu_ChatCommand : BaseChatCommand
    {
        public VeiwStartMenu_ChatCommand( TelegramBotClient client, Update update = null, string name = null) : 
            base(client, update)
        {
            _name = string.IsNullOrEmpty(name) ? this.GetType().Name : name;
        }

        public override async Task ExecuteAsync()
        {
            await BotClient.SendMessage(
                           Chat.Id,
                           "Выберите тип клавиатуру: \n" +
                           "/inline\n" +
                           "/reply\n");
        }

        public override async Task ExecuteAsync(Update update)
        {
            _update = update;
            _chat = update.Message.Chat;

            await ExecuteAsync();
        }

        public override async Task UndoAsync()
        {

        }
    }
}
