using GymManagement.Areas.Identity.Data;
using GymManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GymManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var trainers = _context.Trainers.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                trainers = trainers.Where(t =>
                    t.FullName.Contains(searchString) ||
                    t.ContactNo.Contains(searchString)); // Filter by name or contact
            }

            // Return the filtered list to the view
            return View(await trainers.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.TrainerTypes = new[] { "Personal Trainer", "Gym Trainer" };
            ViewBag.StatusOptions = new[] { "Active", "Inactive" };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Trainer created successfully!";

                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerTypes = new[] { "Personal Trainer", "Gym Trainer" };
            ViewBag.StatusOptions = new[] { "Active", "Inactive" };
            return View(trainer);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            ViewBag.TrainerTypes = new[] { "Personal Trainer", "Gym Trainer" };
            ViewBag.StatusOptions = new[] { "Active", "Inactive" };
            return View(trainer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Trainers.Update(trainer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Trainer updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerTypes = new[] { "Personal Trainer", "Gym Trainer" };
            ViewBag.StatusOptions = new[] { "Active", "Inactive" };
            return View(trainer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
                return NotFound();

            return View(trainer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                TempData["SuccessMessage"] = "Trainer deleted successfully!";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
