namespace QuestionnaireTeamBot.Interfaces
{
    public interface IDialogController
    {
        public string PrepareQuestion(Models.User user, Models.Question? question, int index, string? command);
        public string PrepareAnswer(Models.User user, string value, int index);
    }
}