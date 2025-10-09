using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class GalleryTable
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string Message { get; set; }
        [StringLength(100)]
        public string PostedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public IList<ImagesTable> Imagestable { get; set; }
    }
}
