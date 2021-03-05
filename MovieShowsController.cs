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
    public class MovieShowsController : Controller
    {
        private readonly DBCinemaContext _context;

        public MovieShowsController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: MovieShows
        public async Task<IActionResult> Index(int? id, string? title)
        {
            //var dBCinemaContext = _context.MovieShows.Include(m => m.CinemaHall).Include(m => m.Movie);
            //return View(await dBCinemaContext.ToListAsync());
            ViewBag.MovieId = id;
            ViewBag.MovieTitle = title;
            var movieShowsByMovie = _context.MovieShows.Where(m => m.MovieId == id).Include(m => m.Movie).Include(c=>c.CinemaHall);
            return View(await movieShowsByMovie.ToListAsync());
        }
        public async Task<IActionResult> IndexAll()
        {
            var dBCinemaContext = _context.MovieShows.Include(m => m.CinemaHall).Include(m => m.Movie);
            return View(await dBCinemaContext.ToListAsync());
        }

        // GET: MovieShows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieShow = await _context.MovieShows
                .Include(m => m.CinemaHall)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieShow == null)
            {
                return NotFound();
            }

            return View(movieShow);
        }

        // GET: MovieShows/Create
        public IActionResult Create(int movieId)
        {
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name");
            ViewBag.MovieId = movieId;
            ViewBag.GenreName = _context.Movies.Where(m => m.Id == movieId).FirstOrDefault().Title;
            return View();
        }
        public IActionResult CreateAll()
        {
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name");
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");
            return View();
        }

        // POST: MovieShows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int movieId,[Bind("Id,MovieId,CinemaHallId,DateAndTime")] MovieShow movieShow)
        {
            movieShow.MovieId = movieId;
            if (ModelState.IsValid)
            {
                _context.Add(movieShow);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "MovieShows", new { id = movieId, name = _context.Movies.Where(m => m.Id == movieId).FirstOrDefault().Title });
            }
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name", movieShow.CinemaHallId);
            return RedirectToAction("Index", "MovieShows", new { id = movieId, name = _context.Movies.Where(m => m.Id == movieId).FirstOrDefault().Title });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAll([Bind("Id,MovieId,CinemaHallId,DateAndTime")] MovieShow movieShow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieShow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAll));
            }
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name", movieShow.CinemaHallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieShow.MovieId);
            return View(movieShow);
        }

        // GET: MovieShows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieShow = await _context.MovieShows.FindAsync(id);
            if (movieShow == null)
            {
                return NotFound();
            }
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name", movieShow.CinemaHallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieShow.MovieId);
            return View(movieShow);
        }

        // POST: MovieShows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,CinemaHallId,DateAndTime")] MovieShow movieShow)
        {
            if (id != movieShow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieShow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieShowExists(movieShow.Id))
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
            ViewData["CinemaHallId"] = new SelectList(_context.CinemaHalls, "Id", "Name", movieShow.CinemaHallId);
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieShow.MovieId);
            return View(movieShow);
        }

        // GET: MovieShows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieShow = await _context.MovieShows
                .Include(m => m.CinemaHall)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieShow == null)
            {
                return NotFound();
            }

            return View(movieShow);
        }

        // POST: MovieShows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieShow = await _context.MovieShows.FindAsync(id);
            _context.MovieShows.Remove(movieShow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieShowExists(int id)
        {
            return _context.MovieShows.Any(e => e.Id == id);
        }
    }
}
