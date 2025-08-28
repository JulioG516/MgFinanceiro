using MgFinanceiro.Application.DTOs.Auth;
using MgFinanceiro.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace MgFinanceiro.Application.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public partial class AuthMapper
{
    [MapperIgnoreSource(nameof(Usuario.SenhaHash))]
    public static partial UsuarioDto UsuarioToDto(Usuario usuario);
}