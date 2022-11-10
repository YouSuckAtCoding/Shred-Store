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

        private string EmailType(int selected)
        {
            var config = utilityClass.GetSettings();
            
            switch (selected)
            {
                case 1:
                    return config.GetValue<string>("PageLayouts:ResetPassword");
                case 2:
                    return config.GetValue<string>("PageLayouts:AfterRegistrationTemplate");
                default:
                    break;
            }
            return "NoTemplates";
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
            string emailTemplate = EmailType(template);
            if (emailTemplate != "NoTemplates")
            {
                try
                {
                    var email = await Email
                    .From("shredstore@shred.com")
                    .To(emailAddress, "Customer")
                    .Subject("Password Reset!")
                    .UsingTemplateFromFile(emailTemplate, new { FirstName = emailAddress })
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

