using Telegram.Bot.Types;

namespace Bot.Controllers
{
    public class Users
    {
        private List<Bot.Models.User> userList = new List<Models.User>();
        public bool Contains(long chatId)
        {
            return userList.FirstOrDefault(x => x.Id == chatId) == null ? false : true;
        }

        public Bot.Models.User Get(long chatId)
        {
            return userList.FirstOrDefault(x => x.Id == chatId);
        }


        public void Add(Chat chat)
        {
            if (!Contains(chat.Id))
                userList.Add(
                    new Models.User()
                    {
                        Chat = chat
                    }
                );
        }
    }

}