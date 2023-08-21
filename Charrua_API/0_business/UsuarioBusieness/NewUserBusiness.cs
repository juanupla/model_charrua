using Charrua_API.Configurations.Encrypting_H256;
using Charrua_API.Data;
using Charrua_API.Models;
using Charrua_API.Response.Usuario;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Charrua_API._1_Services;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Charrua_API.business.UsuarioBusieness
{
    public class NewUserBusiness
    {
        public class NewUser_Business : IRequest<RespCreateUser>
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }

        }
        public class validacion : AbstractValidator<NewUser_Business>
        {
            public validacion()
            {
                RuleFor(x => x.Name).NotEmpty().WithMessage("Name needed");
                RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName needed");
                RuleFor(x => x.Password).NotEmpty().WithMessage("password needed");
                RuleFor(x => x.Email).NotEmpty().WithMessage("email needed");
            }
        }
        public class manejador : IRequestHandler<NewUser_Business, RespCreateUser>
        {
            private readonly ContextBD contextBD;
            private readonly IValidator<NewUser_Business> validator;
            public  EmailService _emailService;


            public manejador(ContextBD contextBD, IValidator<NewUser_Business> validator)
            {
                this.contextBD = contextBD;
                this.validator = validator;
                
                this._emailService = null;
                
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

                var userExistente = await contextBD.usuarios.Where(x => (x.Name == request.Name && x.LastName == request.LastName) ||
                x.Email == request.Email).ToListAsync();

                if(userExistente.Any())
                {
                    string error = "Email o Usuario existente";
                    result.setError(error, HttpStatusCode.BadRequest);
                    return result;
                }


                Usuario user = new Usuario();
                user.Name = request.Name;
                user.LastName = request.LastName;
                user.Password = EncryptingH256.getInstance().ConvertirSHA256(request.Password); 
                user.Email = request.Email;
                user.Authorization = "user";
                user.Token = EncryptingH256.getInstance().GenerarToken(); //genero token por cada usuario para poder usarlos en emails para confirmaciones
                user.Confirmado = false;

                await contextBD.usuarios.AddAsync(user);
                await contextBD.SaveChangesAsync();

                //var confirmationCode = Guid.NewGuid().ToString(); // Código de confirmación único

                //var smtp = new SmtpClient();
                //smtp.Connect("smpt.live.com.ar", 587, SecureSocketOptions.StartTls);
                //smtp.Authenticate("juan_ce@live.com.ar", "aads");

                _emailService = new EmailServiceImpl("smtp.gmail.com",587, "juan.cremona@gmail.com", "qtfjlphkbjjepwvu");
                await _emailService.SendConfirmationEmail(user.Email, user.Token);


                result.Name = user.Name;
                result.LastName = user.LastName;
                result.Email = user.Email;
                result.Mensaje = $"Tu usuario ha sido creado exitosamente. Hemos enviado un email a la casilla de correos {user.Email} para verificar tu cuenta.";
               

                return result;

            }
        }



    }
}
