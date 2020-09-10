using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>{
            public string Email { get; set; }

            public string  Password { get; set; }
        }

         public class EjecutaVlidacion : AbstractValidator<Ejecuta>{
            public EjecutaVlidacion()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();

            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;

            private readonly SignInManager<Usuario> _signInManager;

            private readonly IJwtGenerador _jwtGenerador;


            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                    _userManager = userManager;
                    _signInManager = signInManager;
                    _jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);

                if(usuario == null) {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized, new {usuario = "Error el email no existe"});
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                if(resultado.Succeeded){
                    var dataUser = new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerador.CreatToken(usuario),
                        Email = usuario.Email,
                        UserName = usuario.UserName,
                        Imagen = null,
                    };
                    return dataUser;
                }
                
                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }

    }
}