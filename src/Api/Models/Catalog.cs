namespace Api.Models;

public class Catalog
{
    public Guid StandId { get; set; }
    public required Stand Stand { get; set; }
    public Guid ProductId { get; set; }
    public required Product Product { get; set; }
    public bool InStock { get; set; }
    public decimal Price { get; set; }
}

