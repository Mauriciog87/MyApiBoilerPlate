namespace MyApiBoilerPlate.Application.Common.Models;

public sealed class PagedResult<T>
{
  public IEnumerable<T> Data { get; init; } = [];
  public int PageSize { get; init; }
  public int PageNumber { get; init; }
  public int TotalRecords { get; init; }
  public int TotalPages { get; init; }
  public bool HasPreviousPage { get; init; }
  public bool HasNextPage { get; init; }
  public bool OrderDescending { get; init; }
  public string? OrderBy { get; init; }

  public PagedResult(
      IEnumerable<T> data,
      int pageSize,
      int pageNumber,
      int totalRecords,
      bool orderDescending = false,
      string? orderBy = null)
  {
    Data = data;
    PageSize = pageSize;
    PageNumber = pageNumber;
    TotalRecords = totalRecords;
    TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    HasPreviousPage = pageNumber > 1;
    HasNextPage = pageNumber < TotalPages;
    OrderDescending = orderDescending;
    OrderBy = orderBy;
  }
}
