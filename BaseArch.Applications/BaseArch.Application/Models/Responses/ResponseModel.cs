using System.Text.Json.Serialization;

namespace BaseArch.Application.Models.Responses
{
    /// <summary>
    /// The standard response model
    /// </summary>
    /// <typeparam name="TResponse">Type of the data</typeparam>
    /// <param name="Data">Data</param>
    /// <param name="Pagination">Pagination data <see cref="PaginationResponseModel"/></param>
    /// <param name="Result">Result from process</param>
    /// <param name="Message">Message from the process</param>
    public record ResponseModel<TResponse>(TResponse? Data, PaginationResponseModel? Pagination, bool Result, string Message)
    {
        /// <summary>
        /// Result from the process
        /// </summary>
        public bool Result { get; init; } = Result;

        /// <summary>
        /// Data
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TResponse? Data { get; init; } = Data;

        /// <summary>
        /// Pagination
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public PaginationResponseModel? Pagination { get; init; } = Pagination;

        /// <summary>
        /// Message from the process
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Message { get; init; } = Message;
    }
}
