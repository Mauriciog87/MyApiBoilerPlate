namespace MyApiBoilerPlate.Domain.Entities
{
  public sealed class User : BaseEntity
  {
    public int UserId { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public bool IsActive { get; private set; }

    public User() { }

    public User(string firstName, string lastName, string email, string phoneNumber, string passwordHash, DateTime dateOfBirth)
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
      PhoneNumber = phoneNumber;
      PasswordHash = passwordHash;
      DateOfBirth = dateOfBirth;
      IsActive = true;
    }

    public void Update(string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth, bool isActive)
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
      PhoneNumber = phoneNumber;
      DateOfBirth = dateOfBirth;
      IsActive = isActive;
      UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
      IsActive = false;
      UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Activate()
    {
      IsActive = true;
      UpdatedAt = DateTimeOffset.UtcNow;
    }
  }
}
