using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Melodix.MVC.Controllers
{
    public class SuscripcionesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuscripcionesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: SuscripcionesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SuscripcionesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SuscripcionesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SuscripcionesController/Create
        [HttpGet]
        public IActionResult Create(int detalleId, int planId)
        {
            var userId = _userManager.GetUserId(User);

            var suscripcion = new Suscripcion
            {
                UsuarioId = userId,
                PlanId = planId,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddMonths(1),
                Activo = true
            };

            _context.Suscripciones.Add(suscripcion);
            _context.SaveChanges();

            // Redirige a crear factura automáticamente
            return RedirectToAction("Create", "Facturas", new { detalleId, planId, suscripcionId = suscripcion.SuscripcionId });
        }


        // GET: SuscripcionesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SuscripcionesController/Edit/5
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

        // GET: SuscripcionesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SuscripcionesController/Delete/5
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
