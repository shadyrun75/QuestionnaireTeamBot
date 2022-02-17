using QuestionnaireTeamBot.Models;

namespace QuestionnaireTeamBot.Controllers.DialogControllers
{
    public class SetEveningTime : Interfaces.IDialogController
    {
        public string PrepareAnswer(User user, string value, int index)
        {
            throw new NotImplementedException();
        }

        public string PrepareQuestion(User user, Question? question, int index, string? command)
        {
            user.TimeReport = Enums.TimeReport.Evening;
            return "Установлено вечернее время отчета";
        }
    }
}