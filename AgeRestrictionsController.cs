using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaWebApplication;

namespace CinemaWebApplication.Controllers
{
    public class AgeRestrictionsController : Controller
    {
        private readonly DBCinemaContext _context;

        public AgeRestrictionsController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: AgeRestrictions
        public async Task<IActionResult> Index()
        {
            return View(await _context.AgeRestrictions.ToListAsync());
        }

        // GET: AgeRestrictions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ageRestriction = await _context.AgeRestrictions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ageRestriction == null)
            {
                return NotFound();
            }

            //return View(ageRestriction);
            return RedirectToAction("IndexAR", "Movies", new { id = ageRestriction.Id, number = ageRestriction.Number });
        }

        // GET: AgeRestrictions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AgeRestrictions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number")] AgeRestriction ageRestriction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ageRestriction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ageRestriction);
        }

        // GET: AgeRestrictions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ageRestriction = await _context.AgeRestrictions.FindAsync(id);
            if (ageRestriction == null)
            {
                return NotFound();
            }
            return View(ageRestriction);
        }

        // POST: AgeRestrictions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number")] AgeRestriction ageRestriction)
        {
            if (id != ageRestriction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ageRestriction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgeRestrictionExists(ageRestriction.Id))
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
            return View(ageRestriction);
        }

        // GET: AgeRestrictions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ageRestriction = await _context.AgeRestrictions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ageRestriction == null)
            {
                return NotFound();
            }

            return View(ageRestriction);
        }

        // POST: AgeRestrictions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ageRestriction = await _context.AgeRestrictions.FindAsync(id);
            _context.AgeRestrictions.Remove(ageRestriction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgeRestrictionExists(int id)
        {
            return _context.AgeRestrictions.Any(e => e.Id == id);
        }
    }
}
