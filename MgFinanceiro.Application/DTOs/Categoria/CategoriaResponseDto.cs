namespace MgFinanceiro.Application.DTOs.Categoria;

public class CategoriaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}