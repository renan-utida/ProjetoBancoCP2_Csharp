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
                return NotFound(new { mensagem = "Contratação não encontrada." });

            return contratacao;
        }

        // POST: api/Contratacoes
        [HttpPost]
        public async Task<ActionResult<Contratacao>> PostContratacao(Contratacao contratacao)
        {
            // Verifica se cliente existe
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.IdCliente == contratacao.IdCliente);
            if (!clienteExiste)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            // Verifica se produto existe
            var produtoExiste = await _context.Produtos.AnyAsync(p => p.IdProduto == contratacao.IdProduto);
            if (!produtoExiste)
                return NotFound(new { mensagem = "Produto não encontrado." });

            contratacao.Status = "PENDENTE";
            contratacao.DtSolicitacao = DateTime.Now;

            _context.Contratacoes.Add(contratacao);
            await _context.SaveChangesAsync();

            // 202 Accepted — simula envio para fila de processamento
            return AcceptedAtAction(nameof(GetContratacao), new { id = contratacao.IdContratacao }, contratacao);
        }

        // PUT: api/Contratacoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContratacao(int id, Contratacao contratacao)
        {
            if (id != contratacao.IdContratacao)
                return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

            _context.Entry(contratacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContratacaoExists(id))
                    return NotFound(new { mensagem = "Contratação não encontrada." });
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Contratacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContratacao(int id)
        {
            var contratacao = await _context.Contratacoes.FindAsync(id);

            if (contratacao == null)
                return NotFound(new { mensagem = "Contratação não encontrada." });

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
