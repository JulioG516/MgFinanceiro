using FluentValidation.TestHelper;
using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Application.Validators;

namespace MgFinanceiro.Tests.TransacaoTests;

public class TransacaoValidatorTests
{
    private readonly CreateTransacaoRequestValidator _createTransacaoRequestValidator;
    private readonly UpdateTransacaoRequestValidator _updateTransacaoRequestValidator;

    public TransacaoValidatorTests()
    {
        _createTransacaoRequestValidator = new CreateTransacaoRequestValidator();
        _updateTransacaoRequestValidator = new UpdateTransacaoRequestValidator();
    }
    
    #region Create Transacao Request

    [Fact]
    public void Should_Have_Error_When_Request_Descricao_Is_Empty()
    {
        var request = new CreateTransacaoRequest
        {
            Descricao = "",
            Valor = 250,
            Data = DateTime.UtcNow,
            CategoriaId = 1,
            Observacoes = null,
        };
        
        var result = _createTransacaoRequestValidator.TestValidate(request);
        result.ShouldHaveValidationErrorFor(t => t.Descricao)
            .WithErrorMessage("A descrição da transação é obrigatória.");
    }
    

    #endregion

}