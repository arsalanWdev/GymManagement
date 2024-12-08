using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymManagement.Areas.Identity.Data;
using GymManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace GymManagement.Controllers
{
    [Authorize(Roles ="Receptionist")]
    public class WalkInController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WalkInController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WalkIn
        public async Task<IActionResult> Index()
        {
            return View(await _context.WalkIns.ToListAsync());
        }

        // GET: WalkIn/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walkIn = await _context.WalkIns
                .FirstOrDefaultAsync(m => m.Id == id);
            if (walkIn == null)
            {
                return NotFound();
            }

            return View(walkIn);
        }

        // GET: WalkIn/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WalkIn/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Contact,VisitDate,Purpose")] WalkIn walkIn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(walkIn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(walkIn);
        }

        // GET: WalkIn/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walkIn = await _context.WalkIns.FindAsync(id);
            if (walkIn == null)
            {
                return NotFound();
            }
            return View(walkIn);
        }

        // POST: WalkIn/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Contact,VisitDate,Purpose")] WalkIn walkIn)
        {
            if (id != walkIn.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(walkIn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalkInExists(walkIn.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(walkIn);
        }

        // GET: WalkIn/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var walkIn = await _context.WalkIns
                .FirstOrDefaultAsync(m => m.Id == id);
            if (walkIn == null)
            {
                return NotFound();
            }

            return View(walkIn);
        }

        // POST: WalkIn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var walkIn = await _context.WalkIns.FindAsync(id);
            if (walkIn != null)
            {
                _context.WalkIns.Remove(walkIn);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WalkInExists(int id)
        {
            return _context.WalkIns.Any(e => e.Id == id);
        }
    }
}
