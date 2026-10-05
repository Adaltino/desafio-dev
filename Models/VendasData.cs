using System.Text.Json.Serialization;

namespace desafio_dev.Models;

public class VendasData
{
    [JsonPropertyName("vendas")]
    public List<Venda> Vendas { get; set; } = [];
}
