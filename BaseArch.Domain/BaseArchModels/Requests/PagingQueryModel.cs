namespace BaseArch.Domain.BaseArchModels.Requests
{
    /// <summary>
    /// Paging model for pagination
    /// </summary>
    /// <param name="PageNumber">Page number</param>
    /// <param name="PageSize">Maximum number of returned items</param>
    public record PagingQueryModel(int PageNumber, int PageSize);
}
