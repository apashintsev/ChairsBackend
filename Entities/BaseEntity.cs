using System.ComponentModel.DataAnnotations;

namespace ChairsBackend.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
