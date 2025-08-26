using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace MgFinanceiro.Application.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public partial class TransacaoMapper
{
    public static partial Transacao CreateTransacaoRequestToTransacao(CreateTransacaoRequest request);
    public static partial Transacao UpdateTransacaoRequestToTransacao(UpdateTransacaoRequest request);

    [MapProperty(nameof(Transacao.Categoria.Nome), nameof(TransacaoResponseDto.CategoriaNome))]
    [MapProperty(nameof(Transacao.Categoria.Tipo), nameof(TransacaoResponseDto.CategoriaTipo))]
    public static partial TransacaoResponseDto MapTransacaoToResponse(Transacao transacao);

    public static partial IEnumerable<TransacaoResponseDto> TransacoesToTransacaoResponses(
        IEnumerable<Transacao> transacoes);
}