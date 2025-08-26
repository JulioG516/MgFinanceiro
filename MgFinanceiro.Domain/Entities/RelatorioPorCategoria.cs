namespace MgFinanceiro.Domain.Entities;

public class RelatorioPorCategoria
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int CategoriaId { get; set; }
    public string CategoriaNome { get; set; } = string.Empty;
    public TipoCategoria CategoriaTipo { get; set; }
    public decimal Total { get; set; }
}