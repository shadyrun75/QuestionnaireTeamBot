namespace QuestionnaireTeamBot.Models
{
    public class DialogQuestions
    {
        public Question[] Questions { get; private set; }

        public Enums.TypeDialog TypeDialog { get; private set; }

        public DialogQuestions(Enums.TypeDialog type, IEnumerable<Question> questions)
        {
            TypeDialog = type;
            Questions = questions.ToArray();
        }
    }
}