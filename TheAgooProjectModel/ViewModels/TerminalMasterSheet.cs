using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel.ViewModels
{
    public class TerminalMasterSheet
    {
        public string Fullname { get; set; }
        public string StudentReg { get; set; }
        public string Subject { get; set; }
        public string SessionYear { get; set; }
        public string Term { get; set; }
        public string ClassSub { get; set; }
        public double Total { get; set; }
        public double Average { get; set; }
        public int Position { get; set; }
        public string Remark { get; set; }
    }
}
