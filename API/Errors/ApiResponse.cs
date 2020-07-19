using System;

namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            // TODO: Tricky. Other codes.
            return statusCode switch
            {
                400 => "A bad request",
                401 => "Not Authorized",
                404 => "Resourse was not found",
                500 => "Server-side error",
                _ => null
            };
        }
    }
}