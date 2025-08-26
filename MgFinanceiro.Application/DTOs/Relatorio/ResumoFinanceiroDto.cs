namespace MgFinanceiro.Application.DTOs.Relatorio;

public record ResumoFinanceiroDto(
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo,
    DateTime DataInicio,
    DateTime DataFim,
    int QuantidadeTransacoes)
{
    public decimal SaldoPercentual => TotalReceitas > 0 ? (Saldo / TotalReceitas) * 100 : 0;
}