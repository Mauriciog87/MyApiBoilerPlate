namespace MyApiBoilerPlate.Domain.Entities
{
  public class BaseEntity
  {
    public Guid Id { get; internal set; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; internal set; }
    public DateTimeOffset UpdatedAt { get; internal set; }
  }
}
