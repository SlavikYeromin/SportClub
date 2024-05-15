using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportClubFinal.Models
{
    public class CoachDto
    {

        [Display(Name = "Фото")]
        public IFormFile? Photo { get; set; }

        [Display(Name = "Прізвище")]
        [Required(ErrorMessage = "Поле Прізвище має бути заповнено")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Від 2 до 40 символів")]
        [RegularExpression("[А-ЯІЇҐЄ\\s\\,\\.\\:\\'\\-]{1}[а-яіїґє\\s\\,\\.\\:\\'\\-]{1,39}", ErrorMessage = "Поле прізвище написано неправильно")]
        public string? LastName { get; set; }

        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Поле Ім'я має бути заповнено")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Від 2 до 30 символів")]
        [RegularExpression("[А-ЯІЇҐЄ\\s\\,\\.\\:\\'\\-]{1}[а-яіїґє\\s\\,\\.\\:\\'\\-]{1,29}", ErrorMessage = "Поле ім'я написано неправильно")]
        public string? FirstName { get; set; }

        [Display(Name = "По-батькові")]
        [Required(ErrorMessage = "Поле Ім'я по-батькові має бути заповнено")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Від 2 до 50 символів")]
        [RegularExpression("[А-ЯІЇҐЄ\\s\\,\\.\\:\\'\\-]{1}[а-яіїґє\\s\\,\\.\\:\\'\\-]{1,49}", ErrorMessage = "Поле ім'я по-батькові написано неправильно")]
        public string? MiddleName { get; set; }

        [Display(Name = "Дата народження")]
        [Required(ErrorMessage = "Поле Дата народження має бути заповнено")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Електронна адреса")]
        [RegularExpression(@"(?<name>[\w.]+)\@(?<domain>\w+\.\w+)(\.\w+)?", ErrorMessage = "Некоректна електронна адреса")]
        [Required(ErrorMessage = "Поле Електронна адреса має бути заповнено")]
        [StringLength(70)]
        [EmailAddress(ErrorMessage = "Не є дійсною адресою електронної пошти")]
        public string? Email { get; set; }

        [Display(Name = "Ціна за 1 тренування")]
        [Required(ErrorMessage = "Поле Ціна має бути заповнено")]
        public decimal PricePerTraining { get; set; }

        // Связь с таблицей Боевые искусства
        [ForeignKey("FightingSports")]
        public int FightingSportId { get; set; }
        public virtual KindOfSport FightingSports { get; set; }
    }
}
