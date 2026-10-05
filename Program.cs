using System.Text.Json;
using desafio_dev.Models;
using desafio_dev.Services;

const string caminhoVendas = "Data/vendas.json";
const string caminhoEstoque = "Data/estoque.json";

while (true)
{
    Console.Clear();

    Console.WriteLine("========================================");
    Console.WriteLine("             DESAFIO DEV");
    Console.WriteLine("========================================");
    Console.WriteLine("1 - Calcular comissões");
    Console.WriteLine("2 - Movimentar estoque");
    Console.WriteLine("3 - Calcular juros");
    Console.WriteLine("0 - Sair");
    Console.WriteLine("========================================");

    Console.Write("Escolha uma opção: ");
    var opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            CalcularComissoes();
            break;

        case "2":
            MovimentarEstoque();
            break;

        case "3":
            CalcularJuros();
            break;

        case "0":
            Console.WriteLine("Encerrando...");
            return;

        default:
            Console.WriteLine("Opção inválida.");
            break;
    }

    Console.WriteLine();
    Console.WriteLine("Pressione qualquer tecla para continuar...");
    Console.ReadKey();
}

void CalcularComissoes()
{
    if (!File.Exists(caminhoVendas))
    {
        Console.WriteLine($"Arquivo não encontrado: {caminhoVendas}");
        return;
    }

    var json = File.ReadAllText(caminhoVendas);

    var dados = JsonSerializer.Deserialize<VendasData>(json);

    if (dados is null || dados.Vendas.Count == 0)
    {
        Console.WriteLine("Nenhuma venda encontrada.");
        return;
    }

    var resultados = ComissaoService.CalcularPorVendedor(dados.Vendas);

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("       RELATÓRIO DE COMISSÕES");
    Console.WriteLine("========================================");

    foreach (var resultado in resultados)
    {
        Console.WriteLine($"Vendedor: {resultado.Vendedor}");
        Console.WriteLine($"Total em vendas: R$ {resultado.TotalVendas:N2}");
        Console.WriteLine($"Total de comissão: R$ {resultado.TotalComissao:N2}");
        Console.WriteLine("----------------------------------------");
    }
}

void MovimentarEstoque()
{
    if (!File.Exists(caminhoEstoque))
    {
        Console.WriteLine($"Arquivo não encontrado: {caminhoEstoque}");
        return;
    }

    var json = File.ReadAllText(caminhoEstoque);

    var dados = JsonSerializer.Deserialize<EstoqueData>(json);

    if (dados is null || dados.Estoque.Count == 0)
    {
        Console.WriteLine("Nenhum produto encontrado.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("          CONTROLE DE ESTOQUE");
    Console.WriteLine("========================================");

    Console.WriteLine("1 - Entrada");
    Console.WriteLine("2 - Saída");
    Console.WriteLine("0 - Voltar");

    Console.Write("Escolha uma opção: ");
    var opcao = Console.ReadLine();

    if (opcao == "0")
        return;

    if (opcao != "1" && opcao != "2")
    {
        Console.WriteLine("Opção inválida.");
        return;
    }

    Console.Write("Código do produto: ");

    if (!int.TryParse(Console.ReadLine(), out var codigoProduto))
    {
        Console.WriteLine("Código do produto inválido.");
        return;
    }

    var produto = EstoqueService.BuscarProduto(
        dados.Estoque,
        codigoProduto);

    if (produto is null)
    {
        Console.WriteLine("Produto não encontrado.");
        return;
    }

    Console.WriteLine($"Produto: {produto.DescricaoProduto}");
    Console.WriteLine($"Estoque atual: {produto.Estoque}");

    Console.Write("Quantidade: ");

    if (!int.TryParse(Console.ReadLine(), out var quantidade) ||
        quantidade <= 0)
    {
        Console.WriteLine("Quantidade inválida.");
        return;
    }

    Console.Write("Descrição da movimentação: ");
    var descricao = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(descricao))
    {
        Console.WriteLine("A descrição é obrigatória.");
        return;
    }

    MovimentacaoEstoque? movimentacao;

    if (opcao == "1")
    {
        movimentacao = EstoqueService.RegistrarEntrada(
            dados,
            produto,
            quantidade,
            descricao);
    }
    else
    {
        movimentacao = EstoqueService.RegistrarSaida(
            dados,
            produto,
            quantidade,
            descricao);

        if (movimentacao is null)
        {
            Console.WriteLine("Não há estoque suficiente para essa saída.");
            return;
        }
    }

    EstoqueService.SalvarEstoque(
        caminhoEstoque,
        dados);

    Console.WriteLine();
    Console.WriteLine("Movimentação registrada!");
    Console.WriteLine($"ID: {movimentacao.Id}");
    Console.WriteLine($"Produto: {produto.DescricaoProduto}");
    Console.WriteLine($"Tipo: {movimentacao.Tipo}");
    Console.WriteLine($"Quantidade: {movimentacao.Quantidade}");
    Console.WriteLine($"Descrição: {movimentacao.Descricao}");
    Console.WriteLine($"Estoque atual: {produto.Estoque}");
}

void CalcularJuros()
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("           CÁLCULO DE JUROS");
    Console.WriteLine("========================================");

    Console.Write("Valor da dívida: R$ ");

    if (!decimal.TryParse(
        Console.ReadLine(),
        out var valor) ||
        valor <= 0)
    {
        Console.WriteLine("Valor inválido.");
        return;
    }

    Console.Write("Data de vencimento (dd/MM/yyyy): ");

    if (!DateTime.TryParseExact(
        Console.ReadLine(),
        "dd/MM/yyyy",
        null,
        System.Globalization.DateTimeStyles.None,
        out var dataVencimento))
    {
        Console.WriteLine("Data inválida.");
        return;
    }

    var resultado = JurosService.Calcular(
        valor,
        dataVencimento);

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("             RESULTADO");
    Console.WriteLine("========================================");

    Console.WriteLine(
        $"Valor original: R$ {resultado.ValorOriginal:N2}");

    Console.WriteLine(
        $"Dias de atraso: {resultado.DiasAtraso}");

    Console.WriteLine(
        $"Juros: {resultado.PercentualJuros:P2}");

    Console.WriteLine(
        $"Valor dos juros: R$ {resultado.ValorJuros:N2}");

    Console.WriteLine(
        $"Valor total: R$ {resultado.ValorTotal:N2}");
}
