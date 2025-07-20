using Melodix.Data.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Melodix.MVC.Controllers
{
    public class AlbumsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AlbumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AlbumsController
        public async Task<IActionResult> Index(string searchString, int? id)
        {
            var albumsQuery = _context.Albums
                .Include(a => a.Artista)
                .Include(a => a.Canciones)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                albumsQuery = albumsQuery.Where(a =>
                    a.Nombre.Contains(searchString) ||
                    a.Artista.NombreArtistico.Contains(searchString));
            }

            var albums = await albumsQuery.ToListAsync();

            var albumSeleccionado = id.HasValue
                ? albums.FirstOrDefault(a => a.AlbumId == id)
                : null;

            ViewBag.Album = albumSeleccionado;
            ViewBag.CurrentFilter = searchString;

            return View(albums);
        }


        // GET: AlbumsController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var album = await _context.Albums
                .Include(a => a.Artista)
                .Include(a => a.Canciones)
                .ThenInclude(c => c.Artista) // Para que funcione c.Artista en el reproductor
                .FirstOrDefaultAsync(a => a.AlbumId == id);

            if (album == null)
            {
                return NotFound();
            }

            return View(album); // 👈 Importante: se envía el modelo a la vista
        }


        // GET: AlbumsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlbumsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AlbumsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AlbumsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AlbumsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AlbumsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
