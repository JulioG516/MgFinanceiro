namespace MgFinanceiro.Application.DTOs.Transacao;

public class TransacaoResponseDto
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNome { get; set; }
    public string? CategoriaTipo { get; set; } // Mapeia do Enum para um modo amigavel
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; }
}