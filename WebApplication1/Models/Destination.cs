using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WebApplication1.Models
{
    
public class Destination
    {
        [Key]
        public int DestinationId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(50)]
        public string PopularSeason { get; set; }

        [NotNull]
        public string ImageFileName { get; set; }

        public virtual ICollection<Route>? routes { get; set; }
    }

}