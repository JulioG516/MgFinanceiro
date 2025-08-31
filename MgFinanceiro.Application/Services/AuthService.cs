using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using MgFinanceiro.Application.DTOs.Auth;
using MgFinanceiro.Application.Interfaces;
using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace MgFinanceiro.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;
    private readonly IValidator<UsuarioRegisterRequest> _registerRequestValidator;
    private readonly IValidator<LoginRequest> _loginRequestValidator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IConfiguration configuration,
        IValidator<UsuarioRegisterRequest> registerRequestValidator,
        IValidator<LoginRequest> loginRequestValidator,
        ILogger<AuthService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
        _registerRequestValidator = registerRequestValidator;
        _loginRequestValidator = loginRequestValidator;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest)
    {
        _logger.LogInformation("Iniciando processo de login para o email: {Email}", loginRequest.Email);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Validação
            _logger.LogDebug("Validando requisição de login para o email: {Email}", loginRequest.Email);
            var validationResult = await _loginRequestValidator.ValidateAsync(loginRequest);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validação de login falhou. Erros: {ErrosValidacao}, Email: {Email}", 
                    errors, loginRequest.Email);
                stopwatch.Stop();
                return Result<AuthResponse>.Failure(errors);
            }

            _logger.LogDebug("Buscando usuário pelo email: {Email}", loginRequest.Email);
            var usuario = await _usuarioRepository.GetByEmailAsync(loginRequest.Email);

            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado para o email: {Email}", loginRequest.Email);
                stopwatch.Stop();
                return Result<AuthResponse>.Failure("Email não cadastrado.");
            }

            _logger.LogDebug("Verificando senha para o usuário: {Email}", loginRequest.Email);
            if (!VerifyPassword(loginRequest.Senha, usuario.SenhaHash))
            {
                _logger.LogWarning("Falha na verificação de senha para o email: {Email}", loginRequest.Email);
                stopwatch.Stop();
                return Result<AuthResponse>.Failure("Senha incorreta.");
            }

            _logger.LogDebug("Atualizando última data de login para o usuário: {Email}", loginRequest.Email);
            await _usuarioRepository.UpdateLastLoginAsync(usuario.Id);

            _logger.LogDebug("Gerando token JWT para o usuário: {Email}", loginRequest.Email);
            var token = GenerateJwtToken(usuario);

            var response = new AuthResponse
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Email,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };

            stopwatch.Stop();
            _logger.LogInformation("Login realizado com sucesso. Email: {Email}, Nome: {Nome}, Tempo de execução: {TempoMs}ms",
                usuario.Email, usuario.Nome, stopwatch.ElapsedMilliseconds);

            return Result<AuthResponse>.Success(response);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro inesperado durante o login. Email: {Email}, Tempo decorrido: {TempoMs}ms", 
                loginRequest.Email, stopwatch.ElapsedMilliseconds);
            return Result<AuthResponse>.Failure("Erro interno ao processar login.");
        }
    }

    public async Task<Result<UsuarioDto>> RegisterAsync(UsuarioRegisterRequest usuarioRegisterRequest)
    {
        _logger.LogInformation("Iniciando registro de novo usuário. Email: {Email}, Nome: {Nome}", 
            usuarioRegisterRequest.Email, usuarioRegisterRequest.Nome);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Validação
            _logger.LogDebug("Validando requisição de registro para o email: {Email}", usuarioRegisterRequest.Email);
            var validationResult = await _registerRequestValidator.ValidateAsync(usuarioRegisterRequest);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Validação de registro falhou. Erros: {ErrosValidacao}, Email: {Email}, Nome: {Nome}", 
                    errors, usuarioRegisterRequest.Email, usuarioRegisterRequest.Nome);
                stopwatch.Stop();
                return Result<UsuarioDto>.Failure(errors);
            }

            _logger.LogDebug("Verificando se o email já está em uso: {Email}", usuarioRegisterRequest.Email);
            if (await _usuarioRepository.EmailExistsAsync(usuarioRegisterRequest.Email))
            {
                _logger.LogWarning("Tentativa de registro com email já existente: {Email}", usuarioRegisterRequest.Email);
                stopwatch.Stop();
                return Result<UsuarioDto>.Failure("Este email já está em uso.");
            }

            var usuario = new Usuario
            {
                Nome = usuarioRegisterRequest.Nome,
                Email = usuarioRegisterRequest.Email.ToLower(),
                SenhaHash = HashPassword(usuarioRegisterRequest.Senha),
                DataCriacao = DateTime.UtcNow
            };

            _logger.LogDebug("Persistindo novo usuário no repositório. Email: {Email}, Nome: {Nome}", 
                usuario.Email, usuario.Nome);

            var entityResult = await _usuarioRepository.CreateAsync(usuario);
            if (!entityResult.IsSuccess)
            {
                _logger.LogError("Falha ao criar usuário no repositório. Erro: {Erro}, Email: {Email}, Nome: {Nome}", 
                    entityResult.Error, usuario.Email, usuario.Nome);
                stopwatch.Stop();
                return Result<UsuarioDto>.Failure(entityResult.Error);
            }

            var dto = AuthMapper.UsuarioToDto(entityResult.Value!);
            stopwatch.Stop();
            _logger.LogInformation("Usuário registrado com sucesso. ID: {UsuarioId}, Email: {Email}, Nome: {Nome}, Tempo de execução: {TempoMs}ms",
                entityResult.Value!.Id, usuario.Email, usuario.Nome, stopwatch.ElapsedMilliseconds);

            return Result<UsuarioDto>.Success(dto);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro inesperado ao registrar usuário. Email: {Email}, Nome: {Nome}, Tempo decorrido: {TempoMs}ms", 
                usuarioRegisterRequest.Email, usuarioRegisterRequest.Nome, stopwatch.ElapsedMilliseconds);
            return Result<UsuarioDto>.Failure("Erro interno ao registrar usuário.");
        }
    }

    public string GenerateJwtToken(Usuario usuario)
    {
        _logger.LogDebug("Iniciando geração de token JWT para o usuário: {Email}", usuario.Email);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Name, usuario.Nome),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryInMinutes"]!)),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            stopwatch.Stop();
            _logger.LogInformation("Token JWT gerado com sucesso para o usuário: {Email}, Tempo de execução: {TempoMs}ms",
                usuario.Email, stopwatch.ElapsedMilliseconds);

            return tokenString;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro ao gerar token JWT para o usuário: {Email}, Tempo decorrido: {TempoMs}ms", 
                usuario.Email, stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public string HashPassword(string senha)
    {
        _logger.LogDebug("Iniciando hash de senha");
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(senha);
            stopwatch.Stop();
            _logger.LogDebug("Senha hasheada com sucesso, Tempo de execução: {TempoMs}ms", stopwatch.ElapsedMilliseconds);
            return hash;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro ao hashear senha, Tempo decorrido: {TempoMs}ms", stopwatch.ElapsedMilliseconds);
            throw;
        }
    }

    public bool VerifyPassword(string senha, string hash)
    {
        _logger.LogDebug("Iniciando verificação de senha");
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = BCrypt.Net.BCrypt.Verify(senha, hash);
            stopwatch.Stop();
            _logger.LogDebug("Verificação de senha concluída. Resultado: {Resultado}, Tempo de execução: {TempoMs}ms", 
                result, stopwatch.ElapsedMilliseconds);
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Erro ao verificar senha, Tempo decorrido: {TempoMs}ms", stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}