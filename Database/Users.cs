using Npgsql;

namespace QuestionnaireTeamBot.Database
{
    public class Users
    {
        public void AddUser(Models.User user)
        {
            try
            {
                using (var connection = new NpgsqlConnection(Config.Settings.Database.ConnectionString))
                {
                    connection.Open();
                    NpgsqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        using (var command = new NpgsqlCommand())
                        {
                            command.Connection = connection;
                            command.Transaction = transaction;
                            command.CommandText = "select id from public.telegramusers where id = @id";
                            command.Parameters.AddWithValue("id", user.Id);
                            var obj = command.ExecuteScalar();
                            if (obj != null)
                            {
                                command.CommandText = "update public.telegramusers set username = @username, description = @description where id = @id";
                                command.Parameters.AddWithValue("username", user.UserName);
                                command.Parameters.AddWithValue("description", user.Description);
                                command.ExecuteNonQuery();
                            }
                            else
                            {
                                command.CommandText = "insert into public.telegramusers values (@id, @username, @description)";
                                command.Parameters.AddWithValue("username", user.UserName);
                                command.Parameters.AddWithValue("description", user.Description);
                                command.ExecuteNonQuery();
                            }

                            UpdateRegister(user, connection, transaction);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error add user: {ex.Message}");
                throw ex;
            }
        }

        private void UpdateRegister(Models.User user, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = "update questionnaireteam.registerusers set blockdate = current_timestamp where blockdate is null and userid = @id";
                command.Parameters.AddWithValue("id", user.Id);
                command.ExecuteNonQuery();
                command.CommandText = "insert into questionnaireteam.registerusers (userid, registerdate, timereport) values (@id, current_timestamp, @timereport)";
                command.Parameters.AddWithValue("timereport", (int)user.TimeReport);
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Models.User> GetUsers()
        {
            List<Models.User> result = new List<Models.User>();
            try
            {
                using (var connection = new NpgsqlConnection(Config.Settings.Database.ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "select telegramusers.id, telegramusers.username, telegramusers.description, registerusers.timereport " +
                            "from public.telegramusers join questionnaireteam.registerusers on registerusers.userid = telegramusers.id " +
                            "where registerusers.blockdate is null or registerusers.blockdate > current_date";
                        using (var reader = command.ExecuteReader())
                            while (reader.Read())
                                result.Add(new Models.User()
                                {
                                    Id = reader.GetInt64(0),
                                    UserName = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    TimeReport = (Enums.TimeReport)reader.GetInt32(3)
                                });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error add user: {ex.Message}");
            }
            return result;
        }
    }
}