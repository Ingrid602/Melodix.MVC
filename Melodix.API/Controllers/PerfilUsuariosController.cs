using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Melodix.Data.Data;
using Melodix.Modelos;

namespace Melodix.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilUsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PerfilUsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PerfilUsuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerfilUsuario>>> GetPerfilUsuarios()
        {
            return await _context.PerfilUsuarios.ToListAsync();
        }

        // GET: api/PerfilUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PerfilUsuario>> GetPerfilUsuario(string id)
        {
            var perfilUsuario = await _context.PerfilUsuarios.FindAsync(id);

            if (perfilUsuario == null)
            {
                return NotFound();
            }

            return perfilUsuario;
        }

        // PUT: api/PerfilUsuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerfilUsuario(string id, PerfilUsuario perfilUsuario)
        {
            if (id != perfilUsuario.PerfilUsuarioId)
            {
                return BadRequest();
            }

            _context.Entry(perfilUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerfilUsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PerfilUsuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PerfilUsuario>> PostPerfilUsuario(PerfilUsuario perfilUsuario)
        {
            _context.PerfilUsuarios.Add(perfilUsuario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PerfilUsuarioExists(perfilUsuario.PerfilUsuarioId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPerfilUsuario", new { id = perfilUsuario.PerfilUsuarioId }, perfilUsuario);
        }

        // DELETE: api/PerfilUsuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerfilUsuario(string id)
        {
            var perfilUsuario = await _context.PerfilUsuarios.FindAsync(id);
            if (perfilUsuario == null)
            {
                return NotFound();
            }

            _context.PerfilUsuarios.Remove(perfilUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PerfilUsuarioExists(string id)
        {
            return _context.PerfilUsuarios.Any(e => e.PerfilUsuarioId == id);
        }
    }
}
