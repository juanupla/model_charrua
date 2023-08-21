using Charrua_API.Data;
using Charrua_API.Models;
using Charrua_API.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Charrua_API._0_business.UsuarioBusieness
{
    public class ConfirmBusiness
    {
        public class Confirm_Business : IRequest<ResponseBase>
        {
            public string Token { get; set; }
        }
        public class validation : AbstractValidator<Confirm_Business>
        {
            public validation()
            {
                RuleFor(x=>x.Token).NotEmpty().WithMessage("no puede faltar el codigo");
            }
        }
        public class manejador : IRequestHandler<Confirm_Business, ResponseBase>
        {
            private readonly IValidator<Confirm_Business> validator;
            private readonly ContextBD contextBD;
            public manejador(IValidator<Confirm_Business> validator, ContextBD contextBD)
            {
                this.validator = validator;
                this.contextBD = contextBD;
            }

            public async Task<ResponseBase> Handle(Confirm_Business request, CancellationToken cancellationToken)
            {
                ResponseBase result = new ResponseBase();
                var valid = validator.Validate(request);
                if(!valid.IsValid)
                {
                    result.setError("Error inesperado",HttpStatusCode.InternalServerError);
                    return result;
                }
                var user = await contextBD.usuarios.FirstOrDefaultAsync(x => x.Token == request.Token);
                if(user == null)
                {
                    result.setError("Usuario inexistente o token inválido", HttpStatusCode.InternalServerError);
                    return result;
                }

                user.Confirmado = true;
                

                contextBD.Update(user);
                await contextBD.SaveChangesAsync();

                result.Mensaje = "Usuario confirmado con éxito";
                return result;
            }
        }
    }
}
