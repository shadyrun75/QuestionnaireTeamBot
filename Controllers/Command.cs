using Telegram.Bot.Types;

namespace Bot.Controllers
{
    public class Command
    {
        Users users = new Users();
        public string GetAnswer(Update update)
        {
            var messageText = update.Message.Text;
            bool isCommand = false;
            if (messageText.Length > 0)
                if (update.Message.Text[0] == '/')
                    isCommand = true;

            if (isCommand)
            {
                return GetCommand(update);

            }
            else
                if (!users.Contains(update.Message.Chat.Id))
                return "Ты кто? Я тебя не знаю. Зарегистрируйся через команду /register.";
            else
                return $"{users.Get(update.Message.Chat.Id).Description} сказал: {update.Message.Text}";

        }

        private string GetCommand(Update update)
        {
            if (update.Message.Text == "/register")
            {
                users.Add(update.Message.Chat);
                return "Вы зарегистрированы";
            }
            return "";
        }
    }
}