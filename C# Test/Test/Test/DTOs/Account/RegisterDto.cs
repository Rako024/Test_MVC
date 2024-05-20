using System.ComponentModel.DataAnnotations;

namespace Test.DTOs.Account
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required]
        public string ConPassword { get; set; }
    }
}
