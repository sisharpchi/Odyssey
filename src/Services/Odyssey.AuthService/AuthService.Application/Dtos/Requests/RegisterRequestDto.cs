using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.Dtos.Requests
{
    public class RegisterRequestDto
    {
        [Required]
        public string FirstName { get; set; } = default!;
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = default!;
    }
}
