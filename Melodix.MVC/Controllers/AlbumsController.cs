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

            bool esPremium = false;

            if (user != null)
            {
                var usuarioDb = _context.Users
                    .Include(u => u.Suscripciones)
                    .FirstOrDefault(u => u.Id == user.Id);

                esPremium = usuarioDb?.Suscripciones?.Any(s => s.Activo && s.FechaFin > DateTime.Now) == true;
            }

            ViewBag.EsPremium = esPremium;

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
        [HttpGet]
        public async Task <ActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Rol != RolUsuario.Musico)
                return Forbid();

            var artista = _context.Artistas.FirstOrDefault(a => a.UsuarioId == user.Id);
            if (artista == null) return NotFound();
            return View();
        }

        // POST: AlbumsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Album album, IFormFile imagen)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Rol != RolUsuario.Musico)
                return Forbid();

            var artista = _context.Artistas.FirstOrDefault(a => a.UsuarioId == user.Id);
            if (artista == null) return NotFound();

            album.ArtistaId = artista.ArtistaId;
            album.FechaLanzamiento = DateTime.Now;

            if (imagen != null && imagen.Length > 0)
            {
                var nombreImagen = Guid.NewGuid() + Path.GetExtension(imagen.FileName);
                var ruta = Path.Combine("wwwroot", "Portadas", nombreImagen);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                album.urlImagen = "/Portadas/" + nombreImagen;
            }

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            TempData["Exito"] = "Álbum creado exitosamente.";
            return RedirectToAction("MisAlbumes");

        }

        // GET: AlbumsController/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            var artista = _context.Artistas.FirstOrDefault(a => a.UsuarioId == user.Id);

            if (artista == null || album.ArtistaId != artista.ArtistaId) return Forbid();

            return View(album);
        }

        // POST: AlbumsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Album album, IFormFile nuevaImagen)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Rol != RolUsuario.Musico)
                return Forbid();

            var artista = _context.Artistas.FirstOrDefault(a => a.UsuarioId == user.Id);
            if (artista == null) return NotFound();

            var albumDb = _context.Albums.FirstOrDefault(a => a.AlbumId == id && a.ArtistaId == artista.ArtistaId);
            if (albumDb == null) return Forbid(); // <- Aquí prevenimos editar un álbum que no le pertenece

            albumDb.Nombre = album.Nombre;

            if (nuevaImagen != null && nuevaImagen.Length > 0)
            {
                var nombreImagen = Guid.NewGuid() + Path.GetExtension(nuevaImagen.FileName);
                var ruta = Path.Combine("wwwroot", "Portadas", nombreImagen);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await nuevaImagen.CopyToAsync(stream);
                }

                albumDb.urlImagen = "/Portadas/" + nombreImagen;
            }

            await _context.SaveChangesAsync();

            TempData["Exito"] = "Álbum actualizado correctamente.";
            return RedirectToAction("MisAlbumes");
        }


        // GET: AlbumsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Rol != RolUsuario.Musico) return Forbid();

            var artista = _context.Artistas.FirstOrDefault(a => a.UsuarioId == user.Id);
            if (artista == null) return NotFound();

            var album = _context.Albums
                .FirstOrDefault(a => a.AlbumId == id && a.ArtistaId == artista.ArtistaId);

            if (album == null) return NotFound();

            return View(album);
        }

        // POST: AlbumsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.Albums
                .Include(a => a.Canciones) // Traer canciones relacionadas
                .FirstOrDefaultAsync(a => a.AlbumId == id);

            if (album == null) return NotFound();

            // Eliminar las canciones relacionadas primero
            if (album.Canciones != null && album.Canciones.Any())
            {
                _context.Canciones.RemoveRange(album.Canciones);
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            TempData["Exito"] = "Álbum y canciones eliminados correctamente.";
            return RedirectToAction("MisAlbumes");
        }



        [HttpGet]
        public async Task<IActionResult> MisAlbumes()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Index", "Home");

            var artista = await _context.Artistas.FirstOrDefaultAsync(a => a.UsuarioId == user.Id);
            if (artista == null) return RedirectToAction("Index", "Home");

            var misAlbumes = await _context.Albums
                .Where(a => a.ArtistaId == artista.ArtistaId)
                .Include(a => a.Canciones)
                .ToListAsync();

            return View(misAlbumes);
        }

    }
}
