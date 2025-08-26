using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Application.DTOs.Transacao;

public class TransacaoResponseDto
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNome { get; set; }
    public TipoCategoria? CategoriaTipo { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; }
}