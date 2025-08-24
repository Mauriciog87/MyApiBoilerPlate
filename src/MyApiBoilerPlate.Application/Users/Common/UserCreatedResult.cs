namespace MyApiBoilerPlate.Application.Users.Common
{
    public sealed record UserCreatedResult(int UserId, Guid Id, string Email);
}