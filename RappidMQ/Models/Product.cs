using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RappidMQ.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; } 
        public string? Name { get; set; }    
        public string? Description { get; set; }
        public int? CategoryId { get; set; } = 0;
    }
}
