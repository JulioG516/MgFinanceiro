namespace MgFinanceiro.Application.DTOs.Relatorio;

public class RelatorioPorCategoriaResponse
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int CategoriaId { get; set; }
    public string CategoriaNome { get; set; } = string.Empty;
    public string CategoriaTipo { get; set; } = string.Empty;
    public decimal Total { get; set; }
}