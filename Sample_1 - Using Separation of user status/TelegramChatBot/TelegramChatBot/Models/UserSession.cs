namespace TelegramChatBot.Models
{
    /// <summary>
    /// Класс для упраления состоянием пользователя 
    /// </summary>
    public class UserSession
    {
        public long TelegramId { get; }
        public string Role { get; set; } = "Guest";         // по умолчанию неавторизованный
        public string CurrentState { get; set; } = "Start"; // меню состояния

        public UserSession(long telegramId)
        {
            TelegramId = telegramId;
        }
    }
}
