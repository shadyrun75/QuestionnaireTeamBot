using Telegram.Bot;
using Telegram.Bot.Types;

namespace QuestionnaireTeamBot.Controllers
{
    public class Main
    {
        TelegramBotClient bot;
        CancellationToken cst;
        static List<Discussion> discussions = new List<Discussion>();
        static ActionControllers.DailyReport dailyReportController = null;

        public delegate void SendMessageHandler(ITelegramBotClient botClient, string message, ChatId chatId, CancellationToken cancellationToken);
        public event SendMessageHandler? OnSendMessage;
        public Main(TelegramBotClient botClient, CancellationToken cancellationToken)
        {
            bot = botClient;
            cst = cancellationToken;
            InitializeDiscussion();
            InitializeControllers();
        }

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

        private void SendMessage(string message, Models.User user)
        {
            OnSendMessage?.Invoke(bot, message, new ChatId(user.Id), cst);
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
                        dis.OnSendMessage += SendMessage;
                        answer.AddRange(dis.Talk(messageText));
                        discussions.Add(dis);
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

        private void InitializeDiscussion()
        {
            foreach (var user in Database.DBWorker.Users.GetUsers())
            {
                var temp = new Discussion(user);
                temp.OnSendMessage += SendMessage;
                discussions.Add(temp);
            }
        }

        private void InitializeControllers()
        {
            InitializeDailyReport();
        }

        private void InitializeDailyReport()
        {
            dailyReportController = new ActionControllers.DailyReport(discussions);
            dailyReportController.Load(Database.DBWorker.DailyReport.LoadDailyReportsUsers(dailyReportController.DateReport));
        }
    }
}