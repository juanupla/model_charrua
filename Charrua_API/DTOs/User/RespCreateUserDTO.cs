using Charrua_API.Response;

namespace Charrua_API.DTOs.User
{
    public class RespCreateUserDTO:ResponseBase
    {
        public string Name { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string templatePath { get; set; }

    }
}
