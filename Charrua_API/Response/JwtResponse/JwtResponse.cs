

using System.Net;

namespace Charrua_API.Response.JwtResponse
{
    public class JwtResponse
    {
        public bool success { get; set; } = true;
        public string message { get; set; } = "Exito";
        public Models.Usuario usr { get; set; } = new Models.Usuario();
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;

        public void setError(string error, HttpStatusCode httpStatusCode)
        {
            success = false;
            message = error;
            this.HttpStatusCode= httpStatusCode;
            
        }
    }
}
