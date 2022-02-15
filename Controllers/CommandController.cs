namespace QuestionnaireTeamBot.Controllers
{
    /// <summary>
    /// Controller for determining command by message
    /// </summary>
    public static class CommandController
    {
        /// <summary>
        /// Method for determining command object by string message
        /// </summary>
        /// <param name="messageText">Message by user</param>
        /// <returns>Object command</returns>
        public static Models.Command? GetCommand(string? messageText)
        {
            if (messageText == null)
                throw new Exception("Ошибка. Пустое сообщение");

            if (!IsCommand(messageText))
                return null;

            var words = messageText.Split(' ', 2);
            if (words.Length > 0)
            {
                Models.Command command = new Models.Command();
                if (words.Length == 2)
                    command.Data = words[1];
                switch (words[0])
                {
                    case "/register":
                        command.Type = Enums.TypeCommand.Register;
                        break;
                    case "/start":
                        command.Type = Enums.TypeCommand.Start;
                        break;
                    case "/clean":
                        command.Type = Enums.TypeCommand.Clean;
                        break;
                    case "/dailyreport":
                        command.Type = Enums.TypeCommand.DailyReport;
                        break;
                    default:
                        throw new Exception("Неизвестная команда");
                }
                return command;
            }
            else
                throw new Exception("Не удалось разобрать команду");
        }

        /// <summary>
        /// Method determining of message is command
        /// </summary>
        /// <param name="message">Message by user</param>
        /// <returns>True flag if message is command, else false</returns>
        private static bool IsCommand(string message)
        {
            if (message.Length > 0)
                if (message[0] == '/')
                    return true;
            return false;
        }
    }
}