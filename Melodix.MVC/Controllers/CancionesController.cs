using Melodix.APIConsumer;
using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Melodix.MVC.Controllers
{
    public class CancionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CancionesController(ApplicationDbContext context)
        {
            _context = context;
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

            return View(canciones);
        }

        // GET: CancionesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CancionesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CancionesController/Create
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

        // GET: CancionesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CancionesController/Edit/5
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

        // GET: CancionesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CancionesController/Delete/5
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
