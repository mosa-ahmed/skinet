using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //We will get the request that comes into our API and pass it to this particular controller and we do this by MiddleWare Component into StartUp class
    //we override the route from BaseApiController and changed it to [Route("errors/{code}")]
    [Route("errors/{code}")]
    //we don't want to be explixit about this method. we want this to handle any type of Http method, we did't add [httpget] or [httppost] or whatever
    //we also tell swagger not to bother with this particular controller because it showed us this error message : Ambiguous HTTP method for action - API.Controllers.ErrorController.Error (API). Actions require an explicit HttpMethod binding for Swagger/OpenAPI 3.0
    //we don't want to this as an endpoint anyway that our client would consume so we add this attribute : [ApiExplorerSettings(IgnoreApi = true)] and this now will be ignored by swagger. This problem only appeared with swagger that it's looking for explicit HttpMethod
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}