using MgFinanceiro.Domain.Common;
using MgFinanceiro.Domain.Entities;
using MgFinanceiro.Domain.Interfaces;
using MgFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MgFinanceiro.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UsuarioRepository> _logger;

    public UsuarioRepository(AppDbContext context, ILogger<UsuarioRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Result<Usuario>> CreateAsync(Usuario usuario)
    {
        try
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return Result<Usuario>.Success(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<Usuario>.Failure("Falha ao criar usuario.");
        }
    }

    public async Task UpdateLastLoginAsync(int usuarioId)
    {
        try
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario != null)
            {
                usuario.UltimoLogin = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Usuarios
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
}