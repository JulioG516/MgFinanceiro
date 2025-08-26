using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Domain.ValueObjects;

public record FiltroTransacao(
    DateTime? DataInicio = null,
    DateTime? DataFim = null,
    int? CategoriaId = null,
    TipoCategoria? Tipo = null,
    int Pagina = 1,
    int TamanhoPagina = 20)
{
    public bool TemFiltroData => DataInicio.HasValue || DataFim.HasValue;
    public bool TemFiltroCategoria => CategoriaId.HasValue;
    public bool TemFiltroTipo => Tipo.HasValue;
}