using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportClubFinal.Models
{
    public class CommentDto
    {
        [Display(Name = "Коментар")]
        [Required(ErrorMessage = "Поле коментар має бути заповнено")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Від 2 до 1000 символів")]
        [RegularExpression("[А-ЯІЇҐЄа-яіїґє0-9\\s\\,\\.\\:\\'\\(\\)\\!\\-\\+]{1,999}", ErrorMessage = "Поле опис написано неправильно")]
        public string CommentDescription { get; set; }

        // Связь с таблицей тренеры
        [ForeignKey("Coaches")]
        public int CoachId { get; set; }
        public virtual Coach Coaches { get; set; }
    }
}
