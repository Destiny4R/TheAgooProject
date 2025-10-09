using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TheAgooProjectModel
{
    public class ImagesTable
    {
        public int Id { get; set; }
        [StringLength(700)]
        public string Name { get; set; }
        public int GalleryId { get; set; }
        [ForeignKey(nameof(GalleryId))]
        public GalleryTable Gallery { get; set; }
    }
}
