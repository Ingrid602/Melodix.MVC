using Melodix.APIConsumer;
using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace Melodix.MVC.Controllers
{
    public class CancionesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public CancionesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CancionesController
        public async Task <IActionResult> Index(string searchString, string genero, int? id)
        {
            var cancionesQuery = _context.Canciones
       .Include(c => c.Artista)
       .Include(c => c.Album)
       .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                cancionesQuery = cancionesQuery.Where(c =>
                    c.Nombre.Contains(searchString) ||
                    c.Artista.NombreArtistico.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(genero))
            {
                cancionesQuery = cancionesQuery.Where(c => c.Genero == genero);
            }

            var canciones = await cancionesQuery.ToListAsync();

            var cancionSeleccionada = id.HasValue
                ? canciones.FirstOrDefault(c => c.CancionId == id)
                : null;

            ViewBag.Cancion = cancionSeleccionada;
            ViewBag.CurrentFilter = searchString;

            var user = await _userManager.GetUserAsync(User);

            
            if (user != null)
            {
                if (user != null)
                {
                    //Playlists del usuario
                    var playlistsDelUsuario = Crud<Playlist>.GetAll()
                        .Where(p => p.UsuarioId == user.Id)
                        .ToList();

                    ViewBag.Playlists = playlistsDelUsuario;

                    // Obtener la playlist "Tus me gusta" con sus canciones
                    var meGusta = await _context.Playlists
                        .Include(p => p.PlaylistCanciones)
                        .FirstOrDefaultAsync(p => p.UsuarioId == user.Id && p.Nombre == "Tus me gusta");

                    var idsCancionesLikeadas = meGusta?.PlaylistCanciones?.Select(pc => pc.CancionId).ToList() ?? new List<int>();
                    ViewBag.CancionesMeGustan = idsCancionesLikeadas;
                }
                
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

            return View(canciones);
        }

        // GET: CancionesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CancionesController/Create
        [HttpGet("/Canciones/Create")]

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            Console.WriteLine($"Usuario actual: {(user != null ? user.UserName : "null")} - Rol: {(user != null ? user.Rol.ToString() : "Desconocido")}");

            if (user == null || user.Rol != RolUsuario.Musico)
                return Forbid();

            var artista = _context.Artistas.FirstOrDefault(a => a.UsuarioId == user.Id);
            if (artista == null)
            {
                Console.WriteLine("No existe el artista para este usuario.");
                return NotFound();
            }

            Console.WriteLine(artista != null
                ? $"[GET Create] Artista encontrado: {artista.NombreArtistico}"
                : "[GET Create] ❌ No se encontró un artista vinculado al usuario.");
            var canciones = _context.Canciones
                .Where(c => c.ArtistaId == artista.ArtistaId)
                .ToList();

            ViewBag.CancionesDelArtista = canciones;
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AlbumId", "Nombre");
            var albumes = _context.Albums
    .Where(a => a.ArtistaId == artista.ArtistaId)
    .ToList();

            ViewBag.Albumes = new SelectList(albumes, "AlbumId", "Nombre");

            return View();
        }

        // POST: CancionesController/Create
        [HttpPost]

        public async Task<IActionResult> Create(Cancion cancion, IFormFile archivo, IFormFile imagen)
        {
            
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                Console.WriteLine("Usuario no autenticado");
                return Forbid();
            }

            Console.WriteLine($"Usuario: {user.UserName} - Rol: {user.Rol}");

            if (user.Rol != RolUsuario.Musico)
            {
                Console.WriteLine("El usuario no tiene rol de músico");
                return Forbid();
            }

            var artista = _context.Artistas.FirstOrDefault(a => a.UsuarioId == user.Id);
            if (artista == null)
            {
                Console.WriteLine("No se encontró el artista vinculado al usuario");
                return NotFound();
            }

            var canciones = _context.Canciones
                .Where(c => c.ArtistaId == artista.ArtistaId)
                .ToList();

            ViewBag.CancionesDelArtista = canciones;

            Console.WriteLine($"Cantidad de canciones encontradas: {canciones.Count}");

            var albumes = _context.Albums
    .Where(a => a.ArtistaId == artista.ArtistaId)
    .ToList();

            ViewBag.Albumes = new SelectList(albumes, "AlbumId", "Nombre", cancion.AlbumId);

            if (archivo != null && archivo.Length > 0)
            {
                var nombreArchivo = Guid.NewGuid() + Path.GetExtension(archivo.FileName);
                var rutaArchivo = Path.Combine("wwwroot", "Canciones", nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                cancion.ArchivoURL = "/Canciones/" + nombreArchivo;
            }
            else
            {
                ModelState.AddModelError("", "Debes subir un archivo de audio.");
                return View();
            }

            // Si no se subió imagen personalizada, usar la del álbum
            if (imagen != null && imagen.Length > 0)
            {
                var nombreImagen = Guid.NewGuid() + Path.GetExtension(imagen.FileName);
                var rutaImagen = Path.Combine("wwwroot", "Portadas", nombreImagen);

                using (var stream = new FileStream(rutaImagen, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                cancion.Urlimagen = "/Portadas/" + nombreImagen;
            }
            else
            {
                // Usar imagen del álbum si existe
                var album = _context.Albums.FirstOrDefault(a => a.AlbumId == cancion.AlbumId);
                cancion.Urlimagen = album?.urlImagen;
            }

            cancion.ArtistaId = artista.ArtistaId;
            

            _context.Canciones.Add(cancion);
            await _context.SaveChangesAsync();

            TempData["Exito"] = "Canción subida correctamente.";
            return RedirectToAction("MisCanciones");


            
        }



        // GET: CancionesController/Edit/5

        public async Task<IActionResult> Edit(int id)
        {
            var cancion = _context.Canciones.FirstOrDefault(c => c.CancionId == id);
            if (cancion == null) return NotFound();

            var artista = _context.Artistas.FirstOrDefault(a => a.ArtistaId == cancion.ArtistaId);
            var albumes = _context.Albums
                .Where(a => a.ArtistaId == artista.ArtistaId)
                .ToList();

            ViewBag.Albumes = new SelectList(albumes, "AlbumId", "Nombre", cancion.AlbumId);
            return View(cancion);
        }


        // POST: CancionesController/Edit/5

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Cancion cancion, IFormFile archivo, IFormFile imagen)
        {
            var cancionDb = _context.Canciones.FirstOrDefault(c => c.CancionId == id);
            if (cancionDb == null) return NotFound();

            cancionDb.Nombre = cancion.Nombre;
            cancionDb.Genero = cancion.Genero;
            cancionDb.Duracion = cancion.Duracion;
            cancionDb.AlbumId = cancion.AlbumId;

            if (archivo != null && archivo.Length > 0)
            {
                var nombreArchivo = Guid.NewGuid() + Path.GetExtension(archivo.FileName);
                var rutaArchivo = Path.Combine("wwwroot", "Canciones", nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                cancionDb.ArchivoURL = "/Canciones/" + nombreArchivo;
            }

            if (imagen != null && imagen.Length > 0)
            {
                var nombreImagen = Guid.NewGuid() + Path.GetExtension(imagen.FileName);
                var rutaImagen = Path.Combine("wwwroot", "Portadas", nombreImagen);

                using (var stream = new FileStream(rutaImagen, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                cancionDb.Urlimagen = "/Portadas/" + nombreImagen;
            }
            else
            {
                var album = _context.Albums.FirstOrDefault(a => a.AlbumId == cancion.AlbumId);
                cancionDb.Urlimagen = album?.urlImagen;
            }

            await _context.SaveChangesAsync();
            TempData["Exito"] = "Canción actualizada correctamente.";
            return RedirectToAction("Create");
        }





        // GET: CancionesController/Delete/5
        public ActionResult Delete(int id)
        {
            var cancion = _context.Canciones.FirstOrDefault(c => c.CancionId == id);
            if (cancion == null) return NotFound();

            return View(cancion);
        }

        // POST: CancionesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var cancion = _context.Canciones.FirstOrDefault(c => c.CancionId == id);
            if (cancion == null) return NotFound();

            _context.Canciones.Remove(cancion);
            await _context.SaveChangesAsync();

            TempData["Exito"] = "Canción eliminada correctamente.";
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> MisCanciones()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Rol != RolUsuario.Musico)
                return Forbid();

            var artista = _context.Artistas.FirstOrDefault(a => a.UsuarioId == user.Id);
            if (artista == null) return NotFound();

            var canciones = _context.Canciones
                .Include(c => c.Album)
                .Where(c => c.ArtistaId == artista.ArtistaId)
                .ToList();

            return View(canciones);
        }

    }
}
