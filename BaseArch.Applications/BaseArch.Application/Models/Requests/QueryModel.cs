namespace BaseArch.Application.Models.Requests
{
    /// <summary>
    /// Query model to filter data
    /// </summary>
    /// <param name="Search"><see cref="SearchQueryModel"/></param>
    /// <param name="Filters"><see cref="FilterQueryModel"/></param>
    /// <param name="Pagination"><see cref="PaginationQueryModel"/></param>
    /// <param name="Sort"><see cref="SortQueryModel"/></param>
    public record QueryModel(SearchQueryModel? Search, IEnumerable<FilterQueryModel>? Filters, PaginationQueryModel? Pagination, SortQueryModel? Sort);
}
