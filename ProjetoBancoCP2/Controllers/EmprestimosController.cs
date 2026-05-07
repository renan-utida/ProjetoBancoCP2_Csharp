using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoBancoCP2.Data;
using ProjetoBancoCP2.Models;
using ProjetoBancoCP2.Services;

namespace ProjetoBancoCP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EmprestimoService _emprestimoService;

        public EmprestimosController(AppDbContext context, EmprestimoService emprestimoService)
        {
            _context = context;
            _emprestimoService = emprestimoService;
        }

        // GET: api/Emprestimos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emprestimo>>> GetEmprestimos()
        {
            return await _context.Emprestimos.ToListAsync();
        }

        // GET: api/Emprestimos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Emprestimo>> GetEmprestimo(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);

            if (emprestimo == null)
                return NotFound(new { mensagem = "Empréstimo não encontrado." });

            return emprestimo;
        }

        // POST: api/Emprestimos
        [HttpPost]
        public async Task<ActionResult<Emprestimo>> PostEmprestimo(Emprestimo emprestimo)
        {
            // Calcula parcela e avalia score automaticamente
            emprestimo = _emprestimoService.PreencherEmprestimo(emprestimo);
            var score = _emprestimoService.AvaliarScore(emprestimo.ValorSolicitado, emprestimo.PrazoMeses);

            if (score == "REPROVADO")
                return BadRequest(new { mensagem = "Empréstimo reprovado por score de crédito.", score });

            _context.Emprestimos.Add(emprestimo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmprestimo), new { id = emprestimo.IdProduto }, new
            {
                emprestimo,
                score,
                mensagem = "Empréstimo criado com sucesso."
            });
        }

        // PUT: api/Emprestimos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmprestimo(int id, Emprestimo emprestimo)
        {
            if (id != emprestimo.IdProduto)
                return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

            // Recalcula parcela ao atualizar
            emprestimo = _emprestimoService.PreencherEmprestimo(emprestimo);

            _context.Entry(emprestimo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmprestimoExists(id))
                    return NotFound(new { mensagem = "Empréstimo não encontrado." });
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Emprestimos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmprestimo(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);

            if (emprestimo == null)
                return NotFound(new { mensagem = "Empréstimo não encontrado." });

            _context.Emprestimos.Remove(emprestimo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmprestimoExists(int id)
        {
            return _context.Emprestimos.Any(e => e.IdProduto == id);
        }
    }
}
 