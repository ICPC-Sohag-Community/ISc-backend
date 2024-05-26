using System.Net;
using FluentValidation.Results;
using ISc.Shared.Extensions;

namespace ISc.Shared
{
    public class PaginatedRespnose<T> : Response
    {
        public PaginatedRespnose(List<T> data)
        {
            Data = data;
        }
        public PaginatedRespnose()
        {
            
        }
        public PaginatedRespnose(bool succeeded, List<T> data = default,
                               string message = null,
                               List<ValidationFailure> validationFailures = null, int count = 0,
                               HttpStatusCode httpStatusCode = HttpStatusCode.OK,
                               int pageNumber = 1, int pageSize = 10)
        {
            Data = data;
            CurrentPage = pageNumber;
            StatusCode = httpStatusCode;
            IsSuccess = succeeded;
            Message = message;
            Errors = validationFailures?.GetDictionary();
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public static PaginatedRespnose<T> Create(List<T> data, int count, int pageNumber, int pageSize)
        {
            return new PaginatedRespnose<T>(true, data, null, null, count, HttpStatusCode.OK, pageNumber, pageSize);
        }
    }
}
