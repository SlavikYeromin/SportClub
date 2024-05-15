using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportClubFinal.Data;
using SportClubFinal.Models;
using System.Data;

namespace SportClubFinal.Controllers
{
    public class ClientController : Controller
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public ClientController(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index(string searchString)
        {

            var clients = context.Clients.Include("FightingSports").OrderByDescending(p => p.Id).ToList();

            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(n => n.LastName.Contains(searchString) || n.FirstName.Contains(searchString) || n.MiddleName.Contains(searchString) || n.BirthDate.ToString().Contains(searchString) || n.FightingSports.Title.Contains(searchString)).ToList();
            }

            return View(clients);
        }

        public IActionResult Create()
        {
            LoadFightingSports();
            return View();
        }

        [NonAction]
        private void LoadFightingSports()
        {
            var fightingSports = context.FightingSports.ToList();
            ViewBag.FightingSports = new SelectList(fightingSports, "Id", "Title");
        }

        [HttpPost]
        public IActionResult Create(ClientDto clientDto)
        {
            LoadFightingSports();

            if (clientDto.Photo == null)
            {
                ModelState.AddModelError("Photo", "Поле Фото має бути заповнено");
            }

            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(clientDto.Photo!.FileName);

            string imageFullPath = environment.WebRootPath + "/PhotoClients/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                clientDto.Photo.CopyTo(stream);
            }

            // save the new product in the database
            Client client = new Client()
            {
                PhotoFile = newFileName,
                LastName = clientDto.LastName,
                FirstName = clientDto.FirstName,
                MiddleName = clientDto.MiddleName,
                BirthDate = clientDto.BirthDate,
                FightingSportId = clientDto.FightingSportId
            };

            

            context.Clients.Add(client);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            LoadFightingSports();

            if(id != 0)
            {
                NotFound();
            }

            var client = context.Clients.Find(id);

            if (client == null)
            {
                return RedirectToAction("Index", "Client");
            }

            var clientDto = new ClientDto()
            {
                LastName = client.LastName,
                FirstName = client.FirstName,
                MiddleName = client.MiddleName,
                BirthDate = client.BirthDate,
                FightingSportId = client.FightingSportId
            };

            ViewData["ClientId"] = client.Id;
            ViewData["PhotoFile"] = client.PhotoFile;
            ViewData["BirthDate"] = client.BirthDate.ToString("MM/dd/yyyy");

            return View(clientDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ClientDto clientDto)
        {
            LoadFightingSports();

            var client = context.Clients.Find(id);

            if (client == null)
            {
                return RedirectToAction("Index", "Client");
            }

            if ((DateTime.Today - clientDto.BirthDate).TotalDays / 365.25 < 16)
            {
                ModelState.AddModelError("BirthDate", "Несумісний вік");
            }

            ModelState.Remove("FightingSports");

            if (!ModelState.IsValid)
            {
                ViewData["ClientId"] = client.Id;
                ViewData["PhotoFile"] = client.PhotoFile;

                return View(clientDto);
            }

            // update the image file if we have a new image file
            string newFileName = client.PhotoFile;
            if (clientDto.Photo != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(clientDto.Photo.FileName);

                string imageFullPath = environment.WebRootPath + "/PhotoClients/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    clientDto.Photo.CopyTo(stream);
                }

                // delete the old image
                string oldImageFullPath = environment.WebRootPath + "/PhotoClients/" + client.PhotoFile;
                System.IO.File.Delete(oldImageFullPath);
            }

            client.PhotoFile = newFileName;
            client.LastName = clientDto.LastName;
            client.FirstName = clientDto.FirstName;
            client.MiddleName = clientDto.MiddleName;
            client.BirthDate = clientDto.BirthDate;
            client.FightingSportId = clientDto.FightingSportId;

            context.Clients.Update(client);
            context.SaveChanges();

            return RedirectToAction("Index", "Client");
        }

        public IActionResult Delete(int id)
        {
            LoadFightingSports();

            if(id != null)
            {
                NotFound();
            }

            var client = context.Clients.Find(id);
            if (client == null)
            {
                return RedirectToAction("Index", "Client");
            }

            string imageFullPath = environment.WebRootPath + "/PhotoClients/" + client.PhotoFile;
            System.IO.File.Delete(imageFullPath);

            context.Clients.Remove(client);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Client");
        }
    }
}

