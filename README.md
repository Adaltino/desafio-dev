# Desafio Dev

Solução de um desafio técnico desenvolvido em **C# / .NET 10**, contendo três exercícios relacionados a vendas, controle de estoque e cálculo de juros.

O projeto foi desenvolvido como uma aplicação **Console**, com separação entre modelos, regras de negócio e interação com o usuário.

## Tecnologias

* C#
* .NET 10
* `System.Text.Json`
* LINQ
* JSON para persistência de dados

## Estrutura do projeto

```text
desafio-dev/
├── Data/
│   ├── vendas.json
│   └── estoque.json
│
├── Models/
│   ├── Venda.cs
│   ├── VendasData.cs
│   ├── ResultadoComissao.cs
│   ├── Produto.cs
│   ├── EstoqueData.cs
│   ├── MovimentacaoEstoque.cs
│   └── ResultadoJuros.cs
│
├── Services/
│   ├── ComissaoService.cs
│   ├── EstoqueService.cs
│   └── JurosService.cs
│
├── Program.cs
└── README.md
```

## Como executar

É necessário ter o **.NET SDK 10** instalado.

Clone o projeto e entre na pasta:

```bash
git clone <URL_DO_REPOSITORIO>
cd desafio-dev
```

Compile o projeto:

```bash
dotnet build
```

Execute:

```bash
dotnet run
```

## Menu principal

Ao executar a aplicação, será apresentado o seguinte menu:

```text
========================================
             DESAFIO DEV
========================================
1 - Calcular comissões
2 - Movimentar estoque
3 - Calcular juros
0 - Sair
========================================
```

---

# 1. Cálculo de Comissões

O primeiro exercício lê as vendas a partir do arquivo:

```text
Data/vendas.json
```

Cada venda possui um vendedor e um valor.

As regras de comissão são:

| Valor da venda                 | Comissão |
| ------------------------------ | -------: |
| Abaixo de R$ 100               |       0% |
| De R$ 100 até abaixo de R$ 500 |       1% |
| A partir de R$ 500             |       5% |

A aplicação agrupa as vendas por vendedor e apresenta:

* Total de vendas;
* Total de comissão.

### Exemplo

```text
========================================
       RELATÓRIO DE COMISSÕES
========================================

Vendedor: Ana Lima
Total em vendas: R$ 1.500,00
Total de comissão: R$ 75,00
```

A regra de comissão fica isolada em:

```text
Services/ComissaoService.cs
```

---

# 2. Controle de Estoque

O segundo exercício utiliza:

```text
Data/estoque.json
```

É possível realizar:

* Entrada de produtos;
* Saída de produtos;
* Consulta do estoque atual;
* Registro da movimentação;
* Persistência das alterações no arquivo JSON.

Cada movimentação possui:

* ID único;
* Código do produto;
* Tipo da movimentação;
* Quantidade;
* Descrição.

### Exemplo

Uma entrada:

```text
Produto: Caneta Azul
Estoque atual: 150

Quantidade: 50
Descrição: Compra de fornecedor
```

Atualiza o estoque para:

```text
Estoque atual: 200
```

E registra a movimentação no JSON:

```json
{
  "Id": 1,
  "CodigoProduto": 101,
  "Tipo": "Entrada",
  "Quantidade": 50,
  "Descricao": "Compra de fornecedor"
}
```

As movimentações são persistidas em:

```text
Data/estoque.json
```

Dessa forma, as alterações continuam disponíveis mesmo após o encerramento da aplicação.

O ID da movimentação é gerado com base no maior ID existente, evitando que os identificadores sejam reiniciados quando o programa for executado novamente.

A regra de negócio do estoque fica em:

```text
Services/EstoqueService.cs
```

---

# 3. Cálculo de Juros

O terceiro exercício recebe:

* Valor da dívida;
* Data de vencimento.

A aplicação verifica quantos dias se passaram desde o vencimento e aplica uma taxa de:

```text
2,5% ao dia
```

O resultado apresenta:

* Valor original;
* Dias de atraso;
* Percentual de juros;
* Valor dos juros;
* Valor total.

### Exemplo

Para uma dívida de:

```text
R$ 1.000,00
```

com:

```text
4 dias de atraso
```

o cálculo será:

```text
2,5% × 4 = 10%
```

Resultado:

```text
Valor original: R$ 1.000,00
Dias de atraso: 4
Juros: 10,00%
Valor dos juros: R$ 100,00
Valor total: R$ 1.100,00
```

A regra fica isolada em:

```text
Services/JurosService.cs
```

---

# Organização do código

O projeto utiliza uma separação simples entre os diferentes tipos de responsabilidade.

## Models

As classes em `Models` representam os dados utilizados pela aplicação.

Exemplos:

```text
Venda
Produto
MovimentacaoEstoque
ResultadoComissao
ResultadoJuros
```

## Services

As classes em `Services` concentram as regras de negócio.

Por exemplo:

```text
ComissaoService
EstoqueService
JurosService
```

Isso evita colocar toda a lógica diretamente no `Program.cs`.

## Program.cs

O `Program.cs` é responsável principalmente por:

* Exibir o menu;
* Ler informações do usuário;
* Validar entradas básicas;
* Chamar os services;
* Exibir os resultados.

---

# Persistência

O projeto utiliza arquivos JSON para armazenamento dos dados.

### Vendas

```text
Data/vendas.json
```

As vendas são carregadas durante o cálculo das comissões.

### Estoque

```text
Data/estoque.json
```

O arquivo contém tanto os produtos quanto o histórico das movimentações.

Exemplo:

```json
{
  "estoque": [
    {
      "codigoProduto": 101,
      "descricaoProduto": "Caneta Azul",
      "estoque": 180
    }
  ],
  "movimentacoes": [
    {
      "Id": 1,
      "CodigoProduto": 101,
      "Tipo": "Entrada",
      "Quantidade": 50,
      "Descricao": "Compra de fornecedor"
    },
    {
      "Id": 2,
      "CodigoProduto": 101,
      "Tipo": "Saída",
      "Quantidade": 20,
      "Descricao": "Venda para cliente"
    }
  ]
}
```

A serialização e desserialização são feitas utilizando `System.Text.Json`.

---

# Convenções de nomenclatura

**Observação:** o desafio foi desenvolvido com nomes de classes, métodos, propriedades e variáveis em **português**, acompanhando a linguagem utilizada no enunciado.

Por exemplo:

```csharp
public class MovimentacaoEstoque
{
    public int Id { get; set; }
    public int CodigoProduto { get; set; }
    public string Tipo { get; set; }
}
```

E:

```csharp
public static MovimentacaoEstoque RegistrarEntrada(...)
```

Em um projeto de produção direcionado a um time internacional, esses nomes poderiam ser escritos em inglês, por exemplo:

```csharp
public class StockMovement
{
    public int Id { get; set; }
    public int ProductCode { get; set; }
    public string Type { get; set; }
}
```

Da mesma forma:

```text
RegistrarEntrada
→ RegisterEntry

RegistrarSaida
→ RegisterExit

BuscarProduto
→ FindProduct

CalcularComissao
→ CalculateCommission

CalcularJuros
→ CalculateInterest
```

A decisão de manter a nomenclatura em português neste desafio foi principalmente para manter o código alinhado ao enunciado e facilitar a relação entre os requisitos e a implementação.

---

# Validações

A aplicação possui validações básicas para evitar operações inválidas.

### Estoque

* Produto inexistente;
* Quantidade inválida;
* Quantidade menor ou igual a zero;
* Descrição vazia;
* Saída maior que o estoque disponível.

### Juros

* Valor inválido;
* Valor menor ou igual a zero;
* Data de vencimento inválida.

### Vendas

* Arquivo inexistente;
* Nenhuma venda encontrada.

---

# Objetivo do projeto

O objetivo principal foi demonstrar conhecimentos de:

* C#;
* .NET;
* Programação orientada a objetos;
* LINQ;
* Manipulação de JSON;
* Separação de responsabilidades;
* Regras de negócio;
* Validação de entradas;
* Persistência de dados;
* Tratamento de cenários inválidos.

O projeto mantém uma arquitetura simples, adequada ao tamanho do desafio, evitando adicionar complexidade desnecessária.
