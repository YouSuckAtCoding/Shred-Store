using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using System.Net.Mail;
using System.Text;

namespace ShredStore.Models.Utility
{
    public class EmailSender
    {

        private readonly MiscellaneousUtilityClass utilityClass;
        public EmailSender(MiscellaneousUtilityClass utilityClass)
        {
            this.utilityClass = utilityClass;
        }

        private string[] EmailType(int selected)
        {
            var config = utilityClass.GetSettings();
            string[] emailInfo = new string[2];
            switch (selected)
            {
                case 1:
                    emailInfo[0] = config.GetValue<string>("PageLayouts:ResetPassword");
                    emailInfo[1] = "Password Reset";
                    return emailInfo;
                case 2:
                    emailInfo[0] = config.GetValue<string>("PageLayouts:AfterRegistration");
                    emailInfo[1] = "Welcome to our shop!";
                    return emailInfo;
                default:
                    break;
            }
            emailInfo[0] = "No Templates";
            return emailInfo;
        }
        private SmtpSender GetSender()
        {
            var sender = new SmtpSender(() => new SmtpClient("localhost")
            {
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 25
                //DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                //PickupDirectoryLocation = @"C:\Demos"
            });
            return sender;
        }
        
        private StringBuilder CreateEmail()
        {
            StringBuilder template = new();
            template.AppendLine("Dear @Model.FirstName,");
            template.AppendLine("<p>Here is your password reset.</p>");
            template.AppendLine("- Shred Store 0/");

            return template;
        }
        /// <summary>
        /// Reset Password page requires a model.Sends an email to the user. The template parameter indicates which type of email is sent.
        /// 1 - > Password Reset ||
        /// 2 - > After Registration ||
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="template"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string emailAddress, int template, UserResetPasswordViewModel user)
        {
            Email.DefaultSender = GetSender();
            var projectRoot = Directory.GetCurrentDirectory();
            Email.DefaultRenderer = new RazorRenderer(projectRoot);
            string[] emailInfo = EmailType(template);
            if (emailInfo[0] != "NoTemplates")
            {
                try
                {
                    var email = await Email
                    .From("shredstore@shred.com")
                    .To(emailAddress, emailAddress)
                    .Subject(emailInfo[1])
                    .UsingTemplateFromFile(emailInfo[0], user)
                    //.Body("Thanks for buying our product.")
                    .SendAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Sends an email to the user. The template parameter indicates which type of email is sent.
        /// 1 - > Password Reset ||
        /// 2 - > After Registration ||
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="template">
        /// </param>
        /// <returns></returns>
        public async Task SendEmailAsync(string emailAddress, int template)
        {
            Email.DefaultSender = GetSender();
            Email.DefaultRenderer = new RazorRenderer();
            string[] emailInfo = EmailType(template);
            if (emailInfo[0] != "NoTemplates")
            {
                try
                {
                    var email = await Email
                    .From("shredstore@shred.com")
                    .To(emailAddress, emailAddress)
                    .Subject(emailInfo[1])
                    .UsingTemplateFromFile(emailInfo[0], new {})
                    //.Body("Thanks for buying our product.")
                    .SendAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }

}

