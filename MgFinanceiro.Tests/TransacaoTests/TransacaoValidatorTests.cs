using FluentValidation.TestHelper;
using MgFinanceiro.Application.DTOs.Transacao;
using MgFinanceiro.Application.Validators;
using Shouldly;

namespace MgFinanceiro.Tests.TransacaoTests;

public class TransacaoValidatorTests
{
    private readonly CreateTransacaoRequestValidator _createTransacaoRequestValidator;
    private readonly UpdateTransacaoRequestValidator _updateTransacaoRequestValidator;
    private readonly DateTime _currentDate; // para nao ter que instanciar toda hora

    public TransacaoValidatorTests()
    {
        _createTransacaoRequestValidator = new CreateTransacaoRequestValidator();
        _updateTransacaoRequestValidator = new UpdateTransacaoRequestValidator();
        _currentDate = new DateTime(2025, 8, 27, 17, 16, 0);
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
    
    [Fact]
    public void Should_Have_Error_When_Descricao_Is_Null()
    {
        var request = new CreateTransacaoRequest
        {
            Descricao = null!,
            Valor = 250,
            Data = _currentDate,
            CategoriaId = 1,
            Observacoes = null
        };

        var result = _createTransacaoRequestValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(t => t.Descricao)
            .WithErrorMessage("A descrição da transação é obrigatória.");
    }
    
    [Fact]
    public void Should_Have_Error_When_Descricao_Exceeds_200_Characters()
    {
        var longDescricao = new string('A', 201);
        var request = new CreateTransacaoRequest
        {
            Descricao = longDescricao,
            Valor = 250,
            Data = _currentDate,
            CategoriaId = 1,
            Observacoes = null
        };

        var result = _createTransacaoRequestValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(t => t.Descricao)
            .WithErrorMessage("A descrição não pode exceder 200 caracteres.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Valor_Is_Not_Greater_Than_Zero(decimal valor)
    {
        var request = new CreateTransacaoRequest
        {
            Descricao = "Transação",
            Valor = valor,
            Data = _currentDate,
            CategoriaId = 1,
            Observacoes = null
        };

        var result = _createTransacaoRequestValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(t => t.Valor)
            .WithErrorMessage("O valor da transação deve ser maior que zero.");
    }
    
    
    [Fact]
    public void Should_Have_Error_When_Data_Is_Empty()
    {
        var request = new CreateTransacaoRequest
        {
            Descricao = "Transação",
            Valor = 250,
            Data = default,
            CategoriaId = 1,
            Observacoes = null
        };

        var result = _createTransacaoRequestValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(t => t.Data)
            .WithErrorMessage("A data da transação é obrigatória.");
    }

    [Fact]
    public void Should_Have_Error_When_Data_Is_Future()
    {
        var futureDate = _currentDate.AddDays(1);
        var request = new CreateTransacaoRequest
        {
            Descricao = "Transação",
            Valor = 250,
            Data = futureDate,
            CategoriaId = 1,
            Observacoes = null
        };

        var result = _createTransacaoRequestValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(t => t.Data)
            .WithErrorMessage("A data da transação não pode ser futura.");
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_CategoriaId_Is_Not_Greater_Than_Zero(int categoriaId)
    {
        var request = new CreateTransacaoRequest
        {
            Descricao = "Transação",
            Valor = 250,
            Data = _currentDate,
            CategoriaId = categoriaId,
            Observacoes = null
        };
        
        var result = _createTransacaoRequestValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(t => t.CategoriaId)
            .WithErrorMessage("O ID da categoria é obrigatório e deve ser válido.");
    }
    [Fact]
    public void Should_Have_Error_When_Observacoes_Exceeds_500_Characters()
    {
        var longObservacoes = new string('A', 501);
        var request = new CreateTransacaoRequest
        {
            Descricao = "Transação",
            Valor = 250,
            Data = _currentDate,
            CategoriaId = 1,
            Observacoes = longObservacoes
        };

        var result = _createTransacaoRequestValidator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(t => t.Observacoes)
            .WithErrorMessage("As observações não podem exceder 500 caracteres.");
    }
    
    [Fact]
    public void Should_Not_Have_Error_When_Observacoes_Is_Null()
    {
        var request = new CreateTransacaoRequest
        {
            Descricao = "Transação",
            Valor = 250,
            Data = _currentDate,
            CategoriaId = 1,
            Observacoes = null
        };

        var result = _createTransacaoRequestValidator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(t => t.Observacoes);
    }
    
    [Fact]
    public void Should_Not_Have_Errors_When_Request_Is_Valid()
    {
        var request = new CreateTransacaoRequest
        {
            Descricao = "Transação válida",
            Valor = 250,
            Data = _currentDate,
            CategoriaId = 1,
            Observacoes = "Observação válida"
        };
        
        var result = _createTransacaoRequestValidator.TestValidate(request);

        result.IsValid.ShouldBeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }
    #endregion
}