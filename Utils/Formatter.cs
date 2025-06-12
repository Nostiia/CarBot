using Mindee.Parsing.Generated;
using System.Text;

namespace CarBot.Utils
{
    public static class Formatter
    {
        public static string FormatVehicleIdentificationData(Dictionary<string, GeneratedFeature> fields, UserInfo userInfo)
        {
            var builder = new StringBuilder();
            builder.AppendLine("**Vehicle Identification Data**");

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
                    case ("brand"):
                        userInfo.Brand = value;
                        break;

                    case ("capacity"):
                        userInfo.Capacity = value;
                        break;

                    case ("color"):
                        userInfo.VehicleColor = value;
                        break;

                    case ("vehicle weight"):
                        userInfo.Weight = value;
                        break;

                    case ("vehicle identification number"):
                        userInfo.VIN = value;
                        break;

                    default:
                        break;
                }
            }

            return builder.ToString();
        }
    }

}
