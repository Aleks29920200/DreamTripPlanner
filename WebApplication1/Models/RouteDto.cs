

namespace WebApplication1.Models
{
    [Serializable]
    internal class RouteDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
       
        public List<double[]>? Coordinates { get; set; }


    }
    

}