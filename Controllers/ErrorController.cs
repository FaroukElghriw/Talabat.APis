using Microsoft.AspNetCore.Mvc;
using Talabat.APis.Errors;

namespace Talabat.APis.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController:ControllerBase
    {
        // this is a EndPoint that will be Exut or Redirect
        // we shoud use a middlewaer to do it 
        public ActionResult Errors(int code)
        {
            return NotFound(new ApiResponse(code));
        }
    }
}
