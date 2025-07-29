using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melodix.MVC.Controllers
{
   

    public class SolicitudesMusicoController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SolicitudesMusicoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SolicitudesMusicoController
        public ActionResult Index()
        {
            var solicitudes = _context.SolicitudesMusico
        .Include(s => s.Usuario)
        .ToList();

            return View(solicitudes);
        }


        public async Task<IActionResult> Aceptar(int id)
        {
            var solicitud = await _context.SolicitudesMusico
                .Include(s => s.Usuario)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (solicitud == null) return NotFound();

            solicitud.Estado = EstadoSolicitud.Aprobada;

            // Cambiar el rol
            solicitud.Usuario.Rol = RolUsuario.Musico;

            // Verifica si ya tiene un artista
            var yaEsArtista = await _context.Artistas
                .AnyAsync(a => a.UsuarioId == solicitud.Usuario.Id);

            if (!yaEsArtista)
            {
                var nuevoArtista = new Artista
                {
                    UsuarioId = solicitud.Usuario.Id,
                    NombreArtistico = solicitud.NombreArtistico,
                    Biografia = "", // o asigna desde la solicitud
                    ImagenUrl = "/img/artistas/default-artista.jpg" // imagen por defecto si deseas
                };

                _context.Artistas.Add(nuevoArtista);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Rechazar(int id)
        {
            var solicitud = await _context.SolicitudesMusico.FindAsync(id);
            if (solicitud == null) return NotFound();

            solicitud.Estado = EstadoSolicitud.Rechazada;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }







        // GET: SolicitudesMusicoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SolicitudesMusicoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SolicitudesMusicoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string NombreArtistico, string Distribuidora, string Mensaje)
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return Unauthorized();

            var yaExiste = _context.SolicitudesMusico.Any(s =>
                s.UsuarioId == usuario.Id &&
                (s.Estado == EstadoSolicitud.Pendiente || s.Estado == EstadoSolicitud.Aprobada)
            );

            if (yaExiste)
            {
                TempData["Error"] = "Ya tienes una solicitud pendiente o aprobada.";
                return RedirectToAction("Details", "PerfilUsuarios", new { id = usuario.Id });
            }

            var solicitud = new SolicitudMusico
            {
                UsuarioId = usuario.Id,
                NombreArtistico = NombreArtistico,
                Distribuidora = Distribuidora,
                Mensaje = Mensaje,
                FechaSolicitud = DateTime.UtcNow,
                Estado = EstadoSolicitud.Pendiente
            };

            _context.SolicitudesMusico.Add(solicitud);
            await _context.SaveChangesAsync();

            TempData["Exito"] = "Tu solicitud fue enviada exitosamente.";
            return RedirectToAction("Details", "PerfilUsuarios", new { id = usuario.Id });
        }


        // GET: SolicitudesMusicoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SolicitudesMusicoController/Edit/5
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

        // GET: SolicitudesMusicoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SolicitudesMusicoController/Delete/5
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
