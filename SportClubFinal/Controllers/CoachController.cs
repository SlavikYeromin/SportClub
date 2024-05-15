using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportClubFinal.Data;
using SportClubFinal.Models;

namespace SportClubFinal.Controllers
{
    [Authorize(Roles = "admin,client")]
    public class CoachController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public CoachController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index(string searchString)
        {
            var coaches2 = context.Coaches.Include("FightingSports").OrderByDescending(p => p.Id).ToList();

            if(!String.IsNullOrEmpty(searchString))
            {
                coaches2 = coaches2.Where(n => n.LastName.Contains(searchString) || n.FirstName.Contains(searchString) || n.MiddleName.Contains(searchString) || n.BirthDate.ToString().Contains(searchString) || n.Email.Contains(searchString) || n.FightingSports.Title.Contains(searchString) || n.PricePerTraining.ToString().Contains(searchString)).ToList();
            }

            return View(coaches2);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadFightingSports();
            return View();
        }

        [NonAction]
        private void LoadFightingSports()
        {
            var fightingSports = context.FightingSports.ToList();
            ViewBag.FightingSports = new SelectList(fightingSports, "Id", "Title"/*, "Description", "PhotoFile"*/);
        }

        [HttpPost]
        public IActionResult Create(CoachDto coachDto)
        {
            LoadFightingSports();
   
            if (coachDto.Photo == null)
            {
                ModelState.AddModelError("Photo", "Поле Фото має бути заповнено");
            }

            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(coachDto.Photo!.FileName);

            string imageFullPath = environment.WebRootPath + "/PhotoCoaches/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                coachDto.Photo.CopyTo(stream);
            }

            // save the new product in the database
            Coach coach = new Coach()
            {
                PhotoFile = newFileName,
                LastName = coachDto.LastName,
                FirstName = coachDto.FirstName,
                MiddleName = coachDto.MiddleName,
                BirthDate = coachDto.BirthDate,
                Email = coachDto.Email,
                PricePerTraining = coachDto.PricePerTraining,
                FightingSportId = coachDto.FightingSportId,
            };

            context.Coaches.Add(coach);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            LoadFightingSports();

            if (id != null)
            {
                NotFound();
            }

            var coach = context.Coaches.Find(id);

            if (coach == null)
            {
                return RedirectToAction("Index", "Coach");
            }

            var coachDto = new CoachDto()
            {
                LastName = coach.LastName,
                FirstName = coach.FirstName,
                MiddleName = coach.MiddleName,
                BirthDate = coach.BirthDate,
                Email = coach.Email,
                PricePerTraining = coach.PricePerTraining,
                FightingSportId = coach.FightingSportId
            };

            ViewData["CoachId"] = coach.Id;
            ViewData["PhotoFile"] = coach.PhotoFile;
            ViewData["BirthDate"] = coach.BirthDate.ToString("MM/dd/yyyy");

            return View(coachDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, CoachDto coachDto)
        {
            LoadFightingSports();   
            
            var coach = context.Coaches.Find(id);

            if (coach == null)
            {
                return RedirectToAction("Index", "Coach");
            }

            if ((DateTime.Today - coachDto.BirthDate).TotalDays / 365.25 < 18)
            {
                ModelState.AddModelError("BirthDate", "Несумісний вік");
            }

            ModelState.Remove("FightingSports");
            
            if (!ModelState.IsValid)
            {
                ViewData["CoachId"] = coach.Id;
                ViewData["PhotoFile"] = coach.PhotoFile;

                return View(coachDto);
            }

            // update the image file if we have a new image file
            string newFileName = coach.PhotoFile;
            if (coachDto.Photo != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(coachDto.Photo.FileName);

                string imageFullPath = environment.WebRootPath + "/PhotoCoaches/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    coachDto.Photo.CopyTo(stream);
                }

                // delete the old image
                string oldImageFullPath = environment.WebRootPath + "/PhotoCoaches/" + coach.PhotoFile;
                System.IO.File.Delete(oldImageFullPath);
            }

            coach.PhotoFile = newFileName;
            coach.LastName = coachDto.LastName;
            coach.FirstName = coachDto.FirstName;
            coach.MiddleName = coachDto.MiddleName;
            coach.BirthDate = coachDto.BirthDate;
            coach.Email = coachDto.Email;
            coach.PricePerTraining = coachDto.PricePerTraining;
            coach.FightingSportId = coachDto.FightingSportId;

            context.Coaches.Update(coach);
            context.SaveChanges();

            return RedirectToAction("Index", "Coach");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            LoadFightingSports();

            if (id != null)
            {
                NotFound();
            }

            var coach = context.Coaches.Find(id);
            if (coach == null)
            {
                return RedirectToAction("Index", "Coach");
            }

            string imageFullPath = environment.WebRootPath + "/PhotoCoaches/" + coach.PhotoFile;
            System.IO.File.Delete(imageFullPath);

            context.Coaches.Remove(coach);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Coach");

        }
    }
}

