using QuestionnaireTeamBot.Models;

namespace QuestionnaireTeamBot.Controllers.DialogControllers
{
    public class SetMorningTime : Interfaces.IDialogController
    {
        public string PrepareAnswer(User user, string value, int index)
        {
            throw new NotImplementedException();
        }

        public string PrepareQuestion(User user, Question? question, int index, string? command)
        {
            user.TimeReport = Enums.TimeReport.Morning;
            return "Установлено утреннее время отчета";
        }
    }
}