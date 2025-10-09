using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectDataAccess
{
    public interface ISmsSender
    {
        Task<string> SendMessage(string? username, string? password, string? message, string? sender_name, string? recipients);
    }
}
