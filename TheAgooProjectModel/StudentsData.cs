using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheAgooProjectModel
{
    public class StudentsData
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? StudentRegNo { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string? OtherName { get; set; }
        [MaxLength(50)]
        public string SurName { get; set; }
        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Address { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [MaxLength(50)]
        public string State { get; set; }
        [Required]
        [MaxLength(50)]
        public string LocalGov { get; set; }
        public string Passport { get; set; }
        [MaxLength(150)]
        public string UserID { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public int? SessionYearId { get; set; }
        [ForeignKey(nameof(SessionYearId))]
        public SessionYear SessionYear { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
