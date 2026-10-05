namespace desafio_dev.Models;

public class MovimentacaoEstoque
{
    public int Id { get; set; }
    public int CodigoProduto { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
