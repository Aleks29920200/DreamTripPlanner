using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Models
{
    public class GroupDto
    {

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }
        public int? UserId { get; set; }

        public virtual User? User { get; set; }

      
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        [NotNull]
        public string Description { get; set; }
    }
}
