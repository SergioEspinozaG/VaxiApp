using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class RoleLista
    {
        public class Ejecuta : IRequest<List<IdentityRole>>{

        }

        public class Manejador : IRequestHandler<Ejecuta, List<IdentityRole>>
        {
            private readonly CursosOnlineContext Context;

            public Manejador(CursosOnlineContext context)
            {
                this.Context = context;
            }
            public async Task<List<IdentityRole>> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                return await Context.Roles.ToListAsync();
            }
        }


    }
}