using System.ComponentModel.DataAnnotations;

namespace App.e_Ticaret.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required, MinLength(10), MaxLength(250), DataType(DataType.MultilineText)]
        public string Address { get; set; } = null!;
    }
}
