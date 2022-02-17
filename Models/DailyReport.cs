namespace QuestionnaireTeamBot.Models
{
    public class DailyReport
    {
        public Dialog Dialog { get; private set; }
        public DateTime DateReport { get; private set; }
        public DailyReport(Dialog dialog, DateTime dateReport)
        {
            Dialog = dialog;
            DateReport = dateReport;
        }
    }
}