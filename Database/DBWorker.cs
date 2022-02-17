namespace QuestionnaireTeamBot.Database
{
    public static class DBWorker
    {
        public static DailyReport DailyReport { get; private set; } = new DailyReport();

        public static Users Users { get; private set; } = new Users();
    }
}