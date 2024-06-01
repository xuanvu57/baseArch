namespace BaseArch.Application.Models.Responses
{
    public static class Responses
    {
        public static ResponseModel<TResponse> From<TResponse>(TResponse data, bool result = true, string message = "")
        {
            return new ResponseModel<TResponse>(data, null, result, message);
        }

        public static ResponseModel<TResponse> From<TResponse>(TResponse data, PaginationResponseModel pagination, bool result = true, string message = "")
        {
            return new ResponseModel<TResponse>(data, pagination, result, message);
        }

        public static ResponseModel<TResponse> From<TResponse>(TResponse data, int pageNumber, int pageSize, int pageCount, bool result = true, string message = "")
        {
            var pagination = new PaginationResponseModel(pageNumber, pageSize, pageCount);

            return new ResponseModel<TResponse>(data, pagination, result, message);
        }
    }
}
