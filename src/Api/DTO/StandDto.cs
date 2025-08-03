namespace Api.Dto;

public class StandDto
{
    public Guid StandId { get; set; }
    public string Name { get; set; } = default!;
    public List<ProductDto> Products{ get; set; } = new();
}
