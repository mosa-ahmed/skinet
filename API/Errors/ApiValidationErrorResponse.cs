using System.Collections.Generic;

namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }
        
        //[ApiController] if it encounters what it sees as a validation error, then it adds the error to sth called ModelState that sends error response to our API server
        //And we will override the behavior of this [ApiController], and we actually do this inside our Startup class so that we configure this particular attribute
        public IEnumerable<string> Errors { get; set; }
    }
}