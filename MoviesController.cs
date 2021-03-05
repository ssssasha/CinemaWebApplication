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
    public class MoviesController : Controller
    {
        private readonly DBCinemaContext _context;

        public MoviesController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(int? id, string? name)
        {
            //if (id == null) return RedirectToAction("Genres", "IndexAll");
            ViewBag.GenreId = id;
            ViewBag.GenreName = name;
            var moviesByGenre = _context.Movies.Where(m => m.GenreId == id).Include(m => m.Genre).Include(n=>n.AgeRestriction);
            return View(await moviesByGenre.ToListAsync());
            //var dBCinemaContext = _context.Movies.Include(m => m.AgeRestriction).Include(m => m.Genre);
            //return View(await dBCinemaContext.ToListAsync());
        }
        public async Task<IActionResult> IndexAR(int? id, string? number)
        {
           // if (id == null) return RedirectToAction("AgeRestrictions", "IndexAll");
            ViewBag.AgeRestrictionId = id;
            ViewBag.AgeRestrictionNumber = number;
            var moviesByAgeRestriction = _context.Movies.Where(m => m.AgeRestrictionId == id).Include(m=>m.AgeRestriction).Include(n=>n.Genre);
            return View(await moviesByAgeRestriction.ToListAsync());
        }
        public async Task<IActionResult> IndexAll()
        {
            var dBCinemaContext = _context.Movies.Include(m => m.AgeRestriction).Include(m => m.Genre);
            return View(await dBCinemaContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.AgeRestriction)
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            //return View(movie);
            return RedirectToAction("Index", "MovieShows", new { id = movie.Id, title = movie.Title });
        }

        // GET: Movies/Create
        public IActionResult Create(int genreId)
        {
            ViewData["AgeRestrictionId"] = new SelectList(_context.AgeRestrictions, "Id", "Number");
            // ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.GenreId = genreId;
            ViewBag.GenreName = _context.Genres.Where(g => g.Id == genreId).FirstOrDefault().Name;
            return View();
        }
        public IActionResult CreateAll()
        {
            ViewData["AgeRestrictionId"] = new SelectList(_context.AgeRestrictions, "Id", "Number");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }
        public IActionResult CreateAR(int ageRestrictionId)
        {
            //ViewData["AgeRestrictionId"] = new SelectList(_context.AgeRestrictions, "Id", "Number");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.AgeRestrictionId = ageRestrictionId;
            ViewBag.AgeRestrictionNumber = _context.AgeRestrictions.Where(a => a.Id == ageRestrictionId).FirstOrDefault().Number;
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int genreId, [Bind("Id,Title,Runtime,ReleaseDate,Directors,Cast,Synopsis,GenreId,AgeRestrictionId")] Movie movie)
        {
            movie.GenreId = genreId;
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Movies", new { id = genreId, name = _context.Genres.Where(g => g.Id == genreId).FirstOrDefault().Name });
            }
            ViewData["AgeRestrictionId"] = new SelectList(_context.AgeRestrictions, "Id", "Number", movie.AgeRestrictionId);
            // ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            // return View(movie);
            return RedirectToAction("Index", "Movies", new { id = genreId, name = _context.Genres.Where(g => g.Id == genreId).FirstOrDefault().Name });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAll( [Bind("Id,Title,Runtime,ReleaseDate,Directors,Cast,Synopsis,GenreId,AgeRestrictionId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAll));
                
            }
            ViewData["AgeRestrictionId"] = new SelectList(_context.AgeRestrictions, "Id", "Number", movie.AgeRestrictionId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAR(int ageRestrictionId, [Bind("Id,Title,Runtime,ReleaseDate,Directors,Cast,Synopsis,GenreId,AgeRestrictionId")] Movie movie)
        {
            movie.AgeRestrictionId = ageRestrictionId;
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("IndexAR", "Movies", new { id = ageRestrictionId, number = _context.AgeRestrictions.Where(a => a.Id == ageRestrictionId).FirstOrDefault().Number });
            }
            //ViewData["AgeRestrictionId"] = new SelectList(_context.AgeRestrictions, "Id", "Number", movie.AgeRestrictionId);
             ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            // return View(movie);
            return RedirectToAction("IndexAR", "Movies", new { id = ageRestrictionId, number = _context.AgeRestrictions.Where(a => a.Id == ageRestrictionId).FirstOrDefault().Number });
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["AgeRestrictionId"] = new SelectList(_context.AgeRestrictions, "Id", "Number", movie.AgeRestrictionId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Runtime,ReleaseDate,Directors,Cast,Synopsis,GenreId,AgeRestrictionId")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            ViewData["AgeRestrictionId"] = new SelectList(_context.AgeRestrictions, "Id", "Number", movie.AgeRestrictionId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.AgeRestriction)
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
            
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
