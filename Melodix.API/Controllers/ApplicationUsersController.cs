using Melodix.Data.Data;
using Melodix.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Melodix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ApplicationUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ApplicationUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsuarios()
        {
            return await _context.Users
               
                .Include(u => u.Perfil)
                .Include(u => u.Playlists)
                .Include(u => u.Suscripciones)
                .Include(u => u.Facturas)
                .Include(u => u.Historiales)
                .Include(u => u.Seguidores)
                .Include(u => u.Siguiendo)
                .Include(u => u.SeguidoresArtistas)
                .Include(u => u.PlaylistsGuardadas)
                .Include(u => u.AlbumesGuardados)
                .ToListAsync();
        }

        // GET: api/ApplicationUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUsuario(string id)
        {
            var usuario = await _context.Users
                .Include(u => u.Perfil)
                .Include(u => u.Playlists)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            return usuario;
        }

        // POST: api/ApplicationUsers
        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> CrearUsuario(ApplicationUser usuario)
        {
            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT: api/ApplicationUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(string id, ApplicationUser usuario)
        {
            if (id != usuario.Id)
                return BadRequest();

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/ApplicationUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(string id)
        {
            var usuario = await _context.Users.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Users.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
