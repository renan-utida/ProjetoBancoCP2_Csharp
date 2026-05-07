using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoBancoCP2.Data;
using ProjetoBancoCP2.Models;

namespace ProjetoBancoCP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContratacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Contratacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contratacao>>> GetContratacoes()
        {
            return await _context.Contratacoes.ToListAsync();
        }

        // GET: api/Contratacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contratacao>> GetContratacao(int id)
        {
            var contratacao = await _context.Contratacoes.FindAsync(id);

            if (contratacao == null)
            {
                return NotFound();
            }

            return contratacao;
        }

        // PUT: api/Contratacoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContratacao(int id, Contratacao contratacao)
        {
            if (id != contratacao.IdContratacao)
            {
                return BadRequest();
            }

            _context.Entry(contratacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContratacaoExists(id))
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

        // POST: api/Contratacoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contratacao>> PostContratacao(Contratacao contratacao)
        {
            _context.Contratacoes.Add(contratacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContratacao", new { id = contratacao.IdContratacao }, contratacao);
        }

        // DELETE: api/Contratacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContratacao(int id)
        {
            var contratacao = await _context.Contratacoes.FindAsync(id);
            if (contratacao == null)
            {
                return NotFound();
            }

            _context.Contratacoes.Remove(contratacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContratacaoExists(int id)
        {
            return _context.Contratacoes.Any(e => e.IdContratacao == id);
        }
    }
}
