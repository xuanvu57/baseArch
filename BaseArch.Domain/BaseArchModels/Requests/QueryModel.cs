namespace BaseArch.Domain.BaseArchModels.Requests
{
    /// <summary>
    /// Query model to filter data
    /// </summary>
    /// <param name="SearchQueryModel"><see cref="SearchQueryModel"/></param>
    /// <param name="FilterQueryModel"><see cref="FilterQueryModel"/></param>
    /// <param name="PagingQueryModel"><see cref="PagingQueryModel"/></param>
    /// <param name="SortQueryModel"><see cref="SortQueryModel"/></param>
    public record QueryModel(SearchQueryModel SearchQueryModel, IEnumerable<FilterQueryModel> FilterQueryModel, PagingQueryModel? PagingQueryModel, SortQueryModel? SortQueryModel);
}
