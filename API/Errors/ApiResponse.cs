using System;

namespace API.Errors
{
    //we will utilize this response when we are returning an error response that we create inside our controller
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        //we also had extended the our ApiResponse to accommodate the extra field that we want to send down with a response which is stackTrace because Message isn't enough in case of Server Error in Development Mode
        
        //Every response is gonn ahave at least these two properties
        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path to the dark side. Errors lead to anger. Anger leads to hate. Hate leads to career change",
                _ => null
            };
        }

    }
}