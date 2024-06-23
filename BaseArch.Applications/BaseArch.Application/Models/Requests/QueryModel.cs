namespace BaseArch.Application.Models.Requests
{
    /// <summary>
    /// Query model to filter data
    /// </summary>
    public record QueryModel()
    {
        /// <summary>
        /// <see cref="SearchQueryModel"/>
        /// </summary>
        public SearchQueryModel? Search { get; init; }

        /// <summary>
        /// <see cref="FilterQueryModel"/>
        /// </summary>
        public IEnumerable<FilterQueryModel>? Filters { get; init; }

        /// <summary>
        /// <see cref="PaginationQueryModel"/>
        /// </summary>
        public PaginationQueryModel? Pagination { get; init; }

        /// <summary>
        /// <see cref="SortQueryModel"/>
        /// </summary>
        public SortQueryModel? Sort { get; init; }
    }
}
