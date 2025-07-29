using Melodix.APIConsumer;
using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melodix.MVC.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlaylistsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: PlaylistsController
        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Obtener todas las playlists del usuario autenticado
            var playlistsDelUsuario = Crud<Playlist>
                .GetAll()
                .Where(p => p.UsuarioId == user.Id)
                .ToList();

            return View(playlistsDelUsuario);
        }

        // GET: PlaylistsController/Details/5

        public async Task <IActionResult> Details(int id)
        {
            var playlist = _context.Playlists
                .Include(p => p.Usuario)
                .Include(p => p.PlaylistCanciones)
                    .ThenInclude(pc => pc.Cancion)
                        .ThenInclude(c => c.Artista)
                .FirstOrDefault(p => p.PlaylistId == id);

            if (playlist == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            bool esPremium = false;

            if (user != null)
            {
                var usuarioDb = _context.Users
                    .Include(u => u.Suscripciones)
                    .FirstOrDefault(u => u.Id == user.Id);

                esPremium = usuarioDb?.Suscripciones?.Any(s => s.Activo && s.FechaFin > DateTime.Now) == true;
            }

            ViewBag.EsPremium = esPremium;

            return View(playlist);
        }



        // GET: PlaylistsController/Create
        public ActionResult Create()
        {

            return View();
        }
       
        // POST: PlaylistsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Playlist data, IFormFile Imagen)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User); // Esto te da el ApplicationUser completo
                if (user == null)
                {
                    return Unauthorized();
                }

                data.UsuarioId = user.Id;
                data.FechaCreacion = DateTime.Now;

                //  Procesar imagen
                if (Imagen != null && Imagen.Length > 0)
                {
                    // Crear carpeta si no existe
                    var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "playlists");
                    if (!Directory.Exists(carpeta))
                    {
                        Directory.CreateDirectory(carpeta);
                    }

                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(Imagen.FileName);
                    var rutaArchivo = Path.Combine(carpeta, nombreArchivo);

                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await Imagen.CopyToAsync(stream);
                    }

                    // Guardar ruta relativa para usarla en la vista
                    data.ImagenUrl = "/img/playlists/" + nombreArchivo;
                }
                else
                {
                    // Si no se sube imagen, se usa una por defecto
                    data.ImagenUrl = "/img/playlists/default-playlist.png";
                }

                // Este objeto sí tendrá el PlaylistId asignado por la API
                var nuevaPlaylist = Crud<Playlist>.Create(data);
               
                // redirigir correctamente a Details
                return RedirectToAction("Details", new { id = nuevaPlaylist.PlaylistId });

                
            }
            catch(Exception ex) 
            {
                ModelState.AddModelError("",ex.Message);
                return View(data);
            }
        }

        // GET: PlaylistsController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = Crud<Playlist>.GetById(id);
            return View(data);
        }

        // POST: PlaylistsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(int id, Playlist data, IFormFile? imagenArchivo, bool EliminarImagen)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                data.PlaylistId = id;
                data.UsuarioId = user.Id;

                // Si el usuario marcó eliminar imagen
                if (EliminarImagen && !string.IsNullOrEmpty(data.ImagenUrl))
                {
                    // Eliminar el archivo físico (opcional)
                    var rutaFisica = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", data.ImagenUrl.TrimStart('/'));
                    if (System.IO.File.Exists(rutaFisica))
                    {
                        System.IO.File.Delete(rutaFisica);
                    }

                    // Reemplazar por imagen por defecto
                    data.ImagenUrl = "/img/playlists/default.png";
                }
                // Recuperar la playlist actual para obtener la imagen existente
                var playlistActual = Crud<Playlist>.GetById(id);

                if (imagenArchivo != null && imagenArchivo.Length > 0)
                {
                    var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(imagenArchivo.FileName)}";
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/playlists", nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await imagenArchivo.CopyToAsync(stream);
                    }

                    data.ImagenUrl = $"/img/playlists/{nombreArchivo}";
                }
                else if(!EliminarImagen)
                {
                    
                    data.ImagenUrl = playlistActual.ImagenUrl;
                }

                


                var success = Crud<Playlist>.Update(id, data);

                if (!success)
                {
                    throw new Exception("No se pudo actualizar la playlist.");
                }

                var updatedData = Crud<Playlist>.GetById(id);
                return View("Details", updatedData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔴 [ERROR] {ex.Message}");
                ModelState.AddModelError("", ex.Message);
                return View("Details", data);
            }
        }

        // GET: PlaylistsController/Delete/5
        public ActionResult Delete(int id)
        {
            var data = Crud<Playlist>.GetById(id);
            return View(data);
        }

        // POST: PlaylistsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Playlist data)
        {
            try
            {
                Crud<Playlist>.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(data);
            }
        }
    }
}
