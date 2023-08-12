﻿using Charrua_API.Data;
using Charrua_API.Models;
using Charrua_API.Response.Usuario;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Charrua_API.business.UsuarioBusieness
{
    public class LoginBusiness
    {
        public class Login_Business : IRequest<RespLogin>
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class validacion : AbstractValidator<Login_Business>
        {
            public validacion()
            {
                RuleFor(x => x.UserName).NotEmpty().WithMessage("Email o Password incorrectos");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Email o Password incorrectos");
            }
        }
        public class manejador : IRequestHandler<Login_Business, RespLogin>
        {

            private readonly IValidator<Login_Business> validator;
            private readonly ContextBD contextBD;
            public IConfiguration configuration;
            public manejador(IValidator<Login_Business> validator, ContextBD contextBD, IConfiguration configuration)
            {
                this.validator = validator;
                this.contextBD = contextBD;
                this.configuration = configuration;
            }

            public async Task<RespLogin> Handle(Login_Business request, CancellationToken cancellationToken)
            {
                RespLogin res = new RespLogin();

                var valida = validator.Validate(request);
                if(!valida.IsValid)
                {
                    var men = String.Join(Environment.NewLine, valida.Errors);
                    res.setError(men,HttpStatusCode.BadRequest);
                    return res;
                }

                var bd = await contextBD.usuarios.Where(x => x.UserName == request.UserName && x.Password == request.Password).FirstOrDefaultAsync();
                if(bd == null)
                {
                    res.setError("Usuario o Password incorrecto", HttpStatusCode.BadRequest);
                    return res;
                }

                var jwt = configuration.GetSection("Jwt").Get<Jwt>();
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                    new Claim("Id",bd.Id.ToString()),
                    new Claim("UserName", bd.UserName),
                    new Claim("Password",bd.Password),
                    new Claim("Email",bd.Email),
                    new Claim("Authorization", bd.Authorization),
                    
                };
                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
                var signIn = new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(jwt.Issuer,jwt.Audience,claims,expires: DateTime.Now.AddMinutes(60),signingCredentials:signIn);

                res.Token = new JwtSecurityTokenHandler().WriteToken(token);

                return res;

            }
        }



    }
}
