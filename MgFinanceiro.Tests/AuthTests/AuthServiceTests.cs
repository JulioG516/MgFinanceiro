using FluentValidation;
using FluentValidation.Results;
using MgFinanceiro.Application.DTOs.Auth;
using MgFinanceiro.Application.Services;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;

namespace MgFinanceiro.Tests.AuthTests;

public class AuthServiceTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IValidator<UsuarioRegisterRequest>> _registerRequestValidatorMock;
    private readonly Mock<IValidator<LoginRequest>> _loginRequestValidatorMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _configurationMock = new Mock<IConfiguration>();
        _registerRequestValidatorMock = new Mock<IValidator<UsuarioRegisterRequest>>();
        _loginRequestValidatorMock = new Mock<IValidator<LoginRequest>>();

        var jwtSettingsSection = new Mock<IConfigurationSection>();
        jwtSettingsSection.Setup(s => s["SecretKey"]).Returns("supersecretkey12345678901234567890");
        jwtSettingsSection.Setup(s => s["Issuer"]).Returns("TestIssuer");
        jwtSettingsSection.Setup(s => s["Audience"]).Returns("TestAudience");
        jwtSettingsSection.Setup(s => s["ExpiryInMinutes"]).Returns("60");
        _configurationMock.Setup(c => c.GetSection("JwtSettings")).Returns(jwtSettingsSection.Object);

        _authService = new AuthService(
            _usuarioRepositoryMock.Object,
            _configurationMock.Object,
            _registerRequestValidatorMock.Object,
            _loginRequestValidatorMock.Object);
    }

    #region LoginAsync

    [Fact]
    public async Task LoginAsync_Should_Return_AuthResponse_When_Credentials_Are_Valid()
    {
        var loginRequest = new LoginRequest { Email = "test@example.com", Senha = "password123" };
        var usuario = new Usuario
        {
            Id = 1,
            Email = "test@example.com",
            Nome = "Test User",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            DataCriacao = DateTime.UtcNow
        };
        _loginRequestValidatorMock.Setup(v => v.ValidateAsync(loginRequest, default))
            .ReturnsAsync(new ValidationResult());
        _usuarioRepositoryMock.Setup(r => r.GetByEmailAsync(loginRequest.Email))
            .ReturnsAsync(usuario);
        _usuarioRepositoryMock.Setup(r => r.UpdateLastLoginAsync(usuario.Id))
            .Returns(Task.CompletedTask);

        var result = await _authService.LoginAsync(loginRequest);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Token.ShouldNotBeNullOrEmpty();
        result.Value.Nome.ShouldBe("Test User");
        result.Value.Email.ShouldBe("test@example.com");
        result.Value.ExpiresAt.ShouldBe(DateTime.UtcNow.AddMinutes(60), TimeSpan.FromSeconds(5));
        _loginRequestValidatorMock.Verify(v => v.ValidateAsync(loginRequest, default), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.GetByEmailAsync(loginRequest.Email), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.UpdateLastLoginAsync(usuario.Id), Times.Once());
    }
    
    [Fact]
    public async Task LoginAsync_Should_Return_Failure_When_Validation_Fails()
    {
        var loginRequest = new LoginRequest { Email = "", Senha = "" };
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Email", "Email é obrigatório"),
            new ValidationFailure("Senha", "Senha é obrigatória")
        });
        _loginRequestValidatorMock.Setup(v => v.ValidateAsync(loginRequest, default))
            .ReturnsAsync(validationResult);

        var result = await _authService.LoginAsync(loginRequest);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("Email é obrigatório");
        result.Error.ShouldContain("Senha é obrigatória");
        _loginRequestValidatorMock.Verify(v => v.ValidateAsync(loginRequest, default), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.GetByEmailAsync(It.IsAny<string>()), Times.Never());
        _usuarioRepositoryMock.Verify(r => r.UpdateLastLoginAsync(It.IsAny<int>()), Times.Never());
    }
    
    [Fact]
    public async Task LoginAsync_Should_Return_Failure_When_Email_Not_Found()
    {
        var loginRequest = new LoginRequest { Email = "test@example.com", Senha = "password123" };
        _loginRequestValidatorMock.Setup(v => v.ValidateAsync(loginRequest, default))
            .ReturnsAsync(new ValidationResult());
        _usuarioRepositoryMock.Setup(r => r.GetByEmailAsync(loginRequest.Email))
            .ReturnsAsync((Usuario)null);

        var result = await _authService.LoginAsync(loginRequest);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Email não cadastrado.");
        _loginRequestValidatorMock.Verify(v => v.ValidateAsync(loginRequest, default), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.GetByEmailAsync(loginRequest.Email), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.UpdateLastLoginAsync(It.IsAny<int>()), Times.Never());
    }
    
    [Fact]
    public async Task LoginAsync_Should_Return_Failure_When_Password_Is_Incorrect()
    {
        var loginRequest = new LoginRequest { Email = "test@example.com", Senha = "wrongpassword" };
        var usuario = new Usuario
        {
            Id = 1,
            Email = "test@example.com",
            Nome = "Test User",
            SenhaHash = BCrypt.Net.BCrypt.HashPassword("password123"),
            DataCriacao = DateTime.UtcNow
        };
        _loginRequestValidatorMock.Setup(v => v.ValidateAsync(loginRequest, default))
            .ReturnsAsync(new ValidationResult());
        _usuarioRepositoryMock.Setup(r => r.GetByEmailAsync(loginRequest.Email))
            .ReturnsAsync(usuario);

        var result = await _authService.LoginAsync(loginRequest);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Senha incorreta.");
        _loginRequestValidatorMock.Verify(v => v.ValidateAsync(loginRequest, default), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.GetByEmailAsync(loginRequest.Email), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.UpdateLastLoginAsync(It.IsAny<int>()), Times.Never());
    }

    #endregion
    #region RegisterAsync
    [Fact]
    public async Task RegisterAsync_Should_Return_UsuarioDto_When_Request_Is_Valid()
    {
        var registerRequest = new UsuarioRegisterRequest
        {
            Nome = "Test User",
            Email = "test@example.com",
            Senha = "password123"
        };
        var usuario = new Usuario
        {
            Id = 1,
            Nome = registerRequest.Nome,
            Email = registerRequest.Email.ToLower(),
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Senha),
            DataCriacao = DateTime.UtcNow
        };
        _registerRequestValidatorMock.Setup(v => v.ValidateAsync(registerRequest, default))
            .ReturnsAsync(new ValidationResult());
        _usuarioRepositoryMock.Setup(r => r.EmailExistsAsync(registerRequest.Email))
            .ReturnsAsync(false);
        _usuarioRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Usuario>()))
            .ReturnsAsync(Result<Usuario>.Success(usuario));

        var result = await _authService.RegisterAsync(registerRequest);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(1);
        result.Value.Nome.ShouldBe("Test User");
        result.Value.Email.ShouldBe("test@example.com");
        _registerRequestValidatorMock.Verify(v => v.ValidateAsync(registerRequest, default), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.EmailExistsAsync(registerRequest.Email), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.CreateAsync(It.Is<Usuario>(u => u.Email == "test@example.com" && u.Nome == "Test User")), Times.Once());
    }
    
      [Fact]
    public async Task RegisterAsync_Should_Return_Failure_When_Validation_Fails()
    {
        var registerRequest = new UsuarioRegisterRequest { Nome = "", Email = "", Senha = "" };
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Nome", "Nome é obrigatório"),
            new ValidationFailure("Email", "Email é obrigatório."),
            new ValidationFailure("Senha", "Senha deve ter pelo menos 6 caracteres")
        });
        _registerRequestValidatorMock.Setup(v => v.ValidateAsync(registerRequest, default))
            .ReturnsAsync(validationResult);

        var result = await _authService.RegisterAsync(registerRequest);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("Nome é obrigatório");
        result.Error.ShouldContain("Email é obrigatório.");
        result.Error.ShouldContain("Senha deve ter pelo menos 6 caracteres");
        _registerRequestValidatorMock.Verify(v => v.ValidateAsync(registerRequest, default), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.EmailExistsAsync(It.IsAny<string>()), Times.Never());
        _usuarioRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Usuario>()), Times.Never());
    }

    [Fact]
    public async Task RegisterAsync_Should_Return_Failure_When_Email_Already_Exists()
    {
        var registerRequest = new UsuarioRegisterRequest
        {
            Nome = "Test User",
            Email = "test@example.com",
            Senha = "password123"
        };
        _registerRequestValidatorMock.Setup(v => v.ValidateAsync(registerRequest, default))
            .ReturnsAsync(new ValidationResult());
        _usuarioRepositoryMock.Setup(r => r.EmailExistsAsync(registerRequest.Email))
            .ReturnsAsync(true);

        var result = await _authService.RegisterAsync(registerRequest);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Este email já esta em uso");
        _registerRequestValidatorMock.Verify(v => v.ValidateAsync(registerRequest, default), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.EmailExistsAsync(registerRequest.Email), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Usuario>()), Times.Never());
    }

    [Fact]
    public async Task RegisterAsync_Should_Return_Failure_When_Create_Fails()
    {
        var registerRequest = new UsuarioRegisterRequest
        {
            Nome = "Test User",
            Email = "test@example.com",
            Senha = "password123"
        };
        _registerRequestValidatorMock.Setup(v => v.ValidateAsync(registerRequest, default))
            .ReturnsAsync(new ValidationResult());
        _usuarioRepositoryMock.Setup(r => r.EmailExistsAsync(registerRequest.Email))
            .ReturnsAsync(false);
        _usuarioRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Usuario>()))
            .ReturnsAsync(Result<Usuario>.Failure("Falha ao criar usuario."));

        var result = await _authService.RegisterAsync(registerRequest);

        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe("Falha ao criar usuario.");
        _registerRequestValidatorMock.Verify(v => v.ValidateAsync(registerRequest, default), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.EmailExistsAsync(registerRequest.Email), Times.Once());
        _usuarioRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Usuario>()), Times.Once());
    }

    #endregion
}