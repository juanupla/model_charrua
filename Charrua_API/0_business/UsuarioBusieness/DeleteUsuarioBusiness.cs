using Charrua_API.Configurations.JsonWebToken;
using Charrua_API.Data;
using Charrua_API.Models;
using Charrua_API.Response.JwtResponse;
using Charrua_API.Response.Usuario;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;

namespace Charrua_API.business.UsuarioBusieness
{
    public class DeleteUsuarioBusiness
    {


        public class DeleteUsuario_Busieness: IRequest<RespCreateUser>
        {
            public ClaimsIdentity identity { get; set; }
            public int Id { get; set; }
        }
        public class validacion : AbstractValidator<DeleteUsuario_Busieness>
        {
            public validacion()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Indique el usuario a eliminar");
                RuleFor(x => x.identity).NotEmpty().WithMessage("no autorizado");
            }
        }
        public class manejador : IRequestHandler<DeleteUsuario_Busieness, RespCreateUser>
        {

            private readonly ContextBD contextBD;
            private readonly IValidator<DeleteUsuario_Busieness> validator;

            public manejador(ContextBD contextBD, IValidator<DeleteUsuario_Busieness> validator)
            {
                this.contextBD = contextBD;
                this.validator = validator;
            }

            public async Task<RespCreateUser> Handle(DeleteUsuario_Busieness request, CancellationToken cancellationToken)
            {
                RespCreateUser result = new RespCreateUser();
                var vali = validator.Validate(request);
                if(!vali.IsValid)
                {
                    var estado = String.Join(Environment.NewLine, vali.Errors);
                    result.setError(estado, HttpStatusCode.BadRequest);
                    return result;
                }

                //------------------
                var rToken = Jwt.validarToken(request.identity,contextBD);

                try 
                { 
                    if (Convert.ToUInt32(rToken.Result.ToString()) == 0) 
                    { 
                        result.setError(HttpStatusCode.Unauthorized.ToString(),HttpStatusCode.Unauthorized); 
                        return result; 
                    } 
                }
                catch(Exception) { }

                if (!rToken.IsCompletedSuccessfully)
                {
                    result.setError("token inválido", HttpStatusCode.Unauthorized);
                    return result;
                }

                JwtResponse user = rToken.Result;     //----------------- verifica el token y devuelve un user x si es ncesario acceder a autorizaciones
                

                //---------------------------------------
                if (user.usr.Authorization == "admin")
                {
                    
                    var usrr = await contextBD.usuarios.FirstOrDefaultAsync(x => x.Id == request.Id);

                    Usuario usuario = new Usuario();
                    usuario.Name = usrr.Name;
                    usuario.LastName = usrr.LastName;
                    usuario.Email = usrr.Email;
                    usuario.Authorization = usrr.Authorization;
                    usuario.Password = usrr.Password;

                    contextBD.usuarios.Remove(usrr);
                    contextBD.SaveChanges();

                    return result;
                }

                result.setError("No posee permisos para realizar esta acción", HttpStatusCode.NonAuthoritativeInformation);
                return result;


            }
        }
    }
}
