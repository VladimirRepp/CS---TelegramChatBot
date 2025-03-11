namespace TelegramChatBot.Utils
{
    public static class ConsoleCommands
    {
        public static Dictionary<int, string> COMMANDS = new Dictionary<int, string>() 
        { 
            [-1] = "stop",
        };

        public static bool CheckCommand(int key, string input)
        {
            return COMMANDS[key] == input;
        }
    }
}
