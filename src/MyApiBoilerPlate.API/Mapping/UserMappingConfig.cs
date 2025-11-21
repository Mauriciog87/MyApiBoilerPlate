using Mapster;
using MyApiBoilerPlate.Application.Users.Common;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.API.Mapping;

public class UserMappingConfig : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<User, UserResponse>()
        .Map(dest => dest.UserId, src => src.UserId)
        .Map(dest => dest.Id, src => src.Id)
        .Map(dest => dest.FirstName, src => src.FirstName)
        .Map(dest => dest.LastName, src => src.LastName)
        .Map(dest => dest.Email, src => src.Email)
        .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
        .Map(dest => dest.DateOfBirth, src => src.DateOfBirth)
        .Map(dest => dest.IsActive, src => src.IsActive)
        .Map(dest => dest.CreatedAt, src => src.CreatedAt)
        .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);
  }
}
