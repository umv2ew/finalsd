using System.ComponentModel.DataAnnotations;

namespace sd.Auth.Web.Models
{
    public class RegisterViewModel
    {
        [Required, MaxLength(50)]
        public string Username { get; set; } = default!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = default!;
    }
}
