using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Application.DTOs;

public record ResumoCategoriaDto(
    int CategoriaId,
    string NomeCategoria,
    TipoCategoria Tipo,
    decimal Total,
    int QuantidadeTransacoes)
{
    public decimal ValorMedio => QuantidadeTransacoes > 0 ? Total / QuantidadeTransacoes : 0;
}