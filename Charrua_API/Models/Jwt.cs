using Charrua_API.Data;
using Charrua_API.Response;
using Charrua_API.Response.Usuario;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace Charrua_API.Models
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

        private readonly ContextBD contextBD;
        public Jwt(ContextBD contextBD)
        {
            this.contextBD = contextBD;
        }


        public async Task<object> validarToken(ClaimsIdentity identity)
        {
            
            try
            {
                RespCreateUser res = new RespCreateUser();
                if (identity.Claims.Count() == 0)
                {
                    
                    res.setError("Verificar Token", HttpStatusCode.NonAuthoritativeInformation);
                    return res;
                }
                var id = identity.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                Usuario usr = new Usuario();
                var ult = await contextBD.usuarios.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefaultAsync();
                if(ult == null)
                {
                    res.setError("Verificar Token", HttpStatusCode.NonAuthoritativeInformation);
                    return res;
                }
                res.UserName = ult.UserName;
                res.Email = ult.Email;
                return res;

            }
            catch(Exception e)
            {
                RespCreateUser res = new RespCreateUser();

                res.setError((res.Mensaje = e.Message), HttpStatusCode.BadRequest);
                return res;
            }
        }
    }
}
