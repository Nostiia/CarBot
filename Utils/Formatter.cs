using Mindee.Parsing.Generated;
using System.Text;

namespace CarBot.Utils
{
    public static class Formatter
    {
        public static string FormatVehicleIdentificationData(Dictionary<string, GeneratedFeature> fields, UserInfo userInfo)
        {
            var builder = new StringBuilder();
            builder.AppendLine("**Vehicle Registration Data**");

            foreach (var field in fields)
            {
                if (field.Key?.ToLower().Contains("classification") == true)
                    continue;

                var name = field.Key?.Replace("_", " ") ?? "Unknown";
                var value = field.Value?.ToString() ?? "N/A";

                string fieldText = $"{name}: {value}";

                fieldText = fieldText.Replace(":value:", "");

                fieldText = fieldText.Replace("\n", "").Trim();

                builder.AppendLine(fieldText);

                switch (name.ToLower())
                {
                    case ("vehicle registration number"):
                        userInfo.VIN = value;
                        break;

                    case ("registration date"):
                        userInfo.RegistrationDate = value;
                        break;

                    case ("release year"):
                        userInfo.ReleaseYear = value;
                        break;

                    case ("surname"):
                        userInfo.Surname = value;
                        break;

                    case ("given names"):
                        userInfo.GivenNames = value;
                        break;

                    default:
                        break;
                }
            }

            return builder.ToString();
        }
    }

}
