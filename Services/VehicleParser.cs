using Mindee.Input;
using Mindee.Product.Custom;
using Mindee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CarBot.Utils;
using Mindee.Product.Passport;
using Mindee.Http;
using Mindee.Product.Generated;

namespace CarBot.Services
{
    public class VehicleParser
    {
        private readonly MindeeClient _mindeeClient;

        public VehicleParser()
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
            CustomEndpoint endpoint = new CustomEndpoint(
                endpointName: "vehicle_identification",
                accountName: "Nostiia",
                version: "1");
            var response = await _mindeeClient.EnqueueAndParseAsync<GeneratedV1>(inputSource, endpoint);


            var fields = response.Document?.Inference?.Prediction?.Fields;

            if (fields == null || fields.Count() == 0)
            {
                return "No data extracted from vehicle document.";
            }

            var result = Formatter.FormatVehicleIdentificationData(fields, _insurancePolicyGenerator);
            File.Delete(tempPath);
            return result;
        }
    }
}
