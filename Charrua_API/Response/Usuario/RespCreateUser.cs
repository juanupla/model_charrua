using Charrua_API.Models;

namespace Charrua_API.Response.Usuario
{
    public class RespCreateUser:ResponseBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
