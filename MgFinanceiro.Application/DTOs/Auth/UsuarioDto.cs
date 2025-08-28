namespace MgFinanceiro.Application.DTOs.Auth;

public class UsuarioDto
{
    public int Id { get; set; }
        
    public string Nome { get; set; } = string.Empty;
        
    public string Email { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? UltimoLogin { get; set; }

}