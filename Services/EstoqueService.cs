using System.Text.Json;
using desafio_dev.Models;

namespace desafio_dev.Services;

public static class EstoqueService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    public static Produto? BuscarProduto(
        List<Produto> produtos,
        int codigoProduto)
    {
        return produtos.FirstOrDefault(
            produto => produto.CodigoProduto == codigoProduto);
    }

    public static MovimentacaoEstoque RegistrarEntrada(
        EstoqueData dados,
        Produto produto,
        int quantidade,
        string descricao)
    {
        produto.Estoque += quantidade;

        var movimentacao = new MovimentacaoEstoque
        {
            Id = GerarProximoId(dados.Movimentacoes),
            CodigoProduto = produto.CodigoProduto,
            Tipo = "Entrada",
            Quantidade = quantidade,
            Descricao = descricao
        };

        dados.Movimentacoes.Add(movimentacao);

        return movimentacao;
    }

    public static MovimentacaoEstoque? RegistrarSaida(
        EstoqueData dados,
        Produto produto,
        int quantidade,
        string descricao)
    {
        if (quantidade > produto.Estoque)
            return null;

        produto.Estoque -= quantidade;

        var movimentacao = new MovimentacaoEstoque
        {
            Id = GerarProximoId(dados.Movimentacoes),
            CodigoProduto = produto.CodigoProduto,
            Tipo = "Saida",
            Quantidade = quantidade,
            Descricao = descricao
        };

        dados.Movimentacoes.Add(movimentacao);

        return movimentacao;
    }

    public static void SalvarEstoque(
        string caminhoArquivo,
        EstoqueData dados)
    {
        var json = JsonSerializer.Serialize(dados, JsonOptions);

        File.WriteAllText(caminhoArquivo, json);
    }

    private static int GerarProximoId(
        List<MovimentacaoEstoque> movimentacoes)
    {
        if (movimentacoes.Count == 0)
            return 1;

        return movimentacoes.Max(movimentacao => movimentacao.Id) + 1;
    }
}
