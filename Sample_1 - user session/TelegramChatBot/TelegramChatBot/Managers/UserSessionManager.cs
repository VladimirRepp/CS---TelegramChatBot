using System.Collections.Concurrent;
using TelegramChatBot.Models;

namespace TelegramChatBot.Managers
{
    /// <summary>
    /// Класс для хранения состояний пользователей
    /// </summary>
    public class UserSessionManager
    {
        private readonly ConcurrentDictionary<long, UserSession> _sessions = new();

        public UserSession GetOrCreateSession(long userId)
        {
            return _sessions.GetOrAdd(userId, id => new UserSession(id));
        }

        public void RemoveSession(long userId)
        {
            _sessions.TryRemove(userId, out _);
        }

        public string GetUserState(long userId)
        {
            return _sessions.TryGetValue(userId, out var state) ? state.CurrentState : "default";
        }

        public UserSession SetUserState(long userId, string state)
        {
             _sessions[userId].CurrentState = state;
             return _sessions[userId];
         }

         public UserSession SetUserRole(long userId, string role)
         {
             _sessions[userId].Role = role;
             return _sessions[userId];
         }
    }
}
