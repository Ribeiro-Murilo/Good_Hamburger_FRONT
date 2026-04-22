namespace GoodHamburgerFront.Models;

public class TipoItemDto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
}

public class MenuItemDto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public double Preco { get; set; }
    public TipoItemDto? Tipo { get; set; }
}
