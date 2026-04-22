namespace GoodHamburgerFront.Models;

public class ItemPedidoRequestDto
{
    public int Id { get; set; }
}

public class ItemPedidoResponseDto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public double Valor { get; set; }
}

public class PedidoRequestDto
{
    public List<ItemPedidoRequestDto>? Itens { get; set; }
}

public class PedidoResponseDto
{
    public Guid Id { get; set; }
}

public class PedidoGetResponseDto
{
    public Guid Id { get; set; }
    public List<ItemPedidoResponseDto>? Itens { get; set; }
    public bool ComDesconto { get; set; }
    public double ValorTotal { get; set; }
    public double ValorFinal { get; set; }
    public bool ValorComDesconto { get; set; }
    public bool Fechado { get; set; }
}

public class PedidoAnteriorItemDto
{
    public Guid Id { get; set; }
    public string DataHora { get; set; } = "";
    public int QuantidadeItens { get; set; }
    public double ValorFinal { get; set; }
}
