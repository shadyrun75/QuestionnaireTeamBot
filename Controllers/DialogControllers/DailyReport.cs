using QuestionnaireTeamBot.Models;

namespace QuestionnaireTeamBot.Controllers.DialogControllers
{
    public class DailyReport : Interfaces.IDialogController
    {
        public static string OnGetQuestionWhatYouDo(Models.User user, string question)
        {
            return String.Format(question, user.TimeReport == Enums.TimeReport.Morning ? "вчера" : "сегодня");
        }

        public static string OnGetQuestionWhatYouWannaDo(Models.User user, string question)
        {
            return String.Format(question, user.TimeReport == Enums.TimeReport.Morning ? "сегодня" : "завтра");
        }

        public string PrepareAnswer(User user, string value, int index)
        {
            switch (index)
            {
                case 1: return "Отчет получен";
                default: return "";
            }
        }

        public string PrepareQuestion(User user, Question? question, int index, string? command)
        {
            if (question == null)
                throw new Exception("Пустой вопрос в DailyReport!");
            switch (index)
            {
                case 0: return OnGetQuestionWhatYouDo(user, question.Text);
                case 1: return OnGetQuestionWhatYouWannaDo(user, question.Text);
            }
            return "Не удалось обработать вопрос";
        }


    }
}