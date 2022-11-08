using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [Route("errors/{code}")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}

// [ApiExplorerSettings(IgnoreApi = true)] xq no me deja entrar swagger
//Fetch error
//response status is 500 /swagger/v1/swagger.json

/// An ActionResult that on execution will write an object to the response
/// using mechanisms provided by the host.