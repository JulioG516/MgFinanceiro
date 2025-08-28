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
using Microsoft.IdentityModel.Tokens;

namespace MgFinanceiro.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;
    private readonly IValidator<UsuarioRegisterRequest> _registerRequestValidator;
    private readonly IValidator<LoginRequest> _loginRequestValidator;

    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration,
        IValidator<UsuarioRegisterRequest> registerRequestValidator, IValidator<LoginRequest> loginRequestValidator)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
        _registerRequestValidator = registerRequestValidator;
        _loginRequestValidator = loginRequestValidator;
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest)
    {
        // Validação
        var validationResult = await _loginRequestValidator.ValidateAsync(loginRequest);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<AuthResponse>.Failure(errors);
        }
            
        var usuario = await _usuarioRepository.GetByEmailAsync(loginRequest.Email);

        if (usuario == null)
        {
            return Result<AuthResponse>.Failure("Email não cadastrado.");
        }

        if (!VerifyPassword(loginRequest.Senha, usuario.SenhaHash))
        {
            return Result<AuthResponse>.Failure("Senha incorreta.");
        }

        await _usuarioRepository.UpdateLastLoginAsync(usuario.Id);
        var token = GenerateJwtToken(usuario);

        return Result<AuthResponse>.Success(new AuthResponse
        {
            Token = token,
            Nome = usuario.Nome,
            Email = usuario.Email,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60) 
        });
        }

    public async Task<Result<UsuarioDto>> RegisterAsync(UsuarioRegisterRequest usuarioRegisterRequest)
    {
        var validationResult = await _registerRequestValidator.ValidateAsync(usuarioRegisterRequest);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<UsuarioDto>.Failure(errors);
        }

        if (await _usuarioRepository.EmailExistsAsync(usuarioRegisterRequest.Email))
        {
            return Result<UsuarioDto>.Failure("Este email já esta em uso");
        }

        var usuario = new Usuario
        {
            Nome = usuarioRegisterRequest.Nome,
            Email = usuarioRegisterRequest.Email.ToLower(),
            SenhaHash = HashPassword(usuarioRegisterRequest.Senha),
            DataCriacao = DateTime.UtcNow
        };

        var entityResult = await _usuarioRepository.CreateAsync(usuario);
        if (!entityResult.IsSuccess)
        {
            return Result<UsuarioDto>.Failure(entityResult.Error);
        }

        var dto = AuthMapper.UsuarioToDto(entityResult.Value!);
        return Result<UsuarioDto>.Success(dto);
    }

    public string GenerateJwtToken(Usuario usuario)
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

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string HashPassword(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public bool VerifyPassword(string senha, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(senha, hash);
    }
}