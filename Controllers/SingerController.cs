using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicBlog.Models;

namespace MusicBlog.controllers
{
    public class SingerController : Controller
    {
        private readonly AppDbContext _context;

        public SingerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Singer
        public async Task<IActionResult> Index()
        {
            return _context.Singers != null ?
                        View(await _context.Singers.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.Singers'  is null.");
        }

        // GET: Singer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Singers == null)
            {
                return NotFound();
            }

            var singer = await _context.Singers.Where(m => m.Id == id).Include(m => m.Albums).FirstAsync();
            if (singer == null)
            {
                return NotFound();
            }

            return View(singer);
        }

        // GET: Singer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Singer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Gender,Poster")] Singer singer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(singer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(singer);
        }

        // GET: Singer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Singers == null)
            {
                return NotFound();
            }

            var singer = await _context.Singers.FindAsync(id);
            if (singer == null)
            {
                return NotFound();
            }
            return View(singer);
        }

        // POST: Singer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Gender,Poster")] Singer singer)
        {
            if (id != singer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(singer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SingerExists(singer.Id))
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
            return View(singer);
        }

        // GET: Singer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Singers == null)
            {
                return NotFound();
            }

            var singer = await _context.Singers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (singer == null)
            {
                return NotFound();
            }

            return View(singer);
        }

        // POST: Singer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Singers == null)
            {
                return Problem("Entity set 'AppDbContext.Singers'  is null.");
            }
            var singer = await _context.Singers.FindAsync(id);
            if (singer != null)
            {
                _context.Singers.Remove(singer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SingerExists(int id)
        {
            return (_context.Singers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Search(string search)
        {
            IQueryable<Singer> singerQuery = _context.Singers;

            if (!string.IsNullOrEmpty(search))
            {
                singerQuery = singerQuery.Where(s => s.Name != null && s.Name.Contains(search));
            }

            var singers = await singerQuery.ToListAsync();

            return View("Index", singers); // Redirige a la vista "Index" con los resultados de la búsqueda
        }

        public async Task<IActionResult> OrderByAlbumsCount()
        {

            IQueryable<Singer> singerQuery = _context.Singers;
            var sortedSingers = singerQuery.OrderBy(a => a.Albums.Count);


            return View("Index", sortedSingers);
        }

        public async Task<IActionResult> SortByAge()
        {

            IQueryable<Singer> singerQuery = _context.Singers;
            var sortedSingers = singerQuery.OrderBy(s => s.Age);

            return View("Index", sortedSingers);
        }

        public async Task<IActionResult> SortByCreation()
        {

            IQueryable<Singer> singerQuery = _context.Singers;
            var sortedSingers = singerQuery.OrderBy(s => s.Id);

            return View("Index", sortedSingers);
        }
    }
}
