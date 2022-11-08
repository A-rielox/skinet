using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/buggy/notfound
        [HttpGet("Notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Products.Find(234);

            if(thing == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok();
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/buggy/servererror
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            //                       p' NullReferenceException

            var thing = _context.Products.Find(234);
            // thing es null y al ejecutarle un metodo va a generar una excepcion
            var thingToReturn = thing.ToString();

            return Ok();

        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/buggy/badrequest
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        ////////////////////////////////////////
        ///////////////////////////////////////////
        /// GET: api/buggy/badrequest/{id}
        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            //                          validation errors
            // badReq x mala validacion, espera int y le paso un string
            return BadRequest();
        }
    }
}
