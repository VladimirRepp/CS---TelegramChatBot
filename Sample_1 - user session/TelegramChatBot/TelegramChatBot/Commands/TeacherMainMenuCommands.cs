﻿using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using TelegramChatBot.Managers;
using TelegramChatBot.Models;
using TelegramChatBot.Global;

namespace TelegramChatBot.Commands
{
    public class TeacherMainMenuCommands : BaseCommands
    {
        public TeacherMainMenuCommands(UserSessionManager sessionManager) : base(sessionManager) 
        {
            _commands = new Dictionary<string, Func<long, ITelegramBotClient, Update, Task>>
            {
                { CommandNames.MENU, VeiwMenuMessageCommandAsync },
                { CommandNames.HELP, ViewHelpMessageCommandAsync },
                { ButtonNames.TEACHER_BUTTON_1, Button1CallbackCommandAsync },
                { ButtonNames.TEACHER_BUTTON_2, Button2CallbackCommandAsync },
                { ButtonNames.TEACHER_BUTTON_3, Button3CallbackCommandAsync },
                { ButtonNames.TEACHER_BACK, BackCallbackCommandAsync },
            };
        }

        private async Task VeiwMenuMessageCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            var t_chat = update.Message.Chat;

            var inlineKeyboard = new InlineKeyboardMarkup(
                            new List<InlineKeyboardButton[]>()
                            {

                                        new InlineKeyboardButton[] {
                                            InlineKeyboardButton.WithCallbackData("Кнопка 1", ButtonNames.TEACHER_BUTTON_1),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Кнопка 2", ButtonNames.TEACHER_BUTTON_2),
                                            InlineKeyboardButton.WithCallbackData("Кнопка 3", ButtonNames.TEACHER_BUTTON_3)
                                        },
                                        new InlineKeyboardButton[] {
                                            InlineKeyboardButton.WithCallbackData("Назад", ButtonNames.TEACHER_BACK),
                                        }
                            });

            await botClient.SendMessage(
                t_chat.Id,
                "Меню для преподавателя:\n",
                replyMarkup: inlineKeyboard
                );
        }

        private async Task Button1CallbackCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            await botClient.SendMessage(
                update.CallbackQuery.Message.Chat.Id,
                $"Привет!\n" +
                $"Это пример #1 работы inline кнопок. Ты нажал на кнопку: {update.CallbackQuery.Data}"
            );
        }

        private async Task Button2CallbackCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            await botClient.SendMessage(
                update.CallbackQuery.Message.Chat.Id,
                $"Это пример #2 работы inline кнопок. Ты нажал на кнопку: {update.CallbackQuery.Data}\n" +
                $"Как тебя зовут?"
            );

            CatcherUserInput.Instance.HandlerAdd(userId, OnButton2CatchUserInput);
        }

        private async Task Button3CallbackCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            await botClient.SendMessage(
                update.CallbackQuery.Message.Chat.Id,
                $"Это пример #3 работы inline кнопок. Ты нажал на кнопку: {update.CallbackQuery.Data}\n" +
                $"Напиши два целых числа через пробел!"
            );

            CatcherUserInput.Instance.HandlerAdd(userId, OnButton3CatchUserInput);
        }

        private async Task BackCallbackCommandAsync(long userId, ITelegramBotClient botClient, Update update)
        {
            await botClient.SendMessage(
                update.CallbackQuery.Message.Chat.Id,
                $"Это пример #4 работы inline кнопок. Ты нажал на кнопку: {update.CallbackQuery.Data}\n" +
                $"Вернемся назад, для просмотра доступных действий вызови команду \"/menu\""
            );

            var userSession = _sessionManager.GetOrCreateSession(userId);
            userSession.Role = "Guest";
        }

        private async Task OnButton2CatchUserInput(long userId, ITelegramBotClient botClient, Message message)
        {
            await botClient.SendMessage(
                message.Chat.Id,
                $"А меня ТестЧатБот!\n" +
                $"Приятно познакомится!",
                replyParameters: message.MessageId
            );
        }

        private async Task OnButton3CatchUserInput(long userId, ITelegramBotClient botClient, Message message)
        {
            int fisrt_value = 0, second_value = 0;

            try
            {
                fisrt_value = Convert.ToInt32(message.Text.Split(' ')[0]);
                second_value = Convert.ToInt32(message.Text.Split(' ')[1]);
            }
            catch
            {
                await botClient.SendMessage(
                message.Chat.Id,
                $"Упс, что то пошло не так ...\n",
                replyParameters: message.MessageId
                );
            }

            await botClient.SendMessage(
               message.Chat.Id,
               $"{fisrt_value} - {second_value} = {fisrt_value - second_value}"
               );
        }
    }
}
