using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportClubFinal.Models
{
    public class UserDto
    {
        [BindProperty]
        [Display(Name = "Електронна адреса")]
        [RegularExpression(@"(?<name>[\w.]+)\@(?<domain>\w+\.\w+)(\.\w+)?", ErrorMessage = "Некоректна адреса")]
        [Required(ErrorMessage = "Поле Електронна адреса має бути заповнено")]
        [StringLength(70)]
        public string Email { get; set; }
    }
}
