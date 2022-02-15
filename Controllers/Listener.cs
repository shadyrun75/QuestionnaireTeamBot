using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace QuestionnaireTeamBot.Controllers
{
    public class Listener
    {
        MainController commandController = new MainController();
        public async void Lisen(string botKey)
        {
            var botClient = new TelegramBotClient(botKey);

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            cts.Cancel();
        }

        async Task SendTextMessage(ITelegramBotClient botClient, string message, ChatId chatId, CancellationToken cancellationToken)
        {
            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: message,
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message == null)
                return;

            var chatId = update.Message.Chat.Id;

            if (update.Type != UpdateType.Message)
            {
                await SendTextMessage(botClient, "Usuported update type", chatId, cancellationToken);
                return;
            }

            if (update.Message!.Type != MessageType.Text)
            {
                await SendTextMessage(botClient, "Usuported message type", chatId, cancellationToken);
                return;
            }

            var answers = commandController.GetAnswer(update);
            foreach (var answer in answers)
            {
                if (answer.Length > 0)
                    await SendTextMessage(botClient, answer, chatId, cancellationToken);
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}