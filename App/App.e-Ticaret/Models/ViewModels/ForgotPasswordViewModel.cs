using System.ComponentModel.DataAnnotations;

namespace App.e_Ticaret.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required, MaxLength(256), EmailAddress]
        public string Email { get; set; } = null!;
    }
}
