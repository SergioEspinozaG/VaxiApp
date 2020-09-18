using System.Net;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia;
using Aplicacion.ManejadorError;

namespace Aplicacion.Comentarios
{
    public class Eliminar
    {
        public class Ejecuta : IRequest{
            public Guid ComentarioId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
        protected readonly CursosOnlineContext Context;

            public Manejador(CursosOnlineContext context)
            {
                this.Context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var comentario = await Context.comentario.FindAsync(request.ComentarioId);

                if(comentario == null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el comentario."});
                }

                Context.Remove(comentario);
                var result = await  Context.SaveChangesAsync();

                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("Error al eliminar el comentario");
            }
        }
    }
}