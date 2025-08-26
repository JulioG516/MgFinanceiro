using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace MgFinanceiro.Application.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public partial class TransacaoMapper
{
    public partial Transacao CreateTransacaoRequestToTransacao(CreateTransacaoRequest request);
    public partial Transacao UpdateTransacaoRequestToTransacao(UpdateTransacaoRequest request);

    [MapProperty(nameof(Transacao.Categoria.Nome), nameof(TransacaoResponseDto.CategoriaNome))]
    [MapProperty(nameof(Transacao.Categoria.Tipo), nameof(TransacaoResponseDto.CategoriaTipo))]
    private partial TransacaoResponseDto MapTransacaoToResponse(Transacao transacao);

    public partial IEnumerable<TransacaoResponseDto> TransacoesToTransacaoResponses(IEnumerable<Transacao> transacoes);
}