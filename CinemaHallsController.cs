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
    public class CinemaHallsController : Controller
    {
        private readonly DBCinemaContext _context;

        public CinemaHallsController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: CinemaHalls
        public async Task<IActionResult> Index()
        {
            return View(await _context.CinemaHalls.ToListAsync());
        }

        // GET: CinemaHalls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaHall = await _context.CinemaHalls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinemaHall == null)
            {
                return NotFound();
            }

            //return View(cinemaHall);
            return RedirectToAction("IndexCH", "Seats", new { id = cinemaHall.Id, name = cinemaHall.Name });
        }

        // GET: CinemaHalls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CinemaHalls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CinemaHall cinemaHall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cinemaHall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinemaHall);
        }

        // GET: CinemaHalls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaHall = await _context.CinemaHalls.FindAsync(id);
            if (cinemaHall == null)
            {
                return NotFound();
            }
            return View(cinemaHall);
        }

        // POST: CinemaHalls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CinemaHall cinemaHall)
        {
            if (id != cinemaHall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cinemaHall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CinemaHallExists(cinemaHall.Id))
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
            return View(cinemaHall);
        }

        // GET: CinemaHalls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaHall = await _context.CinemaHalls
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinemaHall == null)
            {
                return NotFound();
            }

            return View(cinemaHall);
        }

        // POST: CinemaHalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinemaHall = await _context.CinemaHalls.FindAsync(id);
            _context.CinemaHalls.Remove(cinemaHall);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CinemaHallExists(int id)
        {
            return _context.CinemaHalls.Any(e => e.Id == id);
        }
    }
}
