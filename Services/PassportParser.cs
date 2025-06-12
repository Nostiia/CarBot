using Mindee.Input;
using Mindee.Product.Passport;
using Mindee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CarBot.Utils;

namespace CarBot.Services
{
    public class PassportParser
    {
        private readonly MindeeClient _mindeeClient;

        public PassportParser()
        {
            _mindeeClient = new MindeeClient(Environment.GetEnvironmentVariable("MINDEE_API_TOKEN"));
        }

        public async Task<string> ExtractDataAsync(string fileUrl, UserInfo _insurancePolicyGenerator)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".jpg");
            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(new Uri(fileUrl), tempPath);
            }

            var inputSource = new LocalInputSource(tempPath);
            var response = await _mindeeClient.ParseAsync<PassportV1>(inputSource);
            var document = response.Document.Inference.Prediction;

            if (document == null)
            {
                return "No data extracted from passport.";
            }

            var builder = new StringBuilder();
            builder.AppendLine("**Passport Info**");
            builder.AppendLine($"last name: {document.Surname.Value}");
            builder.AppendLine($"first name: {document.GivenNames?.FirstOrDefault().Value}");
            builder.AppendLine($"country: {document.Country?.Value}");
            builder.AppendLine($"ID number: {document.IdNumber?.Value}");
            builder.AppendLine($"birth date: {document.BirthDate?.Value}");

            _insurancePolicyGenerator.FillPasportInfo($"{document.Surname.Value} {document.GivenNames?.FirstOrDefault().Value}", $"{document.BirthDate?.Value}", $"{document.IdNumber?.Value}");

            File.Delete(tempPath);
            return builder.ToString();
        }
    }
}
