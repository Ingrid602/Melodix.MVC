using Melodix.APIConsumer;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Melodix.MVC.Controllers
{
    public class ArtistasController : Controller
    {
        // GET: ArtistasController
        public ActionResult Index()
        {
            var data = Crud <Artista>.GetAll();
            return View(data);
        }

        // GET: ArtistasController/Details/5
        public ActionResult Details(int id)
        {
            var data = Crud <Artista>.GetById(id);
            return View(data);
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
    }
}
