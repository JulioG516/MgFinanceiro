namespace MgFinanceiro.Application.DTOs;

public class CreateCategoriaRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // Mapeado - Aceita -> "Receita" e "Despesa"
}