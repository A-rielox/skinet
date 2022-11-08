using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}

// [ApiExplorerSettings(IgnoreApi = true)] xq no me deja entrar swagger
// Fetch error
// response status is 500 /swagger/v1/swagger.json

// en el Startup.cs
// para redireccionar los errores con mi controller de errores, p' el caso en que no existe la ruta
// app.UseStatusCodePagesWithReExecute("/errors/{0}");


/// An ActionResult that on execution will write an object to the response
/// using mechanisms provided by the host.