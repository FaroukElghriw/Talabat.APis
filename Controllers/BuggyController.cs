using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APis.Errors;
using Talabat.Repositry.Data;

namespace Talabat.APis.Controllers
{
   
    public class BuggyController : BaseApiController
    {
        private readonly StoreDbContext _storeDb;

        public BuggyController(StoreDbContext storeDb)
        {
            _storeDb = storeDb;
        }
        // in Api we havr  5 types of error  happen when the enduser(like forntend) try to conect with backeend
        // we want to make a standerd for the structure of the resposn of this error to  make enduse happer
        // struct object status , title
        // NotFound=>notfoundEndPoint =>404
        // Badrequest=> VailditionError=>400
        // SerevrError=>500
        [HttpGet("notfound")]
        public ActionResult GetNotFound()
        {
            var product = _storeDb.Products.Find(100);// we dont have a proj with 100 id
            if (product is null) return NotFound(new ApiResponse(404)); // for show the struc 
            // we make a customie for the sturcture of the response objrtc of error
            return Ok(product);
        }
        [HttpGet("badrequest")]
        public ActionResult GetBadREquest()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("badRrquest/{id}")]
        public ActionResult GetBadREquest(int id)
        {
            return BadRequest();
        }
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var product = _storeDb.Products.Find(100);
            var pto = product.ToString();
           return Ok(pto);
        }
    }
}
