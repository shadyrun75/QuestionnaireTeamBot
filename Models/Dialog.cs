using QuestionnaireTeamBot.Interfaces;

namespace QuestionnaireTeamBot.Models
{
    public class Dialog
    {

        public User User { get; private set; }
        public DialogQuestions Questions { get; private set; }

        public List<DialogMessage> Messages { get; private set; }
        public Enums.TypeDialog Type => Questions.TypeDialog;

        public IDialogController Controller { get; private set; }
        public bool IsFinished => Questions.Questions.Length == Messages.Count(x => x.Answer != null);
        public Dialog(User user, DialogQuestions questions, IDialogController controller)
        {
            User = user;
            Questions = questions;
            Controller = controller;
            Messages = new List<DialogMessage>();
        }

        public Dialog(User user, DialogQuestions questions, IDialogController controller, IEnumerable<DialogMessage> messages)
        {
            User = user;
            Questions = questions;
            Controller = controller;
            Messages = messages.ToList();
        }

        public string GetQuestion(string? inputMessage = null)
        {
            var index = GetLastQuestionIndex();
            var question = Controller.PrepareQuestion(User, Questions.Questions[index], index, inputMessage);

            if (Messages.Count == index)
                Messages.Add(new DialogMessage()
                {
                    Question = new Message()
                });
            Messages[index].Question.Data = question;
            Messages[index].Question.Date = DateTime.Now;

            return question;
        }

        public string SetAnswer(string inputMessage)
        {
            var index = GetLastQuestionIndex();
            try
            {
                var answer = Controller.PrepareAnswer(User, inputMessage, index);
                Messages[index].Answer = new Message()
                {
                    Data = inputMessage,
                    Date = DateTime.Now
                };
                return answer;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private int GetLastQuestionIndex()
        {
            int index = Messages.Count(x => x.Answer != null);
            if (index == Questions.Questions.Length)
                index--;
            return index;
        }
    }
}