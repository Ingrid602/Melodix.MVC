using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melodix.MVC.Controllers
{
    public class FacturasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FacturasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FacturasController/Create
        public IActionResult Create(int PlanId)
        {
            var plan = _context.Planes.FirstOrDefault(p => p.PlanId == PlanId);
            if (plan == null)
            {
                TempData["Mensaje"] = "El plan no existe.";
                return RedirectToAction("Index", "Canciones");
            }

            ViewBag.Plan = plan;
            return View(new Factura());
        }

        // POST: FacturasController/Create

        [HttpGet]
        public IActionResult Create(int detalleId, int planId, int suscripcionId)
        {
            var userId = _userManager.GetUserId(User);
            var plan = _context.Planes.Find(planId);

            var factura = new Factura
            {
                UsuarioId = userId,
                PlanId = planId,
                SuscripcionId = suscripcionId,
                Fecha = DateTime.Now,
                Total = plan?.PrecioMensual ?? 0,
                Estado = "Pagada"
            };

            _context.Facturas.Add(factura);
            _context.SaveChanges();

            // Asocia factura al detalle de pago
            var detalle = _context.DetallePagos.Find(detalleId);
            if (detalle != null)
            {
                detalle.FacturaId = factura.FacturaId;
                _context.DetallePagos.Update(detalle);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", "Facturas", new { id = factura.FacturaId });
        }


        public IActionResult Details(int id)
        {
            var factura = _context.Facturas
                .Include(f => f.Plan)
                .Include(f => f.Suscripcion)
                .Include(f => f.DetallesPago)
                .FirstOrDefault(f => f.FacturaId == id);

            if (factura == null || factura.UsuarioId != _userManager.GetUserId(User))
                return NotFound();

            return View(factura);
        }

    }
}
