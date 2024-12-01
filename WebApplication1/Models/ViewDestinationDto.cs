using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Models
{
    public class ViewDestinationDto
    {
        
        public int DestinationId { get; set; }

       
        public string Name { get; set; }

       
        public string Description { get; set; }

        public string Location { get; set; }

      
        public string PopularSeason { get; set; }

        
        public string ImageFileName { get; set; }
    }
}
