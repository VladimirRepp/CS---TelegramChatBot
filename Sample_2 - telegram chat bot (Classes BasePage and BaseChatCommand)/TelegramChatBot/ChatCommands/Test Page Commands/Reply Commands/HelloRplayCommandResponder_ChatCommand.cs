using System;
using System.IO.Pipes;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.ChatCommands.Test_Page_Commands.Reply_Commands
{
    internal class HelloRplayCommandResponder_ChatCommand : BaseChatCommand
    {
        public Message Message;
        public User User;

        public HelloRplayCommandResponder_ChatCommand(TelegramBotClient client, Update update = null, string name = null) :
        base(client, update)
        {
            _name = string.IsNullOrEmpty(name) ? this.GetType().Name : name;
        }

        /// <summary>
        /// Первый параметр: Message; Второй: User
        /// </summary>
        /// <param name="my_params"></param>
        /// <exception cref="Exception"></exception>
        public override void SetParams(params object[] my_params)
        {
            if(my_params[0] is Message mess)
            {
                this.Message = mess;
            }
            else
            {
                throw new Exception("HelloRplayCommandResponder_ChatCommand.SetParams: my_params[0] is NOT Message");
            }

            if (my_params[1] is User user)
            {
                this.User = user;
            }
            else
            {
                throw new Exception("HelloRplayCommandResponder_ChatCommand.SetParams: my_params[1] is NOT User");
            }

        }

        public override async Task ExecuteAsync()
        {
            await BotClient.SendMessage(
                           Chat.Id,
                           $"Привет, {this.User.FirstName}!",
                           replyParameters: this.Message.MessageId);
        }

        public override async Task ExecuteAsync(Update update)
        {
            _update = update;
            _chat = update.Message.Chat;

            this.Message = update.Message;
            this.User = this.Message.From;

            await ExecuteAsync();
        }

        public override Task UndoAsync()
        {
            throw new NotImplementedException();
        }
    }
}
