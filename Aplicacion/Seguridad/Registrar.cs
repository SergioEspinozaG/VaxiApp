using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using FluentValidation;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta  : IRequest<UsuarioData>{

            public string Nombre { get; set; }

            public string Apellidos { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string UserName { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>{
            public EjecutaValidador()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>{

            private readonly CursosOnlineContext Context;

            private readonly UserManager<Usuario> UserManager;

            private readonly IJwtGenerador JwtGenerador;

            public Manejador(CursosOnlineContext context, UserManager<Usuario> usuarioManajer, IJwtGenerador jwtGenerador)
            {
                Context = context;
                UserManager = usuarioManajer;
                JwtGenerador = jwtGenerador;
            }

            public async  Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultEmail = await Context.Users.Where(us=> us.Email.Equals(request.Email)).AnyAsync();

                var resultUserName = await Context.Users.Where(us => us.UserName.Equals(request.UserName)).AnyAsync();

                if(resultEmail || resultUserName){
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "Error, el correo o el username ya existen."});
                }

                var usuario = new Usuario{
                    NombreCompleto = request.Nombre + request.Apellidos,
                    Email = request.Email,
                    UserName = request.UserName,
                };

                var resultNewUser = await UserManager.CreateAsync(usuario, request.Password);

                if(resultNewUser.Succeeded){
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Token = JwtGenerador.CreatToken(usuario),
                        UserName = usuario.UserName,
                        Email = usuario.Email
                    };
                }

                throw new Exception("No se pudo registrar el usuario");
            }
        }
    }
}