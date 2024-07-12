using System.Net;
using FluentValidation.Results;
using ISc.Shared.Extensions;
using Microsoft.AspNetCore.Identity;

namespace ISc.Shared
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";
        public object? Data { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }

        public Response(HttpStatusCode statusCode = HttpStatusCode.OK) => StatusCode = statusCode;

        #region Success Methods
        public static Task<Response> SuccessAsync(object data, string message)
        {
            Response response = new Response()
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };

            return Task.FromResult(response);
        }
        public static Task<Response> SuccessAsync()
        {
            Response response = new Response()
            {
                IsSuccess = true
            };

            return Task.FromResult(response);
        }
        public static Task<Response> SuccessAsync(object data)
        {
            Response response = new Response()
            {
                IsSuccess = true,
                Data = data
            };

            return Task.FromResult(response);
        }
        public static Task<Response> SuccessAsync(string message)
        {
            Response response = new Response()
            {
                IsSuccess = true,
                Message = message
            };

            return Task.FromResult(response);
        }
        #endregion
        #region Failure Methods
        public static Task<Response> FailureAsync(object data, string message)
        {
            Response response = new Response()
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = message,
                Data = data
            };

            return Task.FromResult(response);
        }
        public static Task<Response> FailureAsync(string message)
        {
            Response response = new Response()
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = message
            };

            return Task.FromResult(response);
        }
        public static Task<Response> FailureAsync(string message, HttpStatusCode statusCode)
        {
            Response response = new Response()
            {
                IsSuccess = false,
                Message = message,
                StatusCode = statusCode
            };

            return Task.FromResult(response);
        }
        public static Task<Response> ValidationFailureAsync(List<ValidationFailure> validationFailures, HttpStatusCode statusCode)
        {
            Dictionary<string, List<string>> errors = validationFailures.GetDictionary();

            var response = new Response()
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Errors = errors
            };

            return Task.FromResult(response);
        }

        public static Task<Response> ValidationFailureAsync(List<IdentityError> identityErrors, HttpStatusCode httpStatusCode)
        {
            Dictionary<string, List<string>> errors = identityErrors.GetDictionary();

            var response = new Response()
            {
                IsSuccess = false,
                StatusCode = httpStatusCode,
                Errors = errors
            };

            return Task.FromResult(response);
        }

		public static Task<PaginatedRespnose<T>> FailureAsync<T>(string v, HttpStatusCode unauthorized)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
