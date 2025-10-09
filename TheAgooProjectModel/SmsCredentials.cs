using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class SmsCredentials
    {
        public const string Credentials = "Credentials";
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class EmailCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
