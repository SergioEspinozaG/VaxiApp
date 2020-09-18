using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioRolAgregar
    {
        public class Ejecuta : IRequest{

            public string UserName { get; set; }
            public string RoleNombre { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>{
            public EjecutaValidador()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.RoleNombre).NotEmpty();
            }
        }

        public class Manejado : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

            public Manejado(UserManager<Usuario>  userManager, RoleManager<IdentityRole> roleManager)
            {
                this._userManager = userManager;
                this._roleManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var role =  await _roleManager.FindByNameAsync(request.RoleNombre);

                if(role == null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El rol no existe."}); 
                }

                var usuarioIden = await _userManager.FindByNameAsync(request.UserName);

                if (usuarioIden == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "El usuario no existe"}); 
                }

                var resultado =  await _userManager.AddToRoleAsync(usuarioIden, request.RoleNombre);

                if (resultado.Succeeded)
                {
                    return Unit.Value;
                }

                throw new System.Exception("No se pudo agregar el rol al usuario.");
            }
        }


    }
}