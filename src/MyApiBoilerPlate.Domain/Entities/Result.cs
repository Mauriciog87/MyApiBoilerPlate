namespace MyApiBoilerPlate.Domain.Entities
{
  public class Result<T> where T : class
  {
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public bool OrderDescending { get; set; }
    public string? OrderBy { get; set; }

    public Result(
        T data,
        int pageSize,
        int pageNumber,
        int totalRecords,
        bool orderDescending,
        string? orderBy)
    {
      IsSuccess = true;
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
}