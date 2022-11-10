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
    }
}
