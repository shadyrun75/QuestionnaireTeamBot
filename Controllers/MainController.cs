using Telegram.Bot.Types;

namespace QuestionnaireTeamBot.Controllers
{
    public class MainController
    {
        Users users = new Users();

        public IEnumerable<string> GetAnswer(Update update)
        {
            try
            {
                if (update == null)
                    throw new Exception("Ошибка. Пустое сообщение.");

                long chatId = update?.Message?.Chat.Id ?? 0;
                var messageText = update?.Message?.Text ?? "";

                if (!users.Contains(chatId))
                    return UnregisteredChat(messageText, update);

                return users.Get(chatId).GetAnswer(messageText);
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
            var command = CommandController.GetCommand(messageText);
            if (command != null)
            {
                switch (command.Type)
                {
                    case Enums.TypeCommand.Register:
                        users.Add(update.Message.Chat);
                        answer.AddRange(users.Get(update.Message.Chat.Id).GetAnswer(messageText));
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