using MgFinanceiro.Application.Mappers;
using MgFinanceiro.Domain.Entities;
using Shouldly;

namespace MgFinanceiro.Tests.AuthTests;

public class AuthMapperTests
{
    [Fact]
    private void Should_Map_Correctly_To_Usuario_Entity_To_Dto()
    {
        var createdAt = DateTime.Now;

        var usuario = new Usuario
        {
            Id = 1,
            Nome = "Usuario Teste",
            Email = "teste@exemplo.com",
            SenhaHash = "hashDeUmaSenha",
            DataCriacao = createdAt,
            UltimoLogin = createdAt.AddDays(-1),
        };

        var dto = AuthMapper.UsuarioToDto(usuario);
        dto.ShouldNotBeNull();
        dto.Id.ShouldBe(1);
        dto.Nome.ShouldBe("Usuario Teste");
        dto.Email.ShouldBe("teste@exemplo.com");
        dto.DataCriacao.ShouldBe(createdAt);
        dto.UltimoLogin.ShouldBe(createdAt.AddDays(-1));
    }
}