using Melodix.APIConsumer;
using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Melodix.MVC.Controllers
{
    public class AlbumsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AlbumsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var playlistsDelUsuario = Crud<Playlist>.GetAll()
                    .Where(p => p.UsuarioId == user.Id)
                .ToList();

                ViewBag.Playlists = playlistsDelUsuario;
            }

            return View(albums);
        }


        // GET: AlbumsController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var album = await _context.Albums
                .Include(a => a.Artista)
                .Include(a => a.Canciones)
                .ThenInclude(c => c.Artista) // Para que funcione c.Artista en el reproductor
                .Include(a => a.UsuariosQueLoGuardaron)
                .FirstOrDefaultAsync(a => a.AlbumId == id);

            if (album == null)
            {
                return NotFound();
            }

            //Para obtener todas las playlist del usuario logueado
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var playlistsDelUsuario = Crud<Playlist>.GetAll()
                    .Where(p => p.UsuarioId == user.Id)
                .ToList();

                ViewBag.Playlists = playlistsDelUsuario;
            }

            return View(album); //   se envía el modelo a la vista
        }

        //Metodo para guardar o borral un album de la biblioteca

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleGuardarAlbum(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var album = await _context.Albums
                .Include(a => a.UsuariosQueLoGuardaron)
                .FirstOrDefaultAsync(a => a.AlbumId == id);

            if (album == null)
                return NotFound();

            if (album.UsuariosQueLoGuardaron == null)
                album.UsuariosQueLoGuardaron = new List<ApplicationUser>();

            if (album.UsuariosQueLoGuardaron.Any(u => u.Id == user.Id))
            {
                // Ya está guardado, quitarlo
                album.UsuariosQueLoGuardaron.Remove(user);
            }
            else
            {
                // No está guardado, agregarlo
                album.UsuariosQueLoGuardaron.Add(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = album.AlbumId });
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
