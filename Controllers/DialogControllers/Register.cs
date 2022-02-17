using QuestionnaireTeamBot.Models;

namespace QuestionnaireTeamBot.Controllers.DialogControllers
{
    public class Register : Interfaces.IDialogController
    {
        public string PrepareAnswer(User user, string value, int index)
        {
            throw new NotImplementedException();
        }

        public string PrepareQuestion(User user, Question? question, int index, string? command)
        {
            if (command == null)
                throw new Exception("Пустой ключ доступа");
            if (command.ToLower() != Config.Key)
                throw new Exception("Неизвестный ключ доступа");
            QuestionnaireTeamBot.Database.DBWorker.Users.AddUser(user);
            return "Спасибо за регистрацию";
        }
    }
}