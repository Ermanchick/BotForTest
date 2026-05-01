using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static string token = "8771012832:AAE2P_M4lk-kLwIQUGmGrpT3REu0GDtCWBc";

    static async Task Main()
    {
        try
        {
            Console.WriteLine("STEP 1");

            var bot = new TelegramBotClient(token);

            Console.WriteLine("STEP 2");

            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = null
            };

            bot.StartReceiving(
                updateHandler: HandleUpdateAsync,
                errorHandler: HandleErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            Console.WriteLine("STEP 3 - RECEIVING STARTED");

            var me = await bot.GetMe();
            Console.WriteLine($"Bot started: @{me.Username}");

            Console.ReadLine();
            cts.Cancel();
        }
        catch (Exception ex)
        {
            Console.WriteLine("FATAL ERROR:");
            Console.WriteLine(ex);
        }
    }

    private static async Task HandleUpdateAsync(
        ITelegramBotClient bot,
        Update update,
        CancellationToken ct)
    {
        if (update.Message?.Text is not { } text) return;

        Console.WriteLine($"User: {text}");

        await bot.SendMessage(
            chatId: update.Message.Chat.Id,
            text: $"Ты написал: {text}",
            cancellationToken: ct
        );
    }

    private static Task HandleErrorAsync(
        ITelegramBotClient bot,
        Exception ex,
        CancellationToken ct)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return Task.CompletedTask;
    }
}