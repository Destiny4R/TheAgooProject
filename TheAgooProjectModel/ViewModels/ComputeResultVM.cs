using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
    public class ComputeResultVM
    {
        public int TermRegId { get; set; }
        public string Fullname { get; set; }
        public string StudentRegNo { get; set; }
        public string Term { get; set; }
        public string SessionYear { get; set; }
        public string Classes { get; set; }
        public string SubClass { get; set; }
        public string SubjectsState { get; set; }
        public bool Status { get; set; } = false; 
        public bool Attendance { get; set; } = false;
        public bool PostStatus { get; set; } = false;
        public bool RemarkStatus { get; set; } = false;
    }
}
