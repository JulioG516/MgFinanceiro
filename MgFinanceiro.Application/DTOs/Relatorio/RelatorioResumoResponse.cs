namespace MgFinanceiro.Application.DTOs.Relatorio;

public class RelatorioResumoResponse
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public decimal SaldoTotal { get; set; }
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
}