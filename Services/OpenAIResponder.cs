﻿using Betalgo.Ranul.OpenAI.Managers;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using CarBot.Utils;
using System.Threading;

namespace CarBot.Services
{
    public class OpenAIResponder
    {
        private readonly OpenAIService _openAIService;

        public OpenAIResponder()
        {

            _openAIService = new OpenAIService(new OpenAIOptions
            {
                ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")

            });
        }

        public OpenAIService GetAIService()
        {
            return _openAIService;
        }

        public async Task RespondAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken, string userMessage)
        {
            var request = new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("You are a helpful assistant for car insurance."),
                ChatMessage.FromUser(userMessage)
            },
                Model = "gpt-3.5-turbo"
            };

            var result = await _openAIService.ChatCompletion.CreateCompletion(request);
            var response = result.Successful ? result.Choices.First().Message.Content : "Error: Unable to generate response.";
            await bot.SendMessage(update.Message.Chat.Id, response, cancellationToken: cancellationToken);
        }

        public async Task GenerateInsuranceAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken, UserInfo _userInfo)
        {
            string prompt = $@"
                        Generate a detailed car insurance policy text including the following info:
                        
                        Full Name: {_userInfo.FullName}
                        Date of Birth: {_userInfo.BirthDate}
                        ID Number: {_userInfo.IdNumber}

                        Vehicle registration number: {_userInfo.VIN}
                        Registration Date: {_userInfo.RegistrationDate}
                        Release year: {_userInfo.ReleaseYear}
                        Surname: {_userInfo.Surname}
                        Given Names: {_userInfo.GivenNames}
                        Cost: ${_userInfo.Cost}
                        
                        Date: {DateTime.Now}
                        CompanyName: Telegram CarInsuranceBot
                        Write it as a formal insurance document.";

            var completionResult = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                        {
                            new ChatMessage("system", "You are an expert insurance policy generator."),
                            new ChatMessage("user", prompt)
                        },
                MaxTokens = 500,
                Temperature = 0.7f,
                Model = "gpt-3.5-turbo",
            });

            string generatedPolicyText = completionResult.Choices[0].Message.Content;

            byte[] pdfBytes = PdfHelper.GeneratePdfFromText(generatedPolicyText);

            using var stream = new MemoryStream(pdfBytes);
            await bot.SendDocument(update.Message.Chat.Id, new Telegram.Bot.Types.InputFileStream(stream, "InsurancePolicy.pdf"), cancellationToken: cancellationToken);
        }
    }

}
