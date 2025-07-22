using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Melodix.MVC.Controllers
{
    public class PlaylistCancionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlaylistCancionesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int CancionId, int PlaylistId)
        {
            // Verifica si ya existe esa relación
            var existe = _context.PlaylistCanciones
                .Any(pc => pc.CancionId == CancionId && pc.PlaylistId == PlaylistId);

            if (existe)
            {
                TempData["Mensaje"] = "❌ La canción ya está en la playlist.";
                return RedirectToAction("Index", "Canciones");
            }

            var relacion = new PlaylistCancion
            {
                CancionId = CancionId,
                PlaylistId = PlaylistId,
                Orden = 0
            };

            _context.PlaylistCanciones.Add(relacion);
            _context.SaveChanges();

            TempData["Mensaje"] = "✅ Canción agregada a la playlist.";
            return RedirectToAction("Index", "Canciones");
        }




        // GET: PlaylistCanciones
        public ActionResult Index()
        {
            return View();
        }

        // GET: PlaylistCanciones/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlaylistCanciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlaylistCanciones/Create
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

        // GET: PlaylistCanciones/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlaylistCanciones/Edit/5
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

        // GET: PlaylistCanciones/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int PlaylistId, int CancionId)
        {
            var relacion = _context.PlaylistCanciones
                .FirstOrDefault(pc => pc.PlaylistId == PlaylistId && pc.CancionId == CancionId);

            if (relacion != null)
            {
                _context.PlaylistCanciones.Remove(relacion);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", "Playlists", new { id = PlaylistId });
        }



        // POST: PlaylistCanciones/Delete/5
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
