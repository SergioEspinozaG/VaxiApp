using System;
using System.Collections.Generic;
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
    public class Editar
    {
        public class Ejecuta : IRequest{
        public Guid CursoId { get; set; }   
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public DateTime? FechaPublicacion  { get; set; }

        public List<Guid> ListaInstructor { get; set; }

        public decimal? Precio { get; set; }

        public decimal? Promocion { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion()
            {
                RuleFor(x => x.CursoId).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly CursosOnlineContext Context;
            public Manejador(CursosOnlineContext context)
            {
                this.Context =  context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso = await Context.curso.FindAsync(request.CursoId);
                if(curso == null){
                     //throw new Exception("No se puede eliminar el curso");

                     throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {curso = "El curso no existe"});
                }

                 curso.Titulo = request.Titulo ?? curso.Titulo;
                 curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                 curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;
                 curso.FechaDeCreacion = DateTime.UtcNow;

                /*Actualizar el precio del curso*/

                var cursoPrecio = Context.precio.Where(curso => curso.CursoId == request.CursoId).FirstOrDefault();

                if(cursoPrecio != null){
                     cursoPrecio.PrecioActual = request.Precio ??  cursoPrecio.PrecioActual;
                     cursoPrecio.Promocion =  request.Promocion ?? cursoPrecio.Promocion;
                }else{
                    var precioEntidad = new Precio{
                         PrecioId = Guid.NewGuid(),
                         PrecioActual = request.Precio ?? 0,
                         Promocion = request.Promocion ?? 0,
                         CursoId = request.CursoId
                    };

                    await Context.precio.AddAsync(precioEntidad);
                }

                if(request.ListaInstructor != null && request.ListaInstructor.Count > 0){
                    var instructoresBD = Context.cursoInstructor.Where( x => x.CursoId == request.CursoId).ToList();

                    /*Elimina los instructores actuales del curso en la base de datos*/
                    foreach (var instructoresEliminar in instructoresBD)
                    {
                        Context.cursoInstructor.Remove(instructoresEliminar);
                    }

                    /*Procedimiento para agregar a los nuevos instructores del cliente*/
                    foreach (var instructorId in request.ListaInstructor)
                    {
                        var nuevoInstructor = new CursoInstructor{
                            CursoId = request.CursoId,
                            InstructorId = instructorId  
                        };

                        Context.cursoInstructor.Add(nuevoInstructor);
                    }
                }

                 var result = await Context.SaveChangesAsync();

                 if(result > 0 ) { 
                     return Unit.Value; 
                }

                throw new Exception ("No se guardaron los cambios en el curso.");

            }
        }


    }
}