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
    public class SeatsController : Controller
    {
        private readonly DBCinemaContext _context;

        public SeatsController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Seats
        public async Task<IActionResult> Index(int? id, string? name)
        {
            ViewBag.CategoryId = id;
            ViewBag.CategoryName = name;
            var seatsByCategory = _context.Seats.Where(s => s.CategoryId == id).Include(s => s.Category).Include(c => c.CinemaHall);
            return View(await seatsByCategory.ToListAsync());
            //var dBCinemaContext = _context.Seats.Include(s => s.Category).Include(s => s.CinemaHall);
            //return View(await dBCinemaContext.ToListAsync());
        }
        
        public async Task<IActionResult> IndexAll()
        {
            var dBCinemaContext = _context.Seats.Include(s => s.Category).Include(s => s.CinemaHall);
            return View(await dBCinemaContext.ToListAsync());
        }
        public async Task<IActionResult> IndexCH(int? id, string? name)
        {
            ViewBag.CinemaHallId = id;
            ViewBag.CinemaHallName = name;
            var seatsByCinemaHall = _context.Seats.Where(s => s.CinemaHallId == id).Include(s => s.CinemaHall).Include(c=>c.Category);
            return View(await seatsByCinemaHall.ToListAsync());
        }

        // GET: Seats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats
                .Include(s => s.Category)
                .Include(s => s.CinemaHall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // GET: Seats/Create
        public IActionResult Create(int categoryId)
        {
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name");
            ViewBag.CategoryId = categoryId;
            ViewBag.CategoryName = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault().Name;
            return View();
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
           
            //return View();
        }
        public IActionResult CreateAll()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name");
            return View();
        }
        public IActionResult CreateCH(int cinemaHallId)
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.CinemaHallId = cinemaHallId;
            ViewBag.CinemaHallName = _context.CinemaHalls.Where(c => c.Id == cinemaHallId).FirstOrDefault().Name;
            return View();
        }

        // POST: Seats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int categoryId,[Bind("Id,RowNumber,SeatNumber,CinemaHallId,CategoryId")] Seat seat)
        {
            seat.CategoryId = categoryId;
            if (ModelState.IsValid)
            {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Seats", new { id = categoryId, name = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault().Name });
            }
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name", seat.CinemaHallId);
            return RedirectToAction("Index", "Seats", new { id = categoryId, name = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault().Name });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAll([Bind("Id,RowNumber,SeatNumber,CinemaHallId,CategoryId")] Seat seat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAll));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", seat.CategoryId);
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name", seat.CinemaHallId);
            return View(seat);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCH(int cinemaHallId, [Bind("Id,RowNumber,SeatNumber,CinemaHallId,CategoryId")] Seat seat)
        {
            seat.CinemaHallId = cinemaHallId;
            if (ModelState.IsValid)
            {
                _context.Add(seat);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexCH", "Seats", new { id = cinemaHallId, name = _context.CinemaHalls.Where(c => c.Id == cinemaHallId).FirstOrDefault().Name });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", seat.CategoryId);
            return RedirectToAction("IndexCH", "Seats", new { id = cinemaHallId, name = _context.CinemaHalls.Where(c => c.Id == cinemaHallId).FirstOrDefault().Name });
        }

        // GET: Seats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats.FindAsync(id);
            if (seat == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", seat.CategoryId);
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name", seat.CinemaHallId);
            return View(seat);
        }

        // POST: Seats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RowNumber,SeatNumber,CinemaHallId,CategoryId")] Seat seat)
        {
            if (id != seat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatExists(seat.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", seat.CategoryId);
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name", seat.CinemaHallId);
            return View(seat);
        }

        // GET: Seats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await _context.Seats
                .Include(s => s.Category)
                .Include(s => s.CinemaHall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // POST: Seats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeatExists(int id)
        {
            return _context.Seats.Any(e => e.Id == id);
        }
    }
}
