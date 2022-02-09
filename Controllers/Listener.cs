using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Bot.Controllers;

namespace Bot.Controllers
{
    public class Listener
    {
        Command commandController = new Command();
        public async void Lisen()
        {
            var botClient = new TelegramBotClient("5196780296:AAH-M3puDd4ymkEOdtRlZWcpdpxBVLVMfoc");

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

            /*var messageText = update.Message.Text;
            var temp = $"{update.Message.Chat.FirstName} {update.Message.Chat.LastName}";

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            await SendTextMessage(botClient, $"{temp} text {messageText}", chatId, cancellationToken);*/
            var message = commandController.GetAnswer(update);
            if (message.Length > 0)
                await SendTextMessage(botClient, message, chatId, cancellationToken);
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