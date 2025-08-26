using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Application.DTOs;

public class CreateCategoriaRequest
{
    public string Nome { get; set; } = string.Empty;
    public TipoCategoria Tipo { get; set; }
}