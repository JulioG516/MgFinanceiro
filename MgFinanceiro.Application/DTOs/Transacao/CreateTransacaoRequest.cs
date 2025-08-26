namespace MgFinanceiro.Application.DTOs.Transacao;

public class CreateTransacaoRequest
{
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
    public int CategoriaId { get; set; }
    public string? Observacoes { get; set; }
}