using System.Text.Json.Serialization;

namespace desafio_dev.Models;

public class EstoqueData
{
    [JsonPropertyName("estoque")]
    public List<Produto> Estoque { get; set; } = [];

    [JsonPropertyName("movimentacoes")]
    public List<MovimentacaoEstoque> Movimentacoes { get; set; } = [];
}
