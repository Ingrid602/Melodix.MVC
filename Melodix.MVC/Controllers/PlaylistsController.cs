using Melodix.APIConsumer;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Melodix.MVC.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PlaylistsController(UserManager<ApplicationUser> userManager)
        {
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
        public ActionResult Details(int id)
        {
            var data = Crud <Playlist>.GetById(id);
            return View(data);
        }

        // GET: PlaylistsController/Create
        public ActionResult Create()
        {

            return View();
        }
       
        // POST: PlaylistsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Playlist data)
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

                
                // Este objeto sí tendrá el PlaylistId asignado por la API
                var nuevaPlaylist = Crud<Playlist>.Create(data);

                // redirigir correctamente a Details
                return RedirectToAction("Details", new { id = nuevaPlaylist.PlaylistId });

                return RedirectToAction(nameof(Index));
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
        public async Task <IActionResult> Edit(int id, Playlist data)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return Unauthorized();

                data.PlaylistId = id;
                data.UsuarioId = user.Id; 

                //  Agrega este log para depuración para ver el error 
                Console.WriteLine($"🟡 [DEBUG] Editando PlaylistId: {id}, Nombre: {data.Nombre}, UsuarioId: {data.UsuarioId}");

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
