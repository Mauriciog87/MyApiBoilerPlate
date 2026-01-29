namespace MyApiBoilerPlate.Application.Common.Constants;

public static class ValidationConstants
{
  public static class User
  {
    public const int FirstNameMaxLength = 50;
    public const int LastNameMaxLength = 50;
    public const int EmailMaxLength = 100;
    public const string PhoneNumberPattern = @"^\+?[1-9]\d{1,14}$";
  }
}

public static class PaginationConstants
{
  public const int DefaultPageNumber = 1;
  public const int DefaultPageSize = 10;
  public const int MaxPageSize = 100;
  public const int MinPageSize = 1;
  public const int MinPageNumber = 1;
  public const int SortByMaxLength = 50;
}
