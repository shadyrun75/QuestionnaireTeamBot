namespace QuestionnaireTeamBot.Models
{
    /// <summary>
    /// Модель для вопрос-ответа
    /// </summary>
    public class DialogMessage
    {
        /// <summary>
        /// Вопрос от бота
        /// </summary>
        public Message Question { get; private set; } = new Message();
        /// <summary>
        /// Ответ от пользователя
        /// </summary>
        public Message Answer { get; private set; } = new Message();

        public DialogMessage(string question)
        {
            Question.Data = question;
        }

        public string GetQuestion()
        {
            Question.Date = DateTime.Now;
            return Question.Data;
        }

    }
}