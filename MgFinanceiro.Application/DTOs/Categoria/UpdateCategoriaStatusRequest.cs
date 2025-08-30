namespace MgFinanceiro.Application.DTOs.Categoria;

public class UpdateCategoriaStatusRequest
{
    public int Id { get; set; }
    public bool Ativo { get; set; }
}