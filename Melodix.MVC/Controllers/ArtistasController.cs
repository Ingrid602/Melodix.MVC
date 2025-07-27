using Melodix.APIConsumer;
using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melodix.MVC.Controllers
{
    public class ArtistasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtistasController(ApplicationDbContext context)
        {
            _context = context;
            
        }
        // GET: ArtistasController
        public ActionResult Index(string searchString)
        {
            var data = Crud<Artista>.GetAll();
            if (!string.IsNullOrEmpty(searchString))
            {
                data = data
                    .Where(a => a.NombreArtistico.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.CurrentFilter = searchString;
            return View(data);
        }

        // GET: ArtistasController/Details/5
        public ActionResult Details(int id)
        {
            var artista = _context.Artistas
         .Include(a => a.Albumes)
         .Include(a=>a.Seguidores)
         .FirstOrDefault(a => a.ArtistaId == id);

            if (artista == null) return NotFound();

            return View(artista);
        }

        // GET: ArtistasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtistasController/Create
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

        // GET: ArtistasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ArtistasController/Edit/5
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

        // GET: ArtistasController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ArtistasController/Delete/5
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

        [HttpPost]
        public async Task<IActionResult> SeguirArtista(string usuarioId, int artistaId)
        {
            var existe = Crud<SeguidoresArtistas>.GetAll()
                .Any(s => s.UsuarioId == usuarioId && s.ArtistaId == artistaId);

            if (existe)
            {
                // Elimina
                using (var client = new HttpClient())
                {
                    var result = await client.DeleteAsync($"https://localhost:7093/api/SeguidoresArtistas/{usuarioId}/{artistaId}");
                }
            }
            else
            {
                // Crea
                var nuevo = new SeguidoresArtistas
                {
                    UsuarioId = usuarioId,
                    ArtistaId = artistaId
                };
                Crud<SeguidoresArtistas>.Create(nuevo);
            }

            return RedirectToAction("Details", new { id = artistaId });
        }

    }
}
