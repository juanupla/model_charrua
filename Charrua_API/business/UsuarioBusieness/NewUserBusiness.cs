using Charrua_API.Configurations.Encrypting_H256;
using Charrua_API.Data;
using Charrua_API.Models;
using Charrua_API.Response.Usuario;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Charrua_API.business.UsuarioBusieness
{
    public class NewUserBusiness
    {
        public class NewUser_Business : IRequest<RespCreateUser>
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }

        }
        public class validacion : AbstractValidator<NewUser_Business>
        {
            public validacion()
            {
                RuleFor(x => x.Username).NotEmpty().WithMessage("userName needed");
                RuleFor(x => x.Password).NotEmpty().WithMessage("password needed");
                RuleFor(x => x.Email).NotEmpty().WithMessage("email needed");
            }
        }
        public class manejador : IRequestHandler<NewUser_Business, RespCreateUser>
        {
            private readonly ContextBD contextBD;
            private readonly IValidator<NewUser_Business> validator;
            public manejador(ContextBD contextBD, IValidator<NewUser_Business> validator)
            {
                this.contextBD = contextBD;
                this.validator = validator;
            }


            public async Task<RespCreateUser> Handle(NewUser_Business request, CancellationToken cancellationToken)
            {
                RespCreateUser result = new RespCreateUser();

                var valida = validator.Validate(request);
                if(!valida.IsValid)
                {
                    var error = String.Join(Environment.NewLine, valida.Errors);
                    result.setError(error, HttpStatusCode.BadRequest);
                    return result;
                }

                var userExistente = await contextBD.usuarios.Where(x => x.UserName == request.Username || x.Email == request.Email).ToListAsync();
                if(userExistente.Any())
                {
                    string error = "Email o Usuario existente";
                    result.setError(error, HttpStatusCode.BadRequest);
                    return result;
                }

                Usuario user = new Usuario();
                user.UserName = request.Username; 
                user.Password = EncryptingH256.getInstance().ConvertirSHA256(request.Password);
                user.Email = request.Email;
                user.Authorization = "user";

                await contextBD.usuarios.AddAsync(user);
                await contextBD.SaveChangesAsync();

                result.UserName = user.UserName;
                result.Password = user.Password;
                result.Email = user.Email;

                return result;

            }
        }



    }
}
