using MgFinanceiro.Application.DTOs.Auth;
using MgFinanceiro.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MgFinanceiro.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Realiza o login de um usuário.
    /// </summary>
    /// <param name="request">Credenciais do usuário para login.</param>
    /// <returns>Token de autenticação.</returns>
    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Realizar login",
        Description = "Autentica um usuário com base nas credenciais fornecidas e retorna um token JWT."
    )]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(
        [FromBody, SwaggerRequestBody("Credenciais do usuário (e-mail e senha) para login", Required = true)]
        LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.IsSuccess)
        {
            return Unauthorized(result.Error);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Registra um novo usuário.
    /// </summary>
    /// <param name="usuarioRegisterRequest">Dados do usuário a ser registrado.</param>
    /// <returns>Detalhes do usuário registrado.</returns>
    [HttpPost("registrar")]
    [SwaggerOperation(
        Summary = "Registrar usuário",
        Description = "Cria um novo usuário com base nos dados fornecidos, nome, e-mail e senha."
    )]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> Register(
        [FromBody, SwaggerRequestBody("Dados do usuário a ser registrado", Required = true)]
        UsuarioRegisterRequest usuarioRegisterRequest)
    {
        var result = await _authService.RegisterAsync(usuarioRegisterRequest);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(
            nameof(Register),
            new { id = result.Value!.Id },
            result.Value
        );
    }
}