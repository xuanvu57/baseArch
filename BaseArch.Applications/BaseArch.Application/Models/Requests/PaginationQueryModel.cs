namespace BaseArch.Application.Models.Requests
{
    /// <summary>
    /// Paging model for pagination
    /// </summary>
    /// <param name="PageNumber">Page number</param>
    /// <param name="PageSize">Maximum number of returned items</param>
    public record PaginationQueryModel(int PageNumber, int PageSize);
}
