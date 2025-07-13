using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Melodix.MVC.Controllers
{
    public class PerfilUsuariosController : Controller
    {
        // GET: PerfilUsuarios
        public ActionResult Index()
        {
            return View();
        }

        // GET: PerfilUsuarios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PerfilUsuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PerfilUsuarios/Create
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

        // GET: PerfilUsuarios/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PerfilUsuarios/Edit/5
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

        // GET: PerfilUsuarios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PerfilUsuarios/Delete/5
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
