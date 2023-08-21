using Charrua_API.Models;

namespace Charrua_API.Response.Usuario
{
    public class RespCreateUser:ResponseBase
    {
        public string Name { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
     
    }
}
