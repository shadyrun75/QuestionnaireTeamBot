namespace QuestionnaireTeamBot.Models
{
    /// <summary>
    /// Model for save data about chat user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Idenitificator chat
        /// </summary>
        public long Id { get; set; } = 0;
        /// <summary>
        /// Property displayed user name of chat
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// Property displayed some data about user of chat
        /// </summary>
        public string Description { get; set; } = "";

        public Enums.TimeReport TimeReport { get; set; } = Enums.TimeReport.Evening;
    }
}