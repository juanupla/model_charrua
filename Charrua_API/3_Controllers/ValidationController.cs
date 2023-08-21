using Charrua_API._0_business.JwtBusiness;

using Charrua_API.Response.JwtResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Charrua_API._3_Controllers
{
    [ApiController]
    [Route("/validate")]
    public class ValidationController : Controller
    {
        private readonly IMediator mediator;


        public ValidationController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost]
        [Route("/Jwt")]
        public async Task<JwtResponse> JwtValidate([FromBody] ClaimsIdentity identity) //ese endpont recibirá peticiones de otro microservicio para validar JWT
        {
            var result = await mediator.Send(new ValidateBearerBusiness.ValidateBearer_Business
            {
                claimsIdentity = identity
            });
            return result;
        }
    }
}
