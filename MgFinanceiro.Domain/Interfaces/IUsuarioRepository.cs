using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;

namespace MgFinanceiro.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> GetByIdAsync(int id);
    Task<Result<Usuario>> CreateAsync(Usuario usuario);
    Task UpdateLastLoginAsync(int usuarioId);
    Task<bool> EmailExistsAsync(string email);
}