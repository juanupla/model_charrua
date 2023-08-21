using Charrua_API._0_business.UsuarioBusieness;
using Charrua_API.business.UsuarioBusieness;
using Charrua_API.commands.UsuarioCommand;
using Charrua_API.Models;
using Charrua_API.Response;
using Charrua_API.Response.Usuario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Charrua_API.Controllers
{
    [ApiController]
    [Route("/Users")]
    public class UsersController : Controller
    {
        private readonly IMediator mediator;
        public UsersController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("/CreateUser")] 
        public async Task<RespCreateUser> CreateUser([FromBody] NewUserCommand newUserCommand)
        {

            var result = await mediator.Send(new NewUserBusiness.NewUser_Business
            {
                Name= newUserCommand.Name,
                LastName = newUserCommand.LastName,
                Password =newUserCommand.Password,
                Email=newUserCommand.Email
            });
            return result;

        }
        [HttpPost]
        [Route("/Login")]
        public async Task<RespLogin> Login([FromBody] LoginCommand loginCommand)
        {
            var result = await mediator.Send(new LoginBusiness.Login_Business
            {
                Email = loginCommand.Email,
                Password=loginCommand.Password
            });
            return result;
        }
        [HttpGet("/confirm")]
        public async Task<IActionResult> ConfirmRegistration(string code)
        {
            var result = await mediator.Send(new ConfirmBusiness.Confirm_Business
            {
                Token = code
            });
            if(result.Ok == true)
            {
                return Redirect("https://google.com"); //aca debe ir la direccion de nuestra web; se podria pensar un front para una validacion correcta
            }
            return Redirect("https://chat.openai.com/"); //aca se podria pensar lo mismo para un mensaje inválido
        }

        [HttpDelete]
        [Route("/DeleteUser/{id}")]
        [Authorize]
        public async Task<RespCreateUser> deleteUser(int id)
        {
            var ident = HttpContext.User.Identity as ClaimsIdentity;
            var resul = await mediator.Send(new DeleteUsuarioBusiness.DeleteUsuario_Busieness
            {
                Id = id,
                identity = ident
            });
            return resul; 
        }

    }
}
