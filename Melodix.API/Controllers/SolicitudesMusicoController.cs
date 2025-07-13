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
    public class SolicitudesMusicoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SolicitudesMusicoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SolicitudesMusico
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudMusico>>> GetSolicitudesMusico()
        {
            return await _context.SolicitudesMusico.ToListAsync();
        }

        // GET: api/SolicitudesMusico/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudMusico>> GetSolicitudMusico(int id)
        {
            var solicitudMusico = await _context.SolicitudesMusico.FindAsync(id);

            if (solicitudMusico == null)
            {
                return NotFound();
            }

            return solicitudMusico;
        }

        // PUT: api/SolicitudesMusico/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitudMusico(int id, SolicitudMusico solicitudMusico)
        {
            if (id != solicitudMusico.Id)
            {
                return BadRequest();
            }

            _context.Entry(solicitudMusico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudMusicoExists(id))
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

        // POST: api/SolicitudesMusico
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SolicitudMusico>> PostSolicitudMusico(SolicitudMusico solicitudMusico)
        {
            _context.SolicitudesMusico.Add(solicitudMusico);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitudMusico", new { id = solicitudMusico.Id }, solicitudMusico);
        }

        // DELETE: api/SolicitudesMusico/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitudMusico(int id)
        {
            var solicitudMusico = await _context.SolicitudesMusico.FindAsync(id);
            if (solicitudMusico == null)
            {
                return NotFound();
            }

            _context.SolicitudesMusico.Remove(solicitudMusico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SolicitudMusicoExists(int id)
        {
            return _context.SolicitudesMusico.Any(e => e.Id == id);
        }
    }
}
