using System.Net;

namespace BlogAPI.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode {  get; set; }
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
        public List<string> ErrorMessages { get; set; }


        public APIResponse() { 
            ErrorMessages = new List<string>();
        }

    }
}
