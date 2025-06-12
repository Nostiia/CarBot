using CarBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using CarBot.Utils;
using Mindee.Parsing.Standard;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

namespace CarBot.Bot
{
    public class BotMessageHandler
    {
        private readonly ITelegramBotClient _bot;
        private readonly string _token;
        private readonly OpenAIResponder _aiResponder = new();
        private readonly PassportParser _passportParser = new();
        private readonly VehicleParser _vehicleParser = new();
        private readonly ConfirmationHandler _confirmationHandler = new();
        private bool confirmationProcess = false;
        private bool isPassportConfirmed = false;
        private bool isVehicleConfirmed = false;
        private bool costConfirmationProcess = false;

        string extractedDataFromPassport;
        string extractedDataFromVehicle;

        UserInfo _userInfo = new();

        public BotMessageHandler(ITelegramBotClient bot, string token)
        {
            _bot = bot;
            _token = token;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message || update.Message == null) return;

            var message = update.Message;

            if (confirmationProcess && message.Type == MessageType.Text)
            {
                bool isConfirmingVehicle = isPassportConfirmed && !isVehicleConfirmed;

                var result = await _confirmationHandler.HandleConfirmationAsync(bot, message, cancellationToken, isConfirmingVehicle);

                if (result == ConfirmationHandler.ConfirmationResult.Confirmed)
                {
                    confirmationProcess = false;

                    if (!isPassportConfirmed)
                    {
                        isPassportConfirmed = true;
                        await bot.SendMessage(message.Chat.Id, "Passport confirmed.", cancellationToken: cancellationToken);
                        await bot.SendMessage(message.Chat.Id, "Now send the vehicle identification document.", cancellationToken: cancellationToken);
                    }
                    else if (!isVehicleConfirmed)
                    {
                        isVehicleConfirmed = true;

                        await bot.SendMessage(message.Chat.Id, "Vehicle document confirmed.", cancellationToken: cancellationToken);

                        await bot.SendMessage(message.Chat.Id, "The insurance will cost $100. Do you accept it? (Yes / No)", cancellationToken: cancellationToken);
                        costConfirmationProcess = true;
                    }
                }
                else
                {
                    confirmationProcess = false;
                }

                return;
            }

            if (costConfirmationProcess && message.Type == MessageType.Text)
            {
                var costResult = await _confirmationHandler.HandleCostConfirmationAsync(bot, message, cancellationToken, 100f);

                if (costResult == ConfirmationHandler.ConfirmationResult.Confirmed)
                {
                    costConfirmationProcess = false;
                    await bot.SendMessage(message.Chat.Id, "Insurance confirmed. Cost: $100. Thank you!", cancellationToken: cancellationToken);
                    await bot.SendMessage(message.Chat.Id, "Generating your car insurance document...", cancellationToken: cancellationToken);

                    await _aiResponder.GenerateInsuranceAsync(bot, update, cancellationToken, _userInfo);

                    await bot.SendMessage(message.Chat.Id, "Thank you. Feel free to ask any questions!", cancellationToken: cancellationToken);

                    isPassportConfirmed = false;
                    isVehicleConfirmed = false;
                }

                return;
            }

            if (message.Type == MessageType.Photo)
            {
                var photo = message.Photo.Last();
                var file = await bot.GetFile(photo.FileId, cancellationToken);
                var fileUrl = $"https://api.telegram.org/file/bot{_token}/{file.FilePath}";

                await bot.SendMessage(message.Chat.Id, "Extracting data from image...", cancellationToken: cancellationToken);

                string extractedData;

                if (!isPassportConfirmed)
                {
                    extractedData = await _passportParser.ExtractDataAsync(fileUrl, _userInfo);
                    extractedDataFromPassport = extractedData;

                }
                else if (!isVehicleConfirmed)
                {
                    extractedData = await _vehicleParser.ExtractDataAsync(fileUrl, _userInfo);
                    extractedDataFromVehicle = extractedData;
                }
                else
                {
                    await bot.SendMessage(message.Chat.Id, "All required documents have already been confirmed.", cancellationToken: cancellationToken);
                    return;
                }

                await bot.SendMessage(message.Chat.Id,
                    $"Extracted Data:\n\n{extractedData}\nPlease confirm if this is correct. (Yes / No)",
                    cancellationToken: cancellationToken);

                confirmationProcess = true;
            }
            else if (message.Type == MessageType.Text)
            {
                switch (message.Text.ToLower())
                {
                    case "/start":
                        await bot.SendMessage(message.Chat.Id, "Hello! I'm your Car Insurance Assistant Bot.\r\n\r\nYou can send me a photo of your car documents or ask questions about car insurance.");
                        await bot.SendMessage(message.Chat.Id, "Please submit a photo of your passport and then your vehicle identification document.");
                        isPassportConfirmed = false;
                        isVehicleConfirmed = false;
                        confirmationProcess = false;
                        costConfirmationProcess = false;
                        break;

                    case "/create":
                        await bot.SendMessage(message.Chat.Id, "Send your passport photo first.");
                        break;

                    default:
                        await _aiResponder.RespondAsync(bot, update, cancellationToken, message.Text);
                        break;
                }
            }
        }

        public Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Bot error: {ex.Message}");
            return Task.CompletedTask;
        }
    }

}
