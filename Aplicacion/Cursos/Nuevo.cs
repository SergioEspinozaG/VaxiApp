using System.ComponentModel.DataAnnotations;
using System;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Persistencia;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Collections.Generic;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecutar : IRequest{ 
        
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public DateTime? FechaPublicacion  { get; set; }

        public List<Guid> ListaInstructor {get; set;}

        public decimal Precio  { get; set; }

        public decimal Promocion { get; set; }

        }

        public class EjecutaValidacion  : AbstractValidator<Ejecutar>{
            public EjecutaValidacion()
            {
                 RuleFor(x => x.Titulo).NotEmpty();
                 RuleFor(x => x.Descripcion).NotEmpty();
                 RuleFor(x => x.FechaPublicacion).NotEmpty();

            }
        }

        public class Manejador : IRequestHandler<Ejecutar>
        {
            protected readonly CursosOnlineContext Context;
            public Manejador(CursosOnlineContext context)
            {
                this.Context = context;
            }
            public async Task<Unit> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                Guid _cursoId = new Guid();

                var newCurso = new Curso(){
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };

                Context.curso.Add(newCurso);


                if (request.ListaInstructor != null)
                {
                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor{
                            CursoId = newCurso.CursoId,
                            InstructorId = id
                        };
                        Context.cursoInstructor.Add(cursoInstructor);
                    }
                }

                /*Logica para agregar el precio*/

                var precioEntidad = new Precio {
                    CursoId = newCurso.CursoId,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    PrecioId = Guid.NewGuid(),
                };

                Context.precio.Add(precioEntidad);

                var result = await Context.SaveChangesAsync();

                if(result > 0){
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el curso");
            }
        }


    }
}