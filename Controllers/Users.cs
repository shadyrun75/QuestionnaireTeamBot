using Telegram.Bot.Types;

namespace QuestionnaireTeamBot.Controllers
{
    public class Users
    {
        private List<QuestionnaireTeamBot.Models.User> userList = new List<Models.User>();
        public bool Contains(long chatId)
        {
            return userList.FirstOrDefault(x => x.Id == chatId) == null ? false : true;
        }

        public QuestionnaireTeamBot.Models.User Get(long chatId)
        {
            return userList.FirstOrDefault(x => x.Id == chatId) ?? throw new Exception("Критическая ошибка! Не найден чат!");
        }

        public void Add(Chat? chat)
        {
            if (chat == null)
                throw new Exception("Нельзя добавить в список зарегистрированных пользователей пустой чат.");
            if (!Contains(chat.Id))
                userList.Add(
                    new Models.User()
                    {
                        Id = chat.Id,
                        Description = $"{chat.LastName} {chat.FirstName}",
                        UserName = chat.Username
                    }
                );
        }
    }

}