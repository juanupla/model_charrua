using System.Net;

namespace Charrua_API.Response
{
    public class ResponseBase
    {
        public bool Ok { get; set; } = true;
        public string Mensaje { get; set; }
        public HttpStatusCode HttpStatusCodeStatusCode { get; set; } = HttpStatusCode.OK;


        public void setError(string error, HttpStatusCode httpStatusCode)
        {
            this.Mensaje= error;
            this.HttpStatusCodeStatusCode= httpStatusCode;
            Ok = false;
        }
    }
}
