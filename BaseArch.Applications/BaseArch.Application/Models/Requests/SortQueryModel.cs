namespace BaseArch.Application.Models.Requests
{
    /// <summary>
    /// Sort model for sorting
    /// </summary>
    /// <param name="SortBy">Field will be sorted</param>
    /// <param name="SortOrder">Order of sorting; ASC (default) or DESC</param>
    public record SortQueryModel(string SortBy, string SortOrder);
}
