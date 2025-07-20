using Melodix.APIConsumer;
using Melodix.Data.Data;
using Melodix.Modelos;
using Melodix.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Melodix.MVC.Controllers
{
    public class BuscarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BuscarController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            // Establecer endpoints
            Crud<Cancion>.EndPoint = "https://localhost:7093/api/Canciones";
            Crud<Album>.EndPoint = "https://localhost:7093/api/Albums";
            Crud<Playlist>.EndPoint = "https://localhost:7093/api/Playlists";
            Crud<ApplicationUser>.EndPoint = "https://localhost:7093/api/ApplicationUsers";
            Crud<Artista>.EndPoint = "https://localhost:7093/api/Artistas";

            // Obtener todos los datos
            var canciones = Crud<Cancion>.GetAll();
            var albumes = Crud<Album>.GetAll();
            var playlists = Crud<Playlist>.GetAll();
            var usuarios = Crud<ApplicationUser>.GetAll();
            var artistas = Crud<Artista>.GetAll();

            // Filtrar
            var resultadosCanciones = canciones.Where(c => c.Nombre.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
            var resultadosAlbumes = albumes.Where(a => a.Nombre.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
            var resultadosPlaylists = playlists.Where(p => p.Nombre.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
            var resultadosUsuarios = usuarios.Where(u => u.Nombre.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();
            var resultadosArtistas = artistas.Where(a => a.NombreArtistico.Contains(search ?? "", StringComparison.OrdinalIgnoreCase)).ToList();

            // Vincular Usuario con Perfil
            var perfiles = resultadosUsuarios
                .Where(u => u.Perfil != null)
                .Select(u => {
                    u.Perfil.Usuario = u;
                    return u.Perfil;
                }).ToList();

            // Crear modelo
            var modelo = new BuscarViewModel
            {
                Termino = search ?? "",
                Canciones = resultadosCanciones,
                Albumes = resultadosAlbumes,
                Playlists = resultadosPlaylists,
                Perfiles = perfiles,
                Artistas = resultadosArtistas
            };

            return View("Index", modelo);

        }

        [HttpGet]
        public IActionResult VistaParcial(string query)
        {
            var model = Index(query); // Tu método que llena BuscarViewModel
            return PartialView("~/Views/Shared/_ResultadosBusqueda.cshtml", model);
        }

    }
}
