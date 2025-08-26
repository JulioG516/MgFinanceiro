namespace MgFinanceiro.Domain.Entities;

public class Transacao
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public int CategoriaId { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public Categoria? Categoria { get; set; }
}