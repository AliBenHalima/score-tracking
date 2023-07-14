using ScoreTracking.Models;
using ScoreTracking.Validations;
using System.ComponentModel.DataAnnotations;

namespace ScoreTracking.Requests
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(100)]
        [ValidateName("users")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
    }
}
