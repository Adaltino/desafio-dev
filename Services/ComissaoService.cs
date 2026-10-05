using desafio_dev.Models;

namespace desafio_dev.Services;

public static class ComissaoService
{
    public static decimal CalcularComissao(decimal valor)
    {
        if (valor < 100)
            return 0;

        if (valor < 500)
            return valor * 0.01m;

        return valor * 0.05m;
    }

    public static List<ResultadoComissao> CalcularPorVendedor(List<Venda> vendas)
    {
        return vendas
            .GroupBy(venda => venda.Vendedor)
            .Select(grupo => new ResultadoComissao
            {
                Vendedor = grupo.Key,
                TotalVendas = grupo.Sum(venda => venda.Valor),
                TotalComissao = grupo.Sum(venda => CalcularComissao(venda.Valor))
            })
            .OrderBy(resultado => resultado.Vendedor)
            .ToList();
    }
}
