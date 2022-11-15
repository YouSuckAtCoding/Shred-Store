using Serilog;
namespace ShredStore.Models.Utility
{
    public class MiscellaneousUtilityClass
    {
        public IConfigurationRoot GetSettings()
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"appsettings.json");

            var config = configuration.Build();

            return config;
        }

        public string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ!@#$%¨&*()";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
        public Serilog.ILogger GetLog()
        {

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(GetSettings()).CreateLogger();
            return Log.Logger;
        }
    }
}
