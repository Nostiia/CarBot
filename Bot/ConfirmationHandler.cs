using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using static CarBot.Bot.ConfirmationHandler;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.Managers;
using CarBot.Utils;

namespace CarBot.Bot
{
    public class ConfirmationHandler
    {
        public enum ConfirmationResult { None, Confirmed }

        public async Task<ConfirmationResult> HandleConfirmationAsync(
            ITelegramBotClient bot,
            Message message,
            CancellationToken cancellationToken,
            bool everythingConfirmed)
        {
            var input = message.Text.Trim().ToLower();

            if (input == "yes")
                return ConfirmationResult.Confirmed;
            
            else if (input == "no")
            {
                await bot.SendMessage(message.Chat.Id, "Please retake and resend the document.", cancellationToken: cancellationToken);
            }
            else
            {
                await bot.SendMessage(message.Chat.Id, "Reply with 'Yes' or 'No'.", cancellationToken: cancellationToken);
            }

            return ConfirmationResult.None;
        }

        public async Task<ConfirmationResult> HandleCostConfirmationAsync(
            ITelegramBotClient bot,
            Message message,
            CancellationToken cancellationToken,
            float cost
        )
        {
            var input = message.Text.Trim().ToLower();

            if (input == "yes")
            {
                return ConfirmationResult.Confirmed;
            }
            else if (input == "no")
            {
                await bot.SendMessage(message.Chat.Id, $"Sorry, but the only available cost is ${cost}. Do you agree (Yes / No)?", cancellationToken: cancellationToken);
            }
            else
            {
                await bot.SendMessage(message.Chat.Id, "Please reply with 'Yes' or 'No'.", cancellationToken: cancellationToken);
            }

            return ConfirmationResult.None;
        }

    }
}
