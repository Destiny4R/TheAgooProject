using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAgooProjectModel;

namespace TheAgooProjectModel  
{
    public class ParentStudent
    {
        public int Id { get; set; }
        public int StudentsdataId { get; set; }
        [ForeignKey(nameof(StudentsdataId))]
        public StudentsData Studentsdata { get; set; }
        public int ParentTableId { get; set; }
        [ForeignKey(nameof(ParentTableId))]
        public ParentTable ParentTable { get; set; }
    }
}
