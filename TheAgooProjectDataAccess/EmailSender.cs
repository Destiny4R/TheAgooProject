using System.Net.Mail;
using System.Net;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;


namespace TheAgooProjectDataAccess
{
    public class EmailSender : IMailSender
    {
        private readonly IOptions<EmailCredentials> emailCredentials;

        public EmailSender(IOptions<EmailCredentials> emailCredentials)
        {
            this.emailCredentials = emailCredentials;
        }
        public async Task<bool> SendEmailAsync(string email, string cc, string subject, string htmlMessage, string username, string password)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                //create the mail message 
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(username); //IMPORTANT: This must be same as your smtp authentication address.
                mail.To.Add(email);
                if (!string.IsNullOrEmpty(cc))
                {
                    mail.CC.Add(cc);
                }
                else
                {
                    mail.CC.Add(email);
                }

                mail.BodyEncoding = Encoding.UTF8;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlMessage);
                htmlView.ContentType = new System.Net.Mime.ContentType("text/html");
                mail.AlternateViews.Add(htmlView);
                mail.Subject = subject;
                SmtpClient smtp = new SmtpClient(emailCredentials.Value.Host, emailCredentials.Value.Port);

                //IMPORANT:  Your smtp login email MUST be same as your FROM address. 
                NetworkCredential Credentials = new NetworkCredential(username, password);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = Credentials;
                smtp.Port = emailCredentials.Value.Port;    //alternative port number is 8889
                smtp.EnableSsl = emailCredentials.Value.EnableSsl;
                await smtp.SendMailAsync(mail);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                // Handle SMTP-specific exceptions
                Console.WriteLine($"SMTP Exception: {smtpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"General Exception: {ex.Message}");
                return false;
            }
        }

        public async Task SendEmailAsync3(string email, string subject, string htmlMessage, string username, string password)
        {

            using (var client = new SmtpClient(emailCredentials.Value.Host, emailCredentials.Value.Port))
            {
                client.EnableSsl = true; // Enable SSL for secure connection
                client.Credentials = new NetworkCredential(username, password);
                client.Timeout = 400000;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlMessage);
                htmlView.ContentType = new System.Net.Mime.ContentType("text/html");
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(username),
                    To = { new MailAddress(email) },
                    Subject = subject,
                    IsBodyHtml = true
                };
                //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlMessage);
                //htmlView.ContentType = new System.Net.Mime.ContentType("text/html");
                mailMessage.AlternateViews.Add(htmlView);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
