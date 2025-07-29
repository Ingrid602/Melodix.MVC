using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melodix.MVC.Controllers
{
    public class DetallePagosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetallePagosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: DetallePagosController

        public IActionResult Index()
        {
            var lista = _context.DetallePagos
                .Include(d => d.MetodoPago)
                .Include(d => d.Factura)
                .ToList();

            return View(lista);
        }

        // GET: DetallePagosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DetallePagosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DetallePagosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection form)
        {
            var userId = _userManager.GetUserId(User);

            string numero = form["NumeroTarjeta"];
            string titular = form["NombreTitular"];
            string vencimiento = form["Vencimiento"];
            string codigo = form["CodigoSeguridad"];
            string metodo = form["MetodoPagoId"]; // Asegúrate que coincida con el name del input

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(numero) || numero.Length != 16 ||
                string.IsNullOrWhiteSpace(titular) ||
                string.IsNullOrWhiteSpace(vencimiento) ||
                string.IsNullOrWhiteSpace(codigo) ||
                string.IsNullOrWhiteSpace(metodo))
            {
                TempData["Mensaje"] = "Datos inválidos en el formulario.";
                return View();
            }

            // Validación de número entero para metodoPago
            if (!int.TryParse(metodo, out int metodoId))
            {
                TempData["Mensaje"] = "El método de pago no es válido.";
                return View();
            }

            // Crear y guardar el DetallePago
            var detalle = new DetallePago
            {
                NombreTitular = titular,
                NumeroTarjeta = numero,
                Vencimiento = vencimiento,
                CodigoSeguridad = codigo,
                Estado = "Validado",
                MetodoPagoId = metodoId,
                FacturaId = null // temporal
            };

            _context.DetallePagos.Add(detalle);
            _context.SaveChanges();

            // Redirige directamente a crear la suscripción con el PlanId fijo
            return RedirectToAction("Create", "Suscripciones", new { planId = 1 });
        }



        // GET: DetallePagosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DetallePagosController/Edit/5
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

        // GET: DetallePagosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DetallePagosController/Delete/5
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
