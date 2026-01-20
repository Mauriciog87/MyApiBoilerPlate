using Microsoft.Extensions.Logging;
using MyApiBoilerPlate.Application.Common.Interfaces.Repositories;
using MyApiBoilerPlate.Application.Common.Models;
using MyApiBoilerPlate.Domain.Entities;

namespace MyApiBoilerPlate.Infrastructure.Repositories
{
  /// <summary>
  /// Decorator for IUserRepository that adds structured logging.
  /// Logs stored procedure execution details for observability and debugging.
  /// </summary>
  public sealed class LoggingUserRepository(
    IUserRepository innerRepository,
    ILogger<LoggingUserRepository> logger) : IUserRepository
  {
    public async Task<User> CreateUser(User user, CancellationToken cancellationToken)
    {
      logger.LogInformation("[sp_InsertUser] Executing - Email: {Email}", user.Email);
      var result = await innerRepository.CreateUser(user, cancellationToken);
      logger.LogInformation("[sp_InsertUser] Completed - UserId: {UserId}", result.UserId);
      return result;
    }

    public async Task UpdateUser(User user, CancellationToken cancellationToken)
    {
      logger.LogInformation("[sp_UpdateUser] Executing - UserId: {UserId}", user.UserId);
      await innerRepository.UpdateUser(user, cancellationToken);
      logger.LogInformation("[sp_UpdateUser] Completed");
    }

    public async Task DeleteUser(int userId, CancellationToken cancellationToken)
    {
      logger.LogInformation("[sp_DeleteUser] Executing - UserId: {UserId}", userId);
      await innerRepository.DeleteUser(userId, cancellationToken);
      logger.LogInformation("[sp_DeleteUser] Completed");
    }

    public async Task<User?> GetUserById(int userId, CancellationToken cancellationToken)
    {
      logger.LogInformation("[sp_GetUserById] Executing - UserId: {UserId}", userId);
      var result = await innerRepository.GetUserById(userId, cancellationToken);
      logger.LogInformation("[sp_GetUserById] Completed - Found: {Found}", result is not null);
      return result;
    }

    public async Task<PagedResult<User>> GetAllUsers(
      int pageNumber,
      int pageSize,
      string? sortBy,
      bool sortDescending,
      CancellationToken cancellationToken)
    {
      logger.LogInformation(
        "[sp_GetAllUsersPaginated] Executing - Page: {PageNumber}/{PageSize}",
        pageNumber, pageSize);
      
      var result = await innerRepository.GetAllUsers(pageNumber, pageSize, sortBy, sortDescending, cancellationToken);
      
      logger.LogInformation(
        "[sp_GetAllUsersPaginated] Completed - TotalRecords: {TotalRecords}",
        result.TotalRecords);
      
      return result;
    }

    public async Task<bool> CheckIfUserExists(string email, CancellationToken cancellationToken)
    {
      logger.LogInformation("[sp_CheckEmailExists] Executing - Email: {Email}", email);
      var result = await innerRepository.CheckIfUserExists(email, cancellationToken);
      logger.LogInformation("[sp_CheckEmailExists] Completed - Exists: {Exists}", result);
      return result;
    }
  }
}
