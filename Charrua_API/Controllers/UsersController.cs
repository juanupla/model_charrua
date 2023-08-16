using Charrua_API.business.UsuarioBusieness;
using Charrua_API.commands.UsuarioCommand;
using Charrua_API.Models;
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
                Username= newUserCommand.Username,
                Password=newUserCommand.Password,
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
                UserName = loginCommand.UserName,
                Password=loginCommand.Password
            });
            return result;
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
