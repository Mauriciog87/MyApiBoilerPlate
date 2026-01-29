using MyApiBoilerPlate.Application.Common.Interfaces.Authentication;

namespace MyApiBoilerPlate.Infrastructure.Authentication
{
  public class PasswordHasher : IPasswordHasher
  {
    public string HashPassword(string password)
    {
      return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
      return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
  }
}
