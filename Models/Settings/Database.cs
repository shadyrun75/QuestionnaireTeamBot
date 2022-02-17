using System.Text.Json.Serialization;

namespace QuestionnaireTeamBot.Models
{
    public class Database
    {
        public string? Host { get; set; }
        public int? Port { get; set; }
        public string? DatabaseName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }

        [JsonIgnore]
        public string ConnectionString => $"Host={Host};Port={Port};Database={DatabaseName};Username={UserName};Password={Password}";
    }
}