namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
            Details = details;
        }

        //this property is gonna refer to the Stack Trace
        public string Details { get; set; }

        //Next step is that we create some middleware so that we can handle exceptoins and use this particular class in the event that we get an exception 
    }
}