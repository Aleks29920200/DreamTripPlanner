using System.ComponentModel.DataAnnotations;


namespace WebApplication1.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }

        public virtual ICollection<User>? users { get; set; }
    }
}
