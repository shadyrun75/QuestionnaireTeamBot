namespace QuestionnaireTeamBot.Models
{
    public class Message
    {
        private string data = "";
        public DateTime Date { get; set; }
        public string Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                Date = DateTime.Now;
            }
        }
    }
}