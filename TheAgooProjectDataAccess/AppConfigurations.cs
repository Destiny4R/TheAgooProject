using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectDataAccess
{
    public class AppConfigurations
    {
        //Create a class to hold the application configurations
        public string SchoolName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        public string Motto { get; set; }
        public string Layout1 { get; set; }
        public string StateAddress { get; set; }
        public string Abreviation { get; set; }
        public string BaseUrl { get; set; }
        public string AdmissionPrefix { get; set; }
        public string DefaultPassword { get; set; }
    }
}
