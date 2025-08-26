namespace MgFinanceiro.Domain.ValueObjects;

public record Periodo(DateTime DataInicio, DateTime DataFim)
{
    public bool IsValid => DataInicio <= DataFim;
    
    public bool ContemData(DateTime data) => data >= DataInicio && data <= DataFim;
    
    public static Periodo MesAtual()
    {
        var hoje = DateTime.Today;
        var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
        var fimMes = inicioMes.AddMonths(1).AddDays(-1);
        return new Periodo(inicioMes, fimMes);
    }
    
    public static Periodo UltimosMeses(int meses)
    {
        var hoje = DateTime.Today;
        var dataInicio = hoje.AddMonths(-meses);
        return new Periodo(dataInicio, hoje);
    }
}
