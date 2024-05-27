namespace BaseArch.Domain.BaseArchModels.Requests
{
    /// <summary>
    /// Filter model for filtering from specific field
    /// </summary>
    /// <param name="SearchText">Text to search</param>
    /// <param name="FieldName">Field will be search</param>
    public record FilterQueryModel(string SearchText, string FieldName);
}
