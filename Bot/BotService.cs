using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace CarBot.Bot
{
    public class BotService
    {
        private readonly TelegramBotClient _botClient;
        private readonly BotMessageHandler _messageHandler;
        private readonly ReceiverOptions _receiverOptions;

        public BotService()
        {
            var token = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN");
            _botClient = new TelegramBotClient(token);
            _messageHandler = new BotMessageHandler(_botClient, token);
            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
        }

        public void Start()
        {
            _botClient.StartReceiving(
                _messageHandler.HandleUpdateAsync,
                _messageHandler.HandleErrorAsync,
                _receiverOptions,
                cancellationToken: new CancellationTokenSource().Token
            );
        }
    }
}
