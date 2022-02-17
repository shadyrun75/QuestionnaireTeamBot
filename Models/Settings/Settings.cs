namespace QuestionnaireTeamBot.Models
{
    public class Settings
    {
        public string? TelegramBotKey { get; set; }
        public Database? Database { get; set; }

        public Settings()
        {
            Database = new Database();
        }

    }
}