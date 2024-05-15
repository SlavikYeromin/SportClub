﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportClubFinal.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Фото")]
        public string? PhotoFile { get; set; }

        [BindProperty]
        [Display(Name = "Прізвище")]
        [Required(ErrorMessage = "Поле Прізвище має бути заповнено")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Від 2 до 40 символів")]
        [RegularExpression("[А-ЯІЇҐЄ\\s\\,\\.\\:\\'\\-]{1}[а-яіїґє\\s\\,\\.\\:\\'\\-]{1,39}", ErrorMessage = "Поле прізвище написано неправильно")]
        public string? LastName { get; set; }

        [BindProperty]
        [Display(Name = "Ім'я")]
        [Required(ErrorMessage = "Поле Ім'я має бути заповнено")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Від 2 до 30 символів")]
        [RegularExpression("[А-ЯІЇҐЄ\\s\\,\\.\\:\\'\\-]{1}[а-яіїґє\\s\\,\\.\\:\\'\\-]{1,29}", ErrorMessage = "Поле ім'я написано неправильно")]
        public string? FirstName { get; set; }

        [BindProperty]
        [Display(Name = "По-батькові")]
        [Required(ErrorMessage = "Поле Ім'я по-батькові має бути заповнено")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Від 2 до 50 символів")]
        [RegularExpression("[А-ЯІЇҐЄ\\s\\,\\.\\:\\'\\-]{1}[а-яіїґє\\s\\,\\.\\:\\'\\-]{1,49}", ErrorMessage = "Поле ім'я по-батькові написано неправильно")]
        public string? MiddleName { get; set; }

        [BindProperty]
        [Display(Name = "Дата народження")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Поле Дата народження має бути заповнено")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yyyy}")]
        public DateTime BirthDate { get; set; }

        // Связь с таблицей Виды Спорта
        [ForeignKey("FightingSports")]
        public int FightingSportId { get; set; }
        public virtual KindOfSport FightingSports { get; set; }
    }
}