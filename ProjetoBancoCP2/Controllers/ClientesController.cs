using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoBancoCP2.Data;
using ProjetoBancoCP2.Models;

namespace ProjetoBancoCP2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            return cliente;
        }

        // POST: api/Clientes/pf
        [HttpPost("pf")]
        public async Task<ActionResult<PessoaFisica>> PostPessoaFisica(PessoaFisica pf)
        {
            // Verifica se agência existe
            var agenciaExiste = await _context.Agencias.AnyAsync(a => a.IdAgencia == pf.IdAgencia);
            if (!agenciaExiste)
                return BadRequest(new { mensagem = "Agência informada não existe." });

            // Verifica CPF duplicado
            var cpfDuplicado = await _context.PessoasFisicas.AnyAsync(p => p.Cpf == pf.Cpf);
            if (cpfDuplicado)
                return BadRequest(new { mensagem = "CPF já cadastrado." });

            _context.PessoasFisicas.Add(pf);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = pf.IdCliente }, pf);
        }

        // POST: api/Clientes/pj
        [HttpPost("pj")]
        public async Task<ActionResult<PessoaJuridica>> PostPessoaJuridica(PessoaJuridica pj)
        {
            // Verifica se agência existe
            var agenciaExiste = await _context.Agencias.AnyAsync(a => a.IdAgencia == pj.IdAgencia);
            if (!agenciaExiste)
                return BadRequest(new { mensagem = "Agência informada não existe." });

            // Verifica CNPJ duplicado
            var cnpjDuplicado = await _context.PessoasJuridicas.AnyAsync(p => p.Cnpj == pj.Cnpj);
            if (cnpjDuplicado)
                return BadRequest(new { mensagem = "CNPJ já cadastrado." });

            _context.PessoasJuridicas.Add(pj);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = pj.IdCliente }, pj);
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.IdCliente)
                return BadRequest(new { mensagem = "ID da URL não confere com o ID do corpo." });

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                    return NotFound(new { mensagem = "Cliente não encontrado." });
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound(new { mensagem = "Cliente não encontrado." });

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IdCliente == id);
        }
    }
}
