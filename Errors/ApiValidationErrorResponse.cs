namespace Talabat.APis.Errors
{
    public class ApiValidationErrorResponse:ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorResponse():base(400) // as validation error is a BadRequest
        {
            Errors = new List<string>();
        }
        // we need to chnage for the default confiuyrof the invaildmodelstateResone to nake a 
    }
}
