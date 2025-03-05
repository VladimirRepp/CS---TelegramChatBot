using Telegram.Bot.Types;
using Telegram.Bot;

namespace TelegramChatBot.ChatCommands.Test_Page_Commands
{
    internal class CreateStartBotCommands_ChatCommand : BaseChatCommand
    {
        public CreateStartBotCommands_ChatCommand(TelegramBotClient client, Update update = null, string name = null) :
       base(client, update)
        {
            _name = string.IsNullOrEmpty(name) ? this.GetType().Name : name;
        }

        private async Task CreateBotCommands()
        {
            var bot_commands = new[]
            {
                new BotCommand { Command = "start", Description = "Начать" },
                new BotCommand { Command = "stop", Description = "Выйти" }
            };

            await BotClient.SetMyCommands(bot_commands);
        }

        public override async Task ExecuteAsync()
        {
            await CreateBotCommands();
        }

        public override Task ExecuteAsync(Update update)
        {
            throw new NotImplementedException();
        }

        public override Task UndoAsync()
        {
            throw new NotImplementedException();
        }
    }
}
