using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoBancoCP2.Data;
using ProjetoBancoCP2.Models;

namespace ProjetoBancoCP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenciasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgenciasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Agencias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agencia>>> GetAgencias()
        {
            return await _context.Agencias.ToListAsync();
        }

        // GET: api/Agencias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agencia>> GetAgencia(int id)
        {
            var agencia = await _context.Agencias.FindAsync(id);

            if (agencia == null)
                return NotFound(new { mensagem = "Agência não encontrada." });

            return agencia;
        }

        // POST: api/Agencias
        [HttpPost]
        public async Task<ActionResult<Agencia>> PostAgencia(Agencia agencia)
        {
            _context.Agencias.Add(agencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgencia), new { id = agencia.IdAgencia }, agencia);
        }

        // PUT: api/Agencias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgencia(int id, Agencia agencia)
        {
            if (id != agencia.IdAgencia)
                return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

            _context.Entry(agencia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgenciaExists(id))
                    return NotFound(new { mensagem = "Agência não encontrada." });
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Agencias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgencia(int id)
        {
            var agencia = await _context.Agencias.FindAsync(id);

            if (agencia == null)
                return NotFound(new { mensagem = "Agência não encontrada." });

            _context.Agencias.Remove(agencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgenciaExists(int id)
        {
            return _context.Agencias.Any(e => e.IdAgencia == id);
        }
    }
}
