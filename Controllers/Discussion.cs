using QuestionnaireTeamBot.Models;
using Telegram.Bot.Types;

namespace QuestionnaireTeamBot.Controllers
{
    public class Discussion
    {
        public Models.User User { get; private set; }
        public Models.Dialog? CurrentDialog { get; set; }
        public List<Models.Dialog> DialogHistory { get; private set; } = new List<Models.Dialog>();
        public delegate void SendMessageHandler(string message, Models.User user);
        public event SendMessageHandler? OnSendMessage;
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

        public IEnumerable<string> Talk(string messageText, bool botInitiator = false)
        {
            List<string> answer = new List<string>();

            var command = Controllers.Command.GetCommand(messageText);
            if (command == null)
                answer.AddRange(MessageIsNotCommand(messageText));
            else
                answer.AddRange(MessageIsCommand(command, messageText));

            if (botInitiator)
            {
                foreach (var item in answer)
                    OnSendMessage?.Invoke(item, User);
                return answer;
            }

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
                    AddInHistoryCurrentDialog();
            }
            else
                answer.Add($"Нет текущего диалога. Отправь команду или дождись вопроса.");
            return answer;
        }

        private void AddInHistoryCurrentDialog()
        {
            if (CurrentDialog != null)
                DialogHistory.Add(CurrentDialog);
            CurrentDialog = null;
        }

        private IEnumerable<string> MessageIsCommand(Models.Command command, string messageText)
        {
            List<string> answer = new List<string>();
            switch (command.Type)
            {
                case Enums.TypeCommand.Register:
                    CurrentDialog = new Dialog(User, Enums.TypeDialog.Register, new Controllers.DialogControllers.Register());
                    answer.Add(CurrentDialog.GetQuestion(command.Data));
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
                case Enums.TypeCommand.SetEveningTime:
                    CurrentDialog = new Dialog(User, Enums.TypeDialog.SetEveningTime, new Controllers.DialogControllers.SetEveningTime());
                    answer.Add(CurrentDialog.GetQuestion(command.Data));
                    AddInHistoryCurrentDialog();
                    break;
                case Enums.TypeCommand.SetMorningTime:
                    CurrentDialog = new Dialog(User, Enums.TypeDialog.SetMorningTime, new Controllers.DialogControllers.SetMorningTime());
                    answer.Add(CurrentDialog.GetQuestion(command.Data));
                    AddInHistoryCurrentDialog();
                    break;
            }
            return answer;
        }
    }
}