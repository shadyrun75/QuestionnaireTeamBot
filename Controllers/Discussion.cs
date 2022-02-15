using QuestionnaireTeamBot.Models;
using Telegram.Bot.Types;

namespace QuestionnaireTeamBot.Controllers
{
    public class Discussion
    {
        public Models.User User { get; private set; }
        public Models.Dialog CurrentDialog { get; set; } = null;
        public List<Models.Dialog> DialogHistory { get; private set; } = new List<Models.Dialog>();

        public Discussion(Models.User user)
        {
            User = user;
        }

        public Discussion(Chat chat)
        {
            User = new Models.User()
            {
                Id = chat.Id,
                UserName = chat.Username ?? chat.Id.ToString(),
                Description = $"{chat.LastName} {chat.FirstName}",
                TimeReport = Enums.TimeReport.Evening
            };
        }

        public IEnumerable<string> Talk(string messageText)
        {
            List<string> answer = new List<string>();

            var command = Controllers.Command.GetCommand(messageText);
            if (command == null)
                answer.AddRange(MessageIsNotCommand(messageText));
            else
                answer.AddRange(MessageIsCommand(command, messageText));

            return answer;
        }

        private IEnumerable<string> MessageIsNotCommand(string messageText)
        {
            List<string> answer = new List<string>();
            if (CurrentDialog != null)
            {
                answer.Add(CurrentDialog.SetAnswer(messageText));
                if (!CurrentDialog.IsFinished)
                    answer.Add(CurrentDialog.GetQuestion());
                else
                {
                    DialogHistory.Add(CurrentDialog);
                    CurrentDialog = null;
                }
            }
            else
                answer.Add($"Нет текущего диалога. Отправь команду или дождись вопроса.");
            return answer;
        }

        private IEnumerable<string> MessageIsCommand(Models.Command command, string messageText)
        {
            List<string> answer = new List<string>();
            switch (command.Type)
            {
                case Enums.TypeCommand.Register:
                    break;
                case Enums.TypeCommand.Clean:
                    foreach (var item in DialogHistory)
                    {
                        string temp = $"<b>Диалог {item.Type}.</b>\r\n";
                        foreach (var item2 in item.Messages)
                        {
                            temp += $"<b>Вопрос:</b> <i>{item2.Question?.Date}</i> {item2.Question?.Data}\r\n";
                            temp += $"<b>Ответ:</b> <i>{item2.Answer?.Date}</i> {item2.Answer?.Data}\r\n";
                        }
                        answer.Add(temp);
                    }
                    break;
                case Enums.TypeCommand.DailyReport:
                    CurrentDialog = new Dialog(User, Data.Dialogs.DailyReport, new Controllers.DialogControllers.DailyReport());
                    answer.Add(CurrentDialog.GetQuestion(command.Data));
                    break;
            }
            return answer;
        }
    }
}