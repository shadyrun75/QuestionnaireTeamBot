namespace QuestionnaireTeamBot.Models
{
    public class Dialog
    {
        private List<DialogMessage> dialogMessages = new List<DialogMessage>();
        private int currentQuestion = 0;
        private DialogMessage CurrentDialog => dialogMessages[currentQuestion];
        public Enums.TypeCommand Type { get; private set; }
        public bool IsFinished { get; private set; } = false;
        public DialogMessage[] History => dialogMessages.ToArray();

        public Dialog(Enums.TypeCommand typeQuestion, string[] questions)
        {
            if (questions.Length == 0)
                throw new Exception("Список вопросов не может быть пустым. Инициализация диалога невозможна.");

            Type = typeQuestion;

            foreach (var item in questions)
            {
                dialogMessages.Add(new DialogMessage(item));
            }
        }

        public string GetQuestion() => CurrentDialog.GetQuestion();
        public void AddAnswer(string value)
        {
            CurrentDialog.Answer.Data = value;
            if (currentQuestion == dialogMessages.Count - 1)
            {
                IsFinished = true;
                return;
            }
            currentQuestion++;
        }
    }
}