namespace BaseArch.Application.Models.Responses
{
    public record ResponseModel<T>(T? Data, PaginationResponseModel? Pagination, bool Result);

    public record PaginationResponseModel(int PageNumber, int PageSize, int PageCount);
}
