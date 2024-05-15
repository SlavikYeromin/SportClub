using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportClubFinal.Models
{
    public class KindOfSport
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Фото")]
        public string PhotoFile { get; set; }

        [DisplayName("Вид спорту")]
        [Required(ErrorMessage = "Поле Назва має бути заповнено")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Від 2 до 50 символів")]
        [RegularExpression("[А-ЯІЇҐЄ\\s\\,\\.\\:\\'\\-]{1}[а-яіїґє\\s\\,\\.\\:\\'\\-]{1,49}", ErrorMessage = "Поле назва написано неправильно")]
        public string Title { get; set; }

        [BindProperty]
        [Display(Name = "Опис")]
        [Required(ErrorMessage = "Поле Опис має бути заповнено")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Від 2 до 1000 символів")]
        [RegularExpression("[А-ЯІЇҐЄа-яіїґє\\s\\,\\.\\:\\']{1,999}", ErrorMessage = "Поле опис написано неправильно")]
        public string Description { get; set; }
    }
}
