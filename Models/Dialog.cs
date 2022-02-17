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
        public bool IsFinished => Questions.Length <= Messages.Count(x => x.Answer != null);
        public Dialog(User user, DialogQuestions questions, IDialogController controller)
        {
            if (user == null)
                throw new Exception("User is empty");
            if (controller == null)
                throw new Exception("Controller is empty");
            if (questions == null)
                throw new Exception("Questions is empty");

            User = user;
            Questions = questions;
            Controller = controller;
            Messages = new List<DialogMessage>();
        }

        public Dialog(User user, Enums.TypeDialog typeDialog, IDialogController controller)
        {
            if (user == null)
                throw new Exception("User is empty");
            if (controller == null)
                throw new Exception("Controller is empty");

            User = user;
            Questions = new DialogQuestions(typeDialog, null);
            Controller = controller;
            Messages = new List<DialogMessage>();
        }

        public Dialog(User user, DialogQuestions questions, IDialogController controller, IEnumerable<DialogMessage> messages)
        {
            if (user == null)
                throw new Exception("User is empty");
            if (controller == null)
                throw new Exception("Controller is empty");
            if (questions == null)
                throw new Exception("Questions is empty");

            User = user;
            Questions = questions;
            Controller = controller;
            Messages = messages?.ToList() ?? new List<DialogMessage>();
        }

        public string GetQuestion(string? inputMessage = null)
        {
            var index = GetLastQuestionIndex();
            var question = Controller.PrepareQuestion(User, Questions[index], index, inputMessage);

            if (Messages.Count == index)
                Messages.Add(new DialogMessage()
                {
                    Question = new Message()
                });

            Messages[index].Question.Data = question;
            Messages[index].Question.Date = DateTime.Now;

            if (Questions[index] == null)
            {
                Messages[index].Answer = new Message()
                {
                    Data = "",
                    Date = DateTime.Now
                };
            }
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
            if (Questions.Length == 0)
                return 0;

            int index = Messages.Count(x => x.Answer != null);
            if (index == Questions.Length)
                index--;
            return index;
        }
    }
}