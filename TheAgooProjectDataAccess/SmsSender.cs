using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectDataAccess
{
    public class SmsSender : ISmsSender
    {
        public async Task<string> SendMessage(string? username, string? password, string? message, string? sender_name, string? recipients)
        {
            string url;
            if (sender_name == null || sender_name == string.Empty)
            {
                url = "https://portal.nigeriabulksms.com/api/?username=" + username + "&password=" + password + "&action=balance";
            }
            else
            {
                url = "https://portal.nigeriabulksms.com/api/?username=" + username + "&password=" + password + "&message=" + message + "&sender=" + sender_name + "&mobiles=" + recipients;
            }

            var request = WebRequest.Create(url);
            request.Method = "GET";

            using var webResponse = await request.GetResponseAsync();
            using var webStream = webResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = await reader.ReadToEndAsync();
            return data;
        }
    }
}
