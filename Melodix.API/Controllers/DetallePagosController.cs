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
    public class DetallePagosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DetallePagosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/DetallePagos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePago>>> GetDetallePagos()
        {
            return await _context.DetallePagos.ToListAsync();
        }

        // GET: api/DetallePagos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePago>> GetDetallePago(int id)
        {
            var detallePago = await _context.DetallePagos.FindAsync(id);

            if (detallePago == null)
            {
                return NotFound();
            }

            return detallePago;
        }

        // PUT: api/DetallePagos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallePago(int id, DetallePago detallePago)
        {
            if (id != detallePago.DetallePagoId)
            {
                return BadRequest();
            }

            _context.Entry(detallePago).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetallePagoExists(id))
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

        // POST: api/DetallePagos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetallePago>> PostDetallePago(DetallePago detallePago)
        {
            _context.DetallePagos.Add(detallePago);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetallePago", new { id = detallePago.DetallePagoId }, detallePago);
        }

        // DELETE: api/DetallePagos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallePago(int id)
        {
            var detallePago = await _context.DetallePagos.FindAsync(id);
            if (detallePago == null)
            {
                return NotFound();
            }

            _context.DetallePagos.Remove(detallePago);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetallePagoExists(int id)
        {
            return _context.DetallePagos.Any(e => e.DetallePagoId == id);
        }
    }
}
