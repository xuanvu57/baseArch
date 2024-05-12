namespace BaseArch.Domain.Models
{
    /// <summary>
    /// Query model to filter data
    /// </summary>
    /// <param name="SearchQueryModel"><see cref="SearchQueryModel"/></param>
    /// <param name="FilterQueryModel"><see cref="FilterQueryModel"/></param>
    /// <param name="PagingQueryModel"><see cref="PagingQueryModel"/></param>
    /// <param name="SortQueryModel"><see cref="SortQueryModel"/></param>
    public record QueryModel(SearchQueryModel SearchQueryModel, IEnumerable<FilterQueryModel> FilterQueryModel, PagingQueryModel? PagingQueryModel, SortQueryModel? SortQueryModel);

    /// <summary>
    /// Search model for filtering from multi fields
    /// </summary>
    /// <param name="SearchText">Text to search</param>
    /// <param name="FieldNames">Fields will be searched</param>
    public record SearchQueryModel(string SearchText, IEnumerable<string> FieldNames);

    /// <summary>
    /// Filter model for filtering from specific field
    /// </summary>
    /// <param name="SearchText">Text to search</param>
    /// <param name="FieldName">Field will be search</param>
    public record FilterQueryModel(string SearchText, string FieldName);

    /// <summary>
    /// Paging model for pagination
    /// </summary>
    /// <param name="PageNumber">Page number</param>
    /// <param name="PageSize">Maximum number of returned items</param>
    public record PagingQueryModel(int PageNumber, int PageSize);

    /// <summary>
    /// Sort model for sorting
    /// </summary>
    /// <param name="SortBy">Field will be sorted</param>
    /// <param name="SortOrder">Order of sorting; ASC (default) or DESC</param>
    public record SortQueryModel(string SortBy, string SortOrder);
}
