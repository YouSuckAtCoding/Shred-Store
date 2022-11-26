using Serilog;
using System.Net.Mail;
namespace ShredStore.Models.Utility
{
    public class MiscellaneousUtilityClass
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        
        public MiscellaneousUtilityClass(IWebHostEnvironment _hostEnvironment)
        {
            this._hostEnvironment =  _hostEnvironment;
        }

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

        public async Task<string> UploadImage(IFormFile image)
        {
            string rootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(image.FileName);
            string fileExtension = Path.GetExtension(image.FileName);
            string ImageName = fileName + fileExtension;
            string path = Path.Combine(rootPath + "/Images/", ImageName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return ImageName;
        }

        public string DeleteImage(string image)
        {
            string rootPath = _hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath + "/Images/", image);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                try
                {
                    file.Delete();
                    return "Ok";
                }
                catch (Exception ex)
                {

                    return "Error";
                }

            }
            else
            {
                return "Error";
            }
        }

        public List<string> SetProductList(List<string> prods)
        {
            List<string> prodList = new List<string>();
            foreach (var pro in prods)
            {
                int dupeCount = 0;
                foreach (var prod in prods)
                {
                    if (pro == prod)
                    {
                        dupeCount++;
                    }
                }
                prodList.Add(pro + " " + "x" + dupeCount);
            }

            return prodList.Distinct().ToList();
        }

        public bool IsEmailValid(string Email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(Email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }

        }


    }
}
