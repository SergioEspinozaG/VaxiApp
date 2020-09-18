using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Comentarios
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
        public string Alumno { get; set; }

        public int Puntaje { get; set; }

        public string ComentarioTexto { get; set; }

        public Guid CursoId { get; set; }
    }
    public class EjecutaValidacion : AbstractValidator<Ejecuta>
    {
        public EjecutaValidacion()
        {
            RuleFor(x => x.CursoId).NotEmpty();
            RuleFor(x => x.Alumno).NotEmpty();
            RuleFor(x => x.ComentarioTexto).NotEmpty();
            RuleFor(x => x.Puntaje).NotEmpty();
        }    
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

                    var nuevoComentario = new Comentario{
                        ComentarioId = Guid.NewGuid(),
                        Alumno = request.Alumno,
                        ComentarioTexto = request.ComentarioTexto,
                        CursoId = request.CursoId,
                        Puntaje = request.Puntaje,
                        FechaDeCreacion = DateTime.UtcNow
                    };

                    Context.comentario.Add(nuevoComentario);

                   var result = await Context.SaveChangesAsync();

                    if (result > 0)
                    {
                        return Unit.Value;
                    }

                    throw new Exception("No se pudo registrar el comentario");
            }
        }
    }
}