using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RestSharp_Fundamentals.Helpers
{
    public class BaseConfig
    {
        private static readonly string ProjectDirectory = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(
                                                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))), "..");

        private static readonly string AppSettingsPath = Path.Combine(ProjectDirectory, "appsettings.json");

        private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
            .AddJsonFile(AppSettingsPath)
            .Build();

        public static string BaseUrl => Config["BaseUrl"];
        public static string AuthHeader => Config["AuthHeader"];
        public static string Username => Config["Username"];
        public static string Password => Config["Password"];
    }
}
