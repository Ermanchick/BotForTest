using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

class Program
{
    private static string token = "8771012832:AAE2P_M4lk-kLwIQUGmGrpT3REu0GDtCWBc";

    static async Task Main()
    {
        Log("START PROGRAM");

        try
        {
            Log("CREATING BOT CLIENT...");
            var bot = new TelegramBotClient(token);

            Log("BOT CLIENT CREATED");

            using var cts = new CancellationTokenSource();

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message }
            };

            Log("START RECEIVING SETUP");

            bot.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandleErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            Log("START RECEIVING INIT DONE");

            var me = await bot.GetMe();

            Log($"CONNECTED AS: @{me.Username}");

            Console.WriteLine("BOT IS RUNNING. PRESS ENTER TO STOP.");
            Console.ReadLine();

            Log("STOP SIGNAL RECEIVED");
            cts.Cancel();
        }
        catch (Exception ex)
        {
            Log("FATAL ERROR:");
            Log(ex.ToString());
        }
    }

    private static async Task HandleUpdateAsync(
        ITelegramBotClient bot,
        Update update,
        CancellationToken ct)
    {
        Log("UPDATE RECEIVED");

        Log($"UPDATE TYPE: {update.Type}");

        if (update.Message is null)
        {
            Log("MESSAGE IS NULL");
            return;
        }

        Log($"CHAT ID: {update.Message.Chat.Id}");
        Log($"USER ID: {update.Message.From?.Id}");
        Log($"USERNAME: {update.Message.From?.Username}");

        if (update.Message.Text is null)
        {
            Log("TEXT IS NULL");
            return;
        }

        var text = update.Message.Text;

        Log($"TEXT: {text}");

        try
        {
            Log("SENDING RESPONSE...");

            await bot.SendMessage(
                chatId: update.Message.Chat.Id,
                text: $"ECHO: {text}",
                cancellationToken: ct
            );

            Log("MESSAGE SENT SUCCESSFULLY");
        }
        catch (Exception ex)
        {
            Log("SEND MESSAGE ERROR:");
            Log(ex.ToString());
        }
    }

    private static Task HandleErrorAsync(
        ITelegramBotClient bot,
        Exception exception,
        CancellationToken ct)
    {
        Log("TELEGRAM ERROR:");
        Log(exception.ToString());
        return Task.CompletedTask;
    }

    // ===== DEBUG LOGGER =====
    private static void Log(string msg)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {msg}");
    }
}