using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectDataAccess
{
    public interface IMailSender
    {
        Task<bool> SendEmailAsync(string email, string cc, string subject, string htmlMessage, string username, string password);
    }
}
