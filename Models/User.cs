using Telegram.Bot.Types;

namespace Bot.Models
{
    /// <summary>
    /// Model for save data about chat user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Telegram chat class
        /// </summary>
        public Chat Chat { get; set; } = null;
        /// <summary>
        /// Idenitificator chat
        /// </summary>
        public long Id => Chat.Id;
        /// <summary>
        /// Property displayed user name of chat
        /// </summary>
        public string UserName => Chat.Username ?? "";
        /// <summary>
        /// Property displayed some data about user of chat
        /// </summary>
        public string Description => $"{Chat.FirstName} {Chat.LastName} {Chat.Bio} {Chat.Description}";
    }
}