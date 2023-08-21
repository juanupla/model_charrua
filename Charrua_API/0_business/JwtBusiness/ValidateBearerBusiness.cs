using Charrua_API.Configurations.JsonWebToken;
using Charrua_API.Data;
using Charrua_API.Response.JwtResponse;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Net;
using System.Security.Claims;

namespace Charrua_API._0_business.JwtBusiness
{
    public class ValidateBearerBusiness
    {

        public class ValidateBearer_Business : IRequest <JwtResponse>
        {
             public ClaimsIdentity claimsIdentity { get; set; }
        }
        public class validacion : AbstractValidator<ValidateBearer_Business>
        {
            public validacion()
            {
                RuleFor(x => x.claimsIdentity).NotEmpty().WithMessage("claimsIdentity necesario");
            }
        }

        public class manejador : IRequestHandler<ValidateBearer_Business, JwtResponse>
        {
            private readonly ContextBD contextBD;
            private readonly IValidator<ValidateBearer_Business> validator;

            public manejador(ContextBD contextBD, IValidator<ValidateBearer_Business> validator)
            {
                this.contextBD = contextBD;
                this.validator = validator;
            }

            

            public async Task<JwtResponse> Handle(ValidateBearer_Business request, CancellationToken cancellationToken)
            {
                JwtResponse response = new JwtResponse();

                var valid = validator.Validate(request);
                if(!valid.IsValid)
                {
                    var error = String.Join(Environment.NewLine, valid.Errors);
                    response.setError(error, HttpStatusCode.BadRequest);
                    return response;
                }

                var rToken = Jwt.validarToken(request.claimsIdentity, contextBD);

                try
                {
                    if (Convert.ToUInt32(rToken.Result.ToString()) == 0)
                    {
                        response.setError(HttpStatusCode.Unauthorized.ToString(), HttpStatusCode.Unauthorized);
                        return response;
                    }
                }
                catch (Exception) { }

                if (!rToken.IsCompletedSuccessfully)
                {
                    response.setError("token inválido", HttpStatusCode.Unauthorized);
                    return response;
                }

                response.message = "Token válido";
                return response;


                
            }
        }
    }
}
