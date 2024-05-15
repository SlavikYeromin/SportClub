using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportClubFinal.Data;
using SportClubFinal.Models;

namespace SportClubFinal.Controllers
{
    public class ClientSpaController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public ClientSpaController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index(string searchString)
        {

            var clientsSpa = context.ClientSpas.Include("Spass").OrderByDescending(p => p.Id).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                clientsSpa = clientsSpa.Where(n => n.LastName.Contains(searchString) || n.FirstName.Contains(searchString) || n.MiddleName.Contains(searchString) || n.BirthDate.ToString().Contains(searchString) || n.Spass.Title.Contains(searchString)).ToList();
            }

            return View(clientsSpa);
        }

        public IActionResult Create()
        {
            LoadSpaProcedure();
            return View();
        }

        [NonAction]
        private void LoadSpaProcedure()
        {
            var spas = context.Spas.ToList();
            ViewBag.Spas = new SelectList(spas, "Id", "Title");
        }

        [HttpPost]
        public IActionResult Create(ClientSpaDto clientSpaDto)
        {
            LoadSpaProcedure();

            if (clientSpaDto.Photo == null)
            {
                ModelState.AddModelError("Photo", "Поле Фото має бути заповнено");
            }

            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(clientSpaDto.Photo!.FileName);

            string imageFullPath = environment.WebRootPath + "/PhotoClients/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                clientSpaDto.Photo.CopyTo(stream);
            }

            // save the new product in the database
            ClientSpa clientSpa = new ClientSpa()
            {
                PhotoFile = newFileName,
                LastName = clientSpaDto.LastName,
                FirstName = clientSpaDto.FirstName,
                MiddleName = clientSpaDto.MiddleName,
                BirthDate = clientSpaDto.BirthDate,
                SPAId = clientSpaDto.SPAId
            };



            context.ClientSpas.Add(clientSpa);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            LoadSpaProcedure();

            if (id != 0)
            {
                NotFound();
            }

            var clientSpa = context.ClientSpas.Find(id);

            if (clientSpa == null)
            {
                return RedirectToAction("Index", "Client");
            }

            var clientSpaDto = new ClientSpaDto()
            {
                LastName = clientSpa.LastName,
                FirstName = clientSpa.FirstName,
                MiddleName = clientSpa.MiddleName,
                BirthDate = clientSpa.BirthDate,
                SPAId = clientSpa.SPAId
            };

            ViewData["ClientId"] = clientSpa.Id;
            ViewData["PhotoFile"] = clientSpa.PhotoFile;
            ViewData["BirthDate"] = clientSpa.BirthDate.ToString("MM/dd/yyyy");

            return View(clientSpaDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ClientSpaDto clientSpaDto)
        {
            LoadSpaProcedure();

            var clientSpa = context.ClientSpas.Find(id);

            if (clientSpa == null)
            {
                return RedirectToAction("Index", "ClientSpa");
            }

            if ((DateTime.Today - clientSpaDto.BirthDate).TotalDays / 365.25 < 16)
            {
                ModelState.AddModelError("BirthDate", "Несумісний вік");
            }

            ModelState.Remove("Spass");

            if (!ModelState.IsValid)
            {
                ViewData["ClientId"] = clientSpa.Id;
                ViewData["PhotoFile"] = clientSpa.PhotoFile;

                return View(clientSpaDto);
            }

            // update the image file if we have a new image file
            string newFileName = clientSpa.PhotoFile;
            if (clientSpaDto.Photo != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(clientSpaDto.Photo.FileName);

                string imageFullPath = environment.WebRootPath + "/PhotoClients/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    clientSpaDto.Photo.CopyTo(stream);
                }

                // delete the old image
                string oldImageFullPath = environment.WebRootPath + "/PhotoClients/" + clientSpa.PhotoFile;
                System.IO.File.Delete(oldImageFullPath);
            }

            clientSpa.PhotoFile = newFileName;
            clientSpa.LastName = clientSpaDto.LastName;
            clientSpa.FirstName = clientSpaDto.FirstName;
            clientSpa.MiddleName = clientSpaDto.MiddleName;
            clientSpa.BirthDate = clientSpaDto.BirthDate;
            clientSpa.SPAId =clientSpaDto.SPAId;

            context.ClientSpas.Update(clientSpa);
            context.SaveChanges();

            return RedirectToAction("Index", "ClientSpa");
        }

        public IActionResult Delete(int id)
        {
            LoadSpaProcedure();

            if (id != null)
            {
                NotFound();
            }

            var clientSpa = context.ClientSpas.Find(id);
            if (clientSpa == null)
            {
                return RedirectToAction("Index", "ClientSpa");
            }

            string imageFullPath = environment.WebRootPath + "/PhotoClients/" + clientSpa.PhotoFile;
            System.IO.File.Delete(imageFullPath);

            context.ClientSpas.Remove(clientSpa);
            context.SaveChanges(true);

            return RedirectToAction("Index", "ClientSpa");
        }
    }
}
