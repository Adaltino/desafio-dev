using desafio_dev.Models;

namespace desafio_dev.Services;

public static class JurosService
{
    private const decimal JurosPorDia = 0.025m;

    public static ResultadoJuros Calcular(
        decimal valor,
        DateTime dataVencimento)
    {
        var hoje = DateTime.Today;

        var diasAtraso = dataVencimento < hoje
            ? (hoje - dataVencimento).Days
            : 0;

        var percentualJuros = diasAtraso * JurosPorDia;

        var valorJuros = valor * percentualJuros;

        var valorTotal = valor + valorJuros;

        return new ResultadoJuros
        {
            ValorOriginal = valor,
            DiasAtraso = diasAtraso,
            PercentualJuros = percentualJuros,
            ValorJuros = valorJuros,
            ValorTotal = valorTotal
        };
    }
}
