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
    public class SeguidoresUsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeguidoresUsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SeguidoresUsuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeguidoresUsuarios>>> GetSeguidoresUsuarios()
        {
            return await _context.SeguidoresUsuarios.ToListAsync();
        }

        // GET: api/SeguidoresUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeguidoresUsuarios>> GetSeguidoresUsuarios(string id)
        {
            var seguidoresUsuarios = await _context.SeguidoresUsuarios.FindAsync(id);

            if (seguidoresUsuarios == null)
            {
                return NotFound();
            }

            return seguidoresUsuarios;
        }

        // PUT: api/SeguidoresUsuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeguidoresUsuarios(string id, SeguidoresUsuarios seguidoresUsuarios)
        {
            if (id != seguidoresUsuarios.SeguidorId)
            {
                return BadRequest();
            }

            _context.Entry(seguidoresUsuarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeguidoresUsuariosExists(id))
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

        // POST: api/SeguidoresUsuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SeguidoresUsuarios>> PostSeguidoresUsuarios(SeguidoresUsuarios seguidoresUsuarios)
        {
            _context.SeguidoresUsuarios.Add(seguidoresUsuarios);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SeguidoresUsuariosExists(seguidoresUsuarios.SeguidorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSeguidoresUsuarios", new { id = seguidoresUsuarios.SeguidorId }, seguidoresUsuarios);
        }

        // DELETE: api/SeguidoresUsuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeguidoresUsuarios(string id)
        {
            var seguidoresUsuarios = await _context.SeguidoresUsuarios.FindAsync(id);
            if (seguidoresUsuarios == null)
            {
                return NotFound();
            }

            _context.SeguidoresUsuarios.Remove(seguidoresUsuarios);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeguidoresUsuariosExists(string id)
        {
            return _context.SeguidoresUsuarios.Any(e => e.SeguidorId == id);
        }
    }
}
