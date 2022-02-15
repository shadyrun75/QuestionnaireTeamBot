using Telegram.Bot.Types;

namespace QuestionnaireTeamBot.Controllers
{
    public class Main
    {
        List<Discussion> discussions = new List<Discussion>();

        public IEnumerable<string> GetAnswer(Update update)
        {
            try
            {
                if (update == null)
                    throw new Exception("Ошибка. Пустое сообщение.");

                long chatId = update?.Message?.Chat.Id ?? 0;
                var messageText = update?.Message?.Text ?? "";
                var dis = discussions.FirstOrDefault(x => x.User.Id == chatId);
                if (dis == null)
                    return UnregisteredChat(messageText, update);

                return dis.Talk(messageText);
            }
            catch (Exception ex)
            {
                return new String[] { ex.Message };
            }
        }

        private IEnumerable<string> UnregisteredChat(string messageText, Update? update)
        {
            if (update == null)
                throw new Exception("Update is null");
            List<string> answer = new List<string>();
            var command = Command.GetCommand(messageText);
            if (command != null)
            {
                switch (command.Type)
                {
                    case Enums.TypeCommand.Register:
                        var dis = new Discussion(update?.Message?.Chat);
                        discussions.Add(dis);
                        answer.AddRange(dis.Talk(messageText));
                        break;
                    case Enums.TypeCommand.Start:
                        answer.Add("Приветствую. Для дальнейшей работы тебе нужно зарегистрироваться. Введи команду <b>/register</b>, а там посмотрим.");
                        break;
                    default:
                        answer.Add($"Команда <b>{command.Type}</b> недоступна для незарегистрированных пользователей.");
                        break;
                }
            }
            else
                answer.Add("Ты кто? Я тебя не знаю. Зарегистрируйся через команду <b>/register</b>.");
            return answer;
        }
    }
}