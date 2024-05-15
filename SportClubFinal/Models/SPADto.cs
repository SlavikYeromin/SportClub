using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SportClubFinal.Models
{
    public class SPADto
    {
        public IFormFile? Photo { get; set; }

        [DisplayName("Spa процедура")]
        [Required(ErrorMessage = "Поле Назва має бути заповнено")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Від 2 до 50 символів")]
        [RegularExpression("[А-ЯІЇҐЄ\\s\\,\\.\\:\\'\\-]{1}[а-яіїґє\\s\\,\\.\\:\\'\\-]{1,49}", ErrorMessage = "Поле назва написано неправильно")]
        public string Title { get; set; }

        [BindProperty]
        [Display(Name = "Опис")]
        [Required(ErrorMessage = "Поле Опис має бути заповнено")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Від 2 до 1000 символів")]
        [RegularExpression("[А-ЯІЇҐЄа-яіїґє0-9\\s\\,\\.\\:\\'\\%]{1,999}", ErrorMessage = "Поле опис написано неправильно")]
        public string Description { get; set; }
    }
}
