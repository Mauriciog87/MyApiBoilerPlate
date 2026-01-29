namespace MyApiBoilerPlate.Application.Common.Interfaces.Repositories
{
  public interface IDummyRepository
  {
    Task<IEnumerable<string>> GetDummyDataAsync(CancellationToken cancellationToken = default);
    Task<string> GetDummyDataByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddDummyDataAsync(string data, CancellationToken cancellationToken = default);
    Task UpdateDummyDataAsync(Guid id, string data, CancellationToken cancellationToken = default);
    Task DeleteDummyDataAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DummyDataExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetDummyDataByPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
  }
}
