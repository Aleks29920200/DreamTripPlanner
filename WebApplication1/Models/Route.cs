using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class Route
    {
        

        [Key]
        public int RouteId { get; set; }

        [Required]
        [StringLength(100)]
        public string RouteName { get; set; }
        public int UserId { get; set; }

        [ForeignKey("StartDestination")]
        public int? StartDestinationId { get; set; }
        public virtual Destination? StartDestination { get; set; }

        [ForeignKey("EndDestination")]
        public int? EndDestinationId { get; set; }
        public virtual Destination? EndDestination { get; set; }
        [AllowNull]
        public double StartLatitude { get; set; }
        [AllowNull]
        public double StartLongitude { get; set; }
        [AllowNull]
        public double EndLatitude { get; set; }
        [AllowNull]
        public double EndLongitude { get; set; }

        public virtual User? User { get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string Description { get; set; }
    }
}
