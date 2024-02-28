using System.ComponentModel.DataAnnotations;

namespace Talabat.APis.DTOS
{
    public class RegisterDto
    {
        [Required]
        public String DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
