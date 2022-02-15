using QuestionnaireTeamBot.Controllers;
using Telegram.Bot.Types;

namespace QuestionnaireTeamBot.Models
{
    /// <summary>
    /// Model for save data about chat user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Idenitificator chat
        /// </summary>
        public long Id { get; set; } = 0;
        /// <summary>
        /// Property displayed user name of chat
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// Property displayed some data about user of chat
        /// </summary>
        public string Description { get; set; } = "";

        public Dialog CurrentDialog { get; set; } = null;
        public List<Dialog> DialogHistory { get; private set; } = new List<Dialog>();

        public IEnumerable<string> GetAnswer(string messageText)
        {
            List<string> answer = new List<string>();
            var command = CommandController.GetCommand(messageText);
            if (command == null)
            {
                if (CurrentDialog != null)
                {
                    CurrentDialog.AddAnswer(messageText);
                    if (!CurrentDialog.IsFinished)
                        answer.Add(CurrentDialog.GetQuestion());
                    else
                    {
                        DialogHistory.Add(CurrentDialog);
                        CurrentDialog = null;
                        answer.Add("Спасибо за ответ");
                    }
                }
                else
                    answer.Add($"Нет текущего диалога. Отправь команду или дождись вопроса.");
            }
            else
            {
                //answer.Add($"{Description} отправил команду {command.Type} с данными: {command.Data}");
                switch (command.Type)
                {
                    case Enums.TypeCommand.Register:
                        CurrentDialog = Data.Dialogs.GetDialog(Enums.TypeCommand.Register);
                        if (CurrentDialog != null)
                            answer.Add(CurrentDialog.GetQuestion());
                        break;
                    case Enums.TypeCommand.Clean:
                        foreach (var item in DialogHistory)
                        {
                            string temp = $"<b>Диалог {item.Type}.</b>\r\n";
                            foreach (var item2 in item.History)
                            {
                                temp += $"<b>Вопрос:</b> <i>{item2.Question.Date}</i> {item2.Question.Data}\r\n";
                                temp += $"<b>Ответ:</b> <i>{item2.Answer.Date}</i> {item2.Answer.Data}\r\n";
                            }
                            answer.Add(temp);
                        }
                        break;
                    case Enums.TypeCommand.DailyReport:
                        CurrentDialog = Data.Dialogs.GetDialog(Enums.TypeCommand.DailyReport);
                        if (CurrentDialog != null)
                            answer.Add(CurrentDialog.GetQuestion());
                        break;
                }
            }

            return answer;
        }
    }
}