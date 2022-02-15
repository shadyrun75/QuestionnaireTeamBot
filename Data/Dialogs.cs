using QuestionnaireTeamBot.Models;

namespace QuestionnaireTeamBot.Data
{
    public static class Dialogs
    {
        public static DialogQuestions DailyReport => GenerateDailyReport();

        static DialogQuestions GenerateDailyReport()
        {
            List<Question> result = new List<Question>();

            result.Add(new Question("Что ты сделал за {0}"));
            result.Add(new Question("Что ты планируешь сделать за {0}"));

            return new DialogQuestions(Enums.TypeDialog.DailyReport, result);
        }
    }
}