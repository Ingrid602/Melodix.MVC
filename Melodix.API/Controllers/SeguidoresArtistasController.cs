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
    public class SeguidoresArtistasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeguidoresArtistasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SeguidoresArtistas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeguidoresArtistas>>> GetSeguidoresArtistas()
        {
            return await _context.SeguidoresArtistas.ToListAsync();
        }

        // GET: api/SeguidoresArtistas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SeguidoresArtistas>> GetSeguidoresArtistas(string id)
        {
            var seguidoresArtistas = await _context.SeguidoresArtistas.FindAsync(id);

            if (seguidoresArtistas == null)
            {
                return NotFound();
            }

            return seguidoresArtistas;
        }

        // PUT: api/SeguidoresArtistas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeguidoresArtistas(string id, SeguidoresArtistas seguidoresArtistas)
        {
            if (id != seguidoresArtistas.UsuarioId)
            {
                return BadRequest();
            }

            _context.Entry(seguidoresArtistas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeguidoresArtistasExists(id))
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

        // POST: api/SeguidoresArtistas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SeguidoresArtistas>> PostSeguidoresArtistas(SeguidoresArtistas seguidoresArtistas)
        {
            _context.SeguidoresArtistas.Add(seguidoresArtistas);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SeguidoresArtistasExists(seguidoresArtistas.UsuarioId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSeguidoresArtistas", new { id = seguidoresArtistas.UsuarioId }, seguidoresArtistas);
        }

        // DELETE: api/SeguidoresArtistas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeguidoresArtistas(string id)
        {
            var seguidoresArtistas = await _context.SeguidoresArtistas.FindAsync(id);
            if (seguidoresArtistas == null)
            {
                return NotFound();
            }

            _context.SeguidoresArtistas.Remove(seguidoresArtistas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeguidoresArtistasExists(string id)
        {
            return _context.SeguidoresArtistas.Any(e => e.UsuarioId == id);
        }
    }
}
