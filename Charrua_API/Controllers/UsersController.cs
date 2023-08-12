using Charrua_API.business.UsuarioBusieness;
using Charrua_API.commands;
using Charrua_API.Response.Usuario;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    }
}
