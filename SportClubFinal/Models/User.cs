using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Identity;

namespace SportClubFinal.Models
{
    public class User : IdentityUser
    {
        [Key]
        public int Id { get; set; }

        [BindProperty]
        [Display(Name = "Електронна адреса")]
        [RegularExpression(@"(?<name>[\w.]+)\@(?<domain>\w+\.\w+)(\.\w+)?", ErrorMessage ="Некоректна адреса")]
        [Required(ErrorMessage = "Поле Електронна адреса має бути заповнено")]
        [StringLength(70)]
        public string Email { get; set; }
    }
}

