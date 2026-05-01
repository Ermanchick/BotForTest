using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static string token = "8771012832:AAE2P_M4lk-kLwIQUGmGrpT3REu0GDtCWBc";

    static async Task Main()
    {
        Console.WriteLine("BOT STARTING...");

        var bot = new TelegramBotClient(token);

        var me = await bot.GetMe();
        Console.WriteLine($"Bot online: @{me.Username}");

        var cts = new CancellationTokenSource();

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        bot.StartReceiving(
            updateHandler: HandleUpdate,
            errorHandler: HandleError,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        Console.WriteLine("RECEIVING STARTED");

        Console.ReadLine();
        cts.Cancel();
    }

    private static async Task HandleUpdate(
        ITelegramBotClient bot,
        Update update,
        CancellationToken ct)
    {
        try
        {
            Console.WriteLine($"UPDATE: {update.Type}");

            if (update.Message is null)
            {
                Console.WriteLine("NO MESSAGE");
                return;
            }

            var text = update.Message.Text;

            Console.WriteLine($"TEXT: {text}");

            if (string.IsNullOrEmpty(text))
            {
                Console.WriteLine("EMPTY TEXT");
                return;
            }

            await bot.SendMessage(
                chatId: update.Message.Chat.Id,
                text: $"Echo: {text}",
                cancellationToken: ct
            );

            Console.WriteLine("MESSAGE SENT");
        }
        catch (Exception ex)
        {
            Console.WriteLine("UPDATE ERROR: " + ex.ToString());
        }
    }

    private static Task HandleError(
        ITelegramBotClient bot,
        Exception exception,
        CancellationToken ct)
    {
        Console.WriteLine("FATAL ERROR: " + exception.ToString());
        return Task.CompletedTask;
    }
}