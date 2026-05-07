using ProjetoBancoCP2.Models;

namespace ProjetoBancoCP2.Services
{
    public class EmprestimoService
    {
        // Fórmula Price (juros compostos):
        // PMT = PV * (i * (1+i)^n) / ((1+i)^n - 1)
        // PV  = valor solicitado
        // i   = taxa de juros mensal (ex: 2.5 -> 0.025)
        // n   = prazo em meses
        public decimal CalcularParcela(decimal valorSolicitado, decimal taxaJuros, int prazoMeses)
        {
            if (prazoMeses <= 0)
                throw new ArgumentException("Prazo deve ser maior que zero.");

            if (valorSolicitado <= 0)
                throw new ArgumentException("Valor solicitado deve ser maior que zero.");

            if (taxaJuros <= 0)
                throw new ArgumentException("Taxa de juros deve ser maior que zero.");

            decimal i = taxaJuros / 100;
            decimal fator = (decimal)Math.Pow((double)(1 + i), prazoMeses);
            decimal parcela = valorSolicitado * (i * fator) / (fator - 1);

            return Math.Round(parcela, 2);
        }

        public string AvaliarScore(decimal valorSolicitado, int prazoMeses)
        {
            // Regra simples de score por valor e prazo
            if (valorSolicitado <= 20000 && prazoMeses <= 36)
                return "APROVADO";

            if (valorSolicitado <= 50000 && prazoMeses <= 60)
                return "ANALISE";

            return "REPROVADO";
        }

        public Emprestimo PreencherEmprestimo(Emprestimo emprestimo)
        {
            emprestimo.ValorParcela = CalcularParcela(
                emprestimo.ValorSolicitado,
                emprestimo.TaxaJuros,
                emprestimo.PrazoMeses
            );

            return emprestimo;
        }
    }
}