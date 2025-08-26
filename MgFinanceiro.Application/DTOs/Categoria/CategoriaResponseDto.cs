using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Application.DTOs.Categoria;

public class CategoriaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public TipoCategoria Tipo { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}