namespace TelegramChatBot.Global
{
    public static class ButtonNames
    {
        // === Common === //
        public readonly static string START_NAME_BUTTON = "button_";

        // === Button Names For LoginMainMenuCommands === //
        public readonly static string STUDENT_LOGIN = START_NAME_BUTTON + "student_login";
        public readonly static string TEACHER_LOGIN = START_NAME_BUTTON + "teacher_login";

        // === Button Names For StudentMainMenuCommands === //
        public readonly static string STUDENT_BUTTON_1 = START_NAME_BUTTON + "1_student";
        public readonly static string STUDENT_BUTTON_2 = START_NAME_BUTTON + "2_student";
        public readonly static string STUDENT_BUTTON_3 = START_NAME_BUTTON + "3_student";
        public readonly static string STUDENT_BACK = START_NAME_BUTTON + "student_back";

        // === Button Names For StudentMainMenuCommands === //
        public readonly static string TEACHER_BUTTON_1 = START_NAME_BUTTON + "1_teacher";
        public readonly static string TEACHER_BUTTON_2 = START_NAME_BUTTON + "2_teacher";
        public readonly static string TEACHER_BUTTON_3 = START_NAME_BUTTON + "3_teacher";
        public readonly static string TEACHER_BACK = START_NAME_BUTTON + "teacher_back";
    }
}

