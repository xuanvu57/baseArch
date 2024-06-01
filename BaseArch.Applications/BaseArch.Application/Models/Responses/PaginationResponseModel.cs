namespace BaseArch.Application.Models.Responses
{
    /// <summary>
    /// Pagination model
    /// </summary>
    /// <param name="PageNumber">Current page number</param>
    /// <param name="PageSize">The maximum item in a page</param>
    /// <param name="PageCount">Total of page</param>
    public record PaginationResponseModel(int PageNumber, int PageSize, int PageCount);
}
