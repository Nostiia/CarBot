using CarBot.Bot;
using DotNetEnv;

Env.Load();
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

var bot = new BotService();
bot.Start();

Console.WriteLine("Bot is running. Press any key to exit.");
