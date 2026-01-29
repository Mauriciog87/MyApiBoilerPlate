namespace MyApiBoilerPlate.Domain.Entities
{
  public class BaseEntity
  {
    public DateTimeOffset CreatedAt { get; internal set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; internal set; } = DateTimeOffset.UtcNow;
  }
}
