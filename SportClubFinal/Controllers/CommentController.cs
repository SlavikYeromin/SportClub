using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportClubFinal.Data;
using SportClubFinal.Models;
using System.Data;

namespace SportClubFinal.Controllers
{
    [Authorize(Roles = "admin,client")]
    public class CommentController : Controller
    {
        private readonly AppDbContext context;

        public CommentController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(string searchString)
        {
            var comments = context.Comments.Include("Coaches").OrderByDescending(p => p.Id).ToList();

            if(!String.IsNullOrEmpty(searchString))
            {
                comments = comments.Where(n => n.Coaches.LastName.Contains(searchString) || n.Coaches.FirstName.Contains(searchString) || n.Coaches.MiddleName.Contains(searchString)).ToList();
            }

            return View(comments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LoadCoaches();
            return View();
        }

        [NonAction]
        private void LoadCoaches()
        {
            var coaches = context.Coaches.ToList();
            ViewBag.Coaches = new SelectList(coaches, "Id", "FirstName", "FirstName", "LastName");
        }

        [HttpPost]
        public IActionResult Create(Comment model)
        {
                context.Comments.Add(model);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id != null)
            {
                NotFound();
            }
            LoadCoaches();
            var comment = context.Comments.Find(id);
            return View(comment);
        }

        [HttpPost]
        public IActionResult Edit(Comment model)
        {
            ModelState.Remove("Coaches");
            if(ModelState.IsValid)
            {
                context.Comments.Update(model);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int? id) 
        { 
            LoadCoaches();
            var comment = context.Comments.Find(id);

            if (comment == null)
            {
                 return RedirectToAction("Index", "Comment");
            }
            context.Comments.Remove(comment);
               context.SaveChanges(true);

                return RedirectToAction("Index", "Comment");
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Comment model)
        {
            context.Comments.Remove(model);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
