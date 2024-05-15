using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportClubFinal.Data;
using SportClubFinal.Models;
using System.Data;

namespace SportClubFinal.Controllers
{
    [Authorize(Roles = "admin,client")]
    public class SPAController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public SPAController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index(string searchString)
        {
            var SPA = context.Spas.OrderByDescending(p => p.Id).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                SPA = SPA.Where(n => n.Title.Contains(searchString)).ToList();
            }

            return View(SPA);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SPADto spaDto)
        {
            if (spaDto.Photo == null)
            {
                ModelState.AddModelError("Photo", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(spaDto);
            }

            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(spaDto.Photo!.FileName);

            string imageFullPath = environment.WebRootPath + "/PhotoSPA/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                spaDto.Photo.CopyTo(stream);
            }

            // save the new product in the database
            SPA spa = new SPA()
            {
                PhotoFile = newFileName,
                Title = spaDto.Title,
                Description = spaDto.Description
            };

            context.Spas.Add(spa);
            context.SaveChanges();

            return RedirectToAction("Index", "SPA");
        }

        public IActionResult Edit(int id)
        {
            var spa = context.Spas.Find(id);

            if (spa == null)
            {
                return RedirectToAction("Index", "SPA");
            }

            var spaDto = new SPADto()
            {
                Title = spa.Title,
                Description = spa.Description,
            };

            ViewData["SPAId"] = spa.Id;
            ViewData["PhotoFile"] = spa.PhotoFile;

            return View(spaDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, SPADto spaDto)
        {
            var spa = context.Spas.Find(id);

            if (spa == null)
            {
                return RedirectToAction("Index", "SPA");
            }

            if (!ModelState.IsValid)
            {
                ViewData["SPAId"] = spa.Id;
                ViewData["PhotoFile"] = spa.PhotoFile;

                return View(spaDto);
            }

            // update the image file if we have a new image file
            string newFileName = spa.PhotoFile;
            if (spaDto.Photo != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(spaDto.Photo.FileName);

                string imageFullPath = environment.WebRootPath + "/PhotoSPA/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    spaDto.Photo.CopyTo(stream);
                }

                // delete the old image
                string oldImageFullPath = environment.WebRootPath + "/PhotoSPA/" + spa.PhotoFile;
                System.IO.File.Delete(oldImageFullPath);
            }

            // update the product in the database
            spa.PhotoFile = newFileName;
            spa.Title = spaDto.Title;
            spa.Description = spaDto.Description;

            context.SaveChanges();

            return RedirectToAction("Index", "SPA");
        }

        public IActionResult Delete(int id)
        {
            var spa = context.Spas.Find(id);
            if (spa == null)
            {
                return RedirectToAction("Index", "SPA");
            }

            string imageFullPath = environment.WebRootPath + "/PhotoSPA/" + spa.PhotoFile;
            System.IO.File.Delete(imageFullPath);

            context.Spas.Remove(spa);
            context.SaveChanges(true);

            return RedirectToAction("Index", "SPA");
        }
    }
}
