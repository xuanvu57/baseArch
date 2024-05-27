namespace BaseArch.Application.Models.Requests
{
    /// <summary>
    /// Search model for filtering from multi fields
    /// </summary>
    /// <param name="SearchText">Text to search</param>
    /// <param name="FieldNames">Fields will be searched</param>
    public record SearchQueryModel(string SearchText, IEnumerable<string> FieldNames);
}
