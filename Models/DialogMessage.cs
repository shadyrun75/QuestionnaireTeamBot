namespace QuestionnaireTeamBot.Models
{
    /// <summary>
    /// Модель для вопрос-ответа
    /// </summary>
    public class DialogMessage
    {
        public Message? Question { get; set; }
        public Message? Answer { get; set; }
    }
}