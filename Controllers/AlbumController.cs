using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MusicBlog.Models;

namespace MusicBlog.controllers
{
    public class AlbumController : Controller
    {
        private readonly AppDbContext _context;

        public AlbumController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Album
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Albums.Include(a => a.Singer);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Album/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Singer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Album/Create
        public IActionResult Create()
        {
            ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name");

            return View();
        }

        // POST: Album/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Released,Poster,SingerId")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name", album.SingerId);


            return View(album);
        }

        // GET: Album/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            // var album = await _context.Albums
            //     .Include(a => a.Singer)
            //     .FirstOrDefaultAsync(m => m.Id == id);

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name", album.SingerId);

            return View(album);
        }

        // POST: Album/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Released,Poster,SingerId")] Album album)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.Id))
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
            ViewData["SingerId"] = new SelectList(_context.Singers, "Id", "Name", album.SingerId);

            // ViewData["SingerName"] = new SelectList(_context.Singers, "Name", "Name", album.Singer.Name);

            return View(album);
        }

        // GET: Album/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Singer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Albums == null)
            {
                return Problem("Entity set 'AppDbContext.Albums'  is null.");
            }
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return (_context.Albums?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Save(Album model)
        {
            if (ModelState.IsValid)
            {
                _context.Albums.Add(model);
                await _context.SaveChangesAsync();
                return Redirect("/Singer/Details/" + model.SingerId);
            }
            return Redirect("/Singer/Index");
        }

        public async Task<IActionResult> Search(string search)
        {
            IQueryable<Album> songQuery = _context.Albums;

            if (!string.IsNullOrEmpty(search))
            {
                songQuery = songQuery.Where(a => a.Name != null && a.Name.Contains(search));
            }

            var albums = await songQuery.ToListAsync();

            return View("Index", albums); // Redirige a la vista "Index" con los resultados de la búsqueda
        }

        public async Task<IActionResult> getSingers(string search)
        {
            IQueryable<Singer> singerQuery = _context.Singers;

            if (!string.IsNullOrEmpty(search))
            {
                singerQuery = singerQuery.Where(s => s.Name != null);
            }

            var singers = await singerQuery.ToListAsync();

            return View("Edit", singers); // Redirige a la vista "Index" con los resultados de la búsqueda
        }

        public async Task<IActionResult> SortByCreation()
        {

            IQueryable<Album> AlbumsQuery = _context.Albums;
            var sortedAlbums = AlbumsQuery.OrderBy(a => a.Id);

            return View("Index", sortedAlbums);
        }

        public async Task<IActionResult> SortBySinger()
        {

            IQueryable<Album> albumsQuery = _context.Albums;
            var sortedAlbums = albumsQuery.OrderBy(a => a.Singer.Name);

            return View("Index", sortedAlbums);
        }

    }
}
