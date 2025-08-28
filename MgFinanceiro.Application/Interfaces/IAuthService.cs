using MgFinanceiro.Application.DTOs.Auth;
using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Application.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> LoginAsync(LoginRequest loginRequest);
    Task<Result<UsuarioDto>> RegisterAsync(UsuarioRegisterRequest usuarioRegisterRequest);
    string GenerateJwtToken(Usuario usuario);
    string HashPassword(string senha);
    bool VerifyPassword(string senha, string hash);
}