using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ScoreTracking.Models
{
    [Table("users")]
    public class User : Base
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
