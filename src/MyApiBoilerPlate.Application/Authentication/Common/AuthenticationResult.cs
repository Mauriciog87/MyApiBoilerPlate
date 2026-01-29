using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Application.Authentication.Common
{
    public record AuthenticationResult(User User, string Token);
}
