namespace QuestionnaireTeamBot.Models
{
    public class DialogQuestions
    {
        private Question[] questionsList;

        public Enums.TypeDialog TypeDialog { get; private set; }

        public DialogQuestions(Enums.TypeDialog type, IEnumerable<Question>? questions)
        {
            TypeDialog = type;
            questionsList = questions?.ToArray() ?? new Question[0];
        }

        public Question? this[int index]
        {
            get
            {
                if ((questionsList.Length > 0) && (questionsList.Length > index) && (index >= 0))
                    return questionsList[index];
                else
                    return null;
            }
        }

        public Int32 Length => questionsList.Length;
    }
}