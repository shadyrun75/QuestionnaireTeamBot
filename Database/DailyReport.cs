
using Npgsql;

namespace QuestionnaireTeamBot.Database
{
    public class DailyReport
    {
        public IEnumerable<long> LoadDailyReportsUsers(DateTime dateReport)
        {
            List<long> result = new List<long>();
            try
            {
                using (var connection = new NpgsqlConnection(Config.Settings.Database.ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "select dailyreport.userid " +
                            "from questionnaireteam.dailyreport " +
                            "where dailyreport.reportdate = @reportdate";
                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                            {
                                result.Add(reader.GetInt64(0));
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error load daily reports users: {ex.Message}");
            }
            return result;
        }
    }
}