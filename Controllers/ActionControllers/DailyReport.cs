using System.Timers;

namespace QuestionnaireTeamBot.Controllers.ActionControllers
{
    public class DailyReport
    {
        /// <summary>
        /// The hour of start morning report 
        /// </summary>
        const int morningHour = 9;
        /// <summary>
        /// The hour of start evening report
        /// </summary>
        const int eveningHour = 15;
        /// <summary>
        /// The hour of end report of day
        /// </summary>
        const int reportEndHour = 15;

        /// <summary>
        /// Current list of report of users whom answer on questions right now
        /// </summary>
        List<Models.DailyReport> currentReports = new List<Models.DailyReport>();
        /// <summary>
        /// List of users id with finished reports of day
        /// </summary>
        List<long> finishedReports = new List<long>();
        IEnumerable<Discussion> discussions;
        System.Timers.Timer timer = new System.Timers.Timer(10000);

        DateTime dateReportEnd => DateReport.AddDays(1).AddHours(reportEndHour);
        DateTime dateMorningReport => DateReport.Date.AddDays(1).AddHours(morningHour);
        DateTime dateEveningReport => DateReport.Date.AddHours(eveningHour);
        DateTime oldDateReport = DateTime.Today.AddHours(reportEndHour);
        public DateTime DateReport => DateTime.Now.Hour >= reportEndHour ? DateTime.Today : DateTime.Today.AddDays(-1);
        public DailyReport(IEnumerable<Discussion> discussionsList)
        {
            if (discussionsList == null)
                throw new Exception("Список дискуссий не должен быть Null");
            discussions = discussionsList;
            timer.Elapsed += timerTick;
            timer.Start();
        }

        public void Load(IEnumerable<long> finishedDailyReportList)
        {
            if (finishedDailyReportList != null)
                finishedReports.AddRange(finishedDailyReportList);
        }

        private void timerTick(object? sender, ElapsedEventArgs e)
        {
            timer.Stop();
            try
            {
                if ((DateTime.Now >= dateMorningReport) && (DateTime.Now < dateReportEnd))
                    InitializeReport(Enums.TimeReport.Morning);
                if ((DateTime.Now >= dateEveningReport) && (DateTime.Now < dateReportEnd))
                    InitializeReport(Enums.TimeReport.Evening);
                if (oldDateReport != DateReport)
                    UpdateDateReport();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DailyController in timer tick: {ex.Message}");
            }
            timer.Start();
        }

        private void InitializeReport(Enums.TimeReport timeReport)
        {
            var list = discussions.Where(x => x.User.TimeReport == timeReport);
            foreach (var item in list)
            {
                if (finishedReports.Contains(item.User.Id))
                    continue;
                var temp = currentReports.FirstOrDefault(x => x.DateReport == DateReport && x.Dialog.User == item.User);
                if (temp == null)
                {
                    item.Talk("/dailyreport", true);
                    if (item.CurrentDialog != null)
                        currentReports.Add(new Models.DailyReport(item.CurrentDialog, DateReport));
                }
                else
                if (item.CurrentDialog?.Type != Enums.TypeDialog.DailyReport)
                    currentReports.Remove(temp);
            }
        }

        private void UpdateDateReport()
        {
            oldDateReport = DateReport;
            finishedReports = new List<long>();
        }
    }
}