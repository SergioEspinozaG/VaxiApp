using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecutar : IRequest<UsuarioData>{

        }

        public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;

            private readonly IJwtGenerador _jwtGenerador;

            private readonly IUsuarioSesion _usuarioSesion;


            public Manejador(UserManager<Usuario> usuarioManajer, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion)
            {
                _userManager = usuarioManajer;
                _jwtGenerador = jwtGenerador;
                _usuarioSesion = usuarioSesion;
            }
            public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var user = await  _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
                return new UsuarioData{
                    NombreCompleto = user.NombreCompleto,
                    UserName =user.UserName,
                    Token = _jwtGenerador.CreatToken(user),
                    Imagen = null,
                    Email = user.Email,
                };
            }
        }

    }
}