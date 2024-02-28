using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Talabat.APis.Errors
{
    public class ApiResponse
    {
        // statuscode
        public int StatusCode { get; set; }

        //title
        public string? Message { get; set; }
        public ApiResponse(int _code, string? _message=null)
        {
            StatusCode = _code;
            Message = _message??GetDefaultResponseMessage(_code);
        }

        private string? GetDefaultResponseMessage(int code)
        {
            return code switch
            {
                400=>"A Bad Request You have make",
                401=> "Authoried ,you are  not ",
                404=>"Resource Not Found",
                500=> "Errors are the path to the dark side .Error let to angur",
                _=>null
            };
        }
    }
}
