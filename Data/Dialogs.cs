namespace QuestionnaireTeamBot.Data
{
    public static class Dialogs
    {
        static readonly Models.Dialog[] ListDialogs = new Models.Dialog[]
        {
            new Models.Dialog(Enums.TypeCommand.DailyReport, new string[]
            {
                "Что ты сделал за {0}?",
                "Что ты планируешь сделать за {0}}?"
            }),
            new Models.Dialog(Enums.TypeCommand.Register, new string[]
            {
                "Как к тебе обращаться?",
                "Когда тебе удобно сообщать о проделанной работе?"
            })
        };

        public static Models.Dialog GetDialog(Enums.TypeCommand typeValue)
        {
            var temp = ListDialogs.FirstOrDefault(x => x.Type == typeValue)?.History.Select(x => x.Question.Data);
            if (temp == null)
                return null;
            var result = new Models.Dialog(typeValue, temp.ToArray());
            return result;
        }
    }
}