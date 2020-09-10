using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest{
            public Guid Id { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

          public class Manejador : IRequestHandler<Ejecuta>
          {
             private readonly CursosOnlineContext Context;
              public Manejador(CursosOnlineContext context)
              {
                 this.Context = context;
              }
              public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
             {

                var instructoresBD = Context.cursoInstructor.Where(curso => curso.CursoId == request.Id);

                foreach (var instructorId in instructoresBD)
                {
                    Context.cursoInstructor.Remove(instructorId);
                }


                /*Elimina los comentarios*/
                var comentariosBD = Context.comentario.Where(curso => curso.CursoId == request.Id);

                foreach (var comentario in comentariosBD)
                {
                    Context.comentario.Remove(comentario);
                }
                
                /*Elimina el precio*/
                var precioBD = Context.precio.Where(curso => curso.CursoId == request.Id).FirstOrDefault();
                if (precioBD != null)
                {
                    Context.precio.Remove(precioBD);
                }

                var curso =  await Context.curso.FindAsync(request.Id);

                if(curso == null){
                     //throw new Exception("No se puede eliminar el curso");

                     throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {curso = "No se encontro el curso"});
                }
                 
                 
                Context.Remove(curso);
                var result = Context.SaveChanges();

                if(result > 0 ){
                    return Unit.Value;
                }

                throw new Exception("No se pudieron guardar los cambios");


              }
          }

    }
}