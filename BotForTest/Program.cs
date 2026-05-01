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

        // ❗ В 22.x НЕТ GetMeAsync → только GetMe
        var me = await bot.GetMe();
        Console.WriteLine($"Bot started: @{me.Username}");

        Console.ReadLine();
        cts.Cancel();
    }

    private static async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        if (update.Message?.Text is not { } text)
            return;

        Console.WriteLine($"USER: {text}");

        // ❗ SendMessage вместо SendTextMessageAsync
        await bot.SendMessage(
            chatId: update.Message.Chat.Id,
            text: $"Echo: {text}",
            cancellationToken: ct
        );
    }

    private static Task HandleError(ITelegramBotClient bot, Exception ex, CancellationToken ct)
    {
        Console.WriteLine($"ERROR: {ex.Message}");
        return Task.CompletedTask;
    }
}
