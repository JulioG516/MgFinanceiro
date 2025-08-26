namespace MgFinanceiro.Domain.Entities;

public class Categoria
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public TipoCategoria Tipo { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

}