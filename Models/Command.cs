using QuestionnaireTeamBot.Enums;

namespace QuestionnaireTeamBot.Models
{
    /// <summary>
    /// Model for commands by user
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Type of command
        /// </summary>
        public Enums.TypeCommand Type { get; set; }
        /// <summary>
        /// Command data
        /// </summary>
        public string Data { get; set; } = "";
    }
}