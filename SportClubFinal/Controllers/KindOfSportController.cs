using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportClubFinal.Data;
using SportClubFinal.Models;

namespace SportClubFinal.Controllers
{
    [Authorize(Roles = "admin,client")]
    public class KindOfSportController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public KindOfSportController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index(string searchString)
        {
            var fightingSports = context.FightingSports.OrderByDescending(p => p.Id).ToList();

            if(!String.IsNullOrEmpty(searchString))
            {
                fightingSports = fightingSports.Where(n => n.Title.Contains(searchString)).ToList();
            }

            return View(fightingSports);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(KindOfSportDto fightingSportDto)
        {
            if (fightingSportDto.Photo == null)
            {
                ModelState.AddModelError("Photo", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(fightingSportDto);
            }

            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(fightingSportDto.Photo!.FileName);

            string imageFullPath = environment.WebRootPath + "/PhotoKindOfSport/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                fightingSportDto.Photo.CopyTo(stream);
            }

            // save the new product in the database
            KindOfSport fightingSport = new KindOfSport()
            {
                PhotoFile = newFileName,
                Title = fightingSportDto.Title,
                Description = fightingSportDto.Description
            };

            context.FightingSports.Add(fightingSport);
            context.SaveChanges();

            return RedirectToAction("Index", "KindOfSport");
        }

        public IActionResult Edit(int id)
        {
            var fightingSport = context.FightingSports.Find(id);

            if (fightingSport == null)
            {
                return RedirectToAction("Index", "KindOfSport");
            }

            var fightingSportDto = new KindOfSportDto()
            {
                Title = fightingSport.Title,
                Description = fightingSport.Description,
            };

            ViewData["FightingSportId"] = fightingSport.Id;
            ViewData["PhotoFile"] = fightingSport.PhotoFile;

            return View(fightingSportDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, KindOfSportDto fightingSportDto)
        {
            var fightingSport = context.FightingSports.Find(id);

            if (fightingSport == null)
            {
                return RedirectToAction("Index", "KindOfSport");
            }

            if(!ModelState.IsValid)
            {
                ViewData["FightingSportId"] = fightingSport.Id;
                ViewData["PhotoFile"] = fightingSport.PhotoFile;

                return View(fightingSportDto);
            }

            // update the image file if we have a new image file
            string newFileName = fightingSport.PhotoFile;
            if(fightingSportDto.Photo != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(fightingSportDto.Photo.FileName);

                string imageFullPath = environment.WebRootPath + "/PhotoKindOfSport/" + newFileName;
                using(var stream = System.IO.File.Create(imageFullPath)) 
                {
                    fightingSportDto.Photo.CopyTo(stream);
                }

                // delete the old image
                string oldImageFullPath = environment.WebRootPath + "/PhotoKindOfSport/" + fightingSport.PhotoFile;
                System.IO.File.Delete(oldImageFullPath);
            }

            // update the product in the database
            fightingSport.PhotoFile = newFileName;
            fightingSport.Title = fightingSportDto.Title;
            fightingSport.Description = fightingSportDto.Description;

            context.SaveChanges();

            return RedirectToAction("Index", "KindOfSport");
        }

        public IActionResult Delete(int id)
        {
            var fightingSport = context.FightingSports.Find(id);
            if(fightingSport == null)
            {
                return RedirectToAction("Index", "KindOfSport");
            }

            string imageFullPath = environment.WebRootPath + "/PhotoKindOfSport/" + fightingSport.PhotoFile;
            System.IO.File.Delete(imageFullPath);

            context.FightingSports.Remove(fightingSport);
            context.SaveChanges(true);

            return RedirectToAction("Index", "KindOfSport");
        }
    }
}
