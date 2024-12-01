namespace WebApplication1.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
 

    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; }


       
        public int? UserId { get; set; }

        public virtual User? User { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        [NotNull]
        public string Description { get; set; }
    }

}