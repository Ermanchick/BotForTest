using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static string token = "8771012832:AAE2P_M4lk-kLwIQUGmGrpT3REu0GDtCWBc";

    static async Task Main()
    {
        var bot = new TelegramBotClient(token);

        using var cts = new CancellationTokenSource();

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

        var me = await bot.GetMe();   // ✔ ВАЖНО: без Async
        Console.WriteLine($"Bot started: @{me.Username}");

        Console.ReadLine();
        cts.Cancel();
    }

    private static async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        if (update.Message?.Text is not { } text) return;

        Console.WriteLine($"User: {text}");

        await bot.SendMessage(
            chatId: update.Message.Chat.Id,
            text: $"Ты написал: {text}",
            cancellationToken: ct
        );
    }

    private static Task HandleError(ITelegramBotClient bot, Exception ex, CancellationToken ct)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return Task.CompletedTask;
    }
}