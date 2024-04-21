using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Models.Request
{
    public class CreatePasswordRequest
    {
        public string AccountNo { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least one letter, one number, and be at least 8 characters long")]
        public string Password { get; set; }
    }



}
