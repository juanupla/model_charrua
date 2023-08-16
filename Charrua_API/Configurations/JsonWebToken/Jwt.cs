using Charrua_API.Data;
using Charrua_API.Response;
using Charrua_API.Response.JwtResponse;
using Charrua_API.Response.Usuario;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Charrua_API.Configurations.JsonWebToken
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }



        public static async Task<JwtResponse> validarToken(ClaimsIdentity identity, ContextBD contextBD)
        {

            try
            {
                JwtResponse response = new JwtResponse();

                if (identity.Claims.Count() == 0)
                {

                    response.setError("Verificar Token", HttpStatusCode.NonAuthoritativeInformation);
                    return response;
                }
                var id = identity.Claims.FirstOrDefault(x => x.Type == "Id").Value;

                var ult = await contextBD.usuarios.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefaultAsync();
                if (ult == null)
                {
                    response.setError("Verificar Token", HttpStatusCode.NonAuthoritativeInformation);
                    return response;
                }


                response.usr.UserName = ult.UserName;
                response.usr.Authorization = ult.Authorization;



                return response;

            }
            catch (Exception e)
            {
                JwtResponse response = new JwtResponse();
                response.setError(e.Message, HttpStatusCode.Unauthorized);
                return response;
            }
        }
    }
}
