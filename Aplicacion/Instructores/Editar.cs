using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Editar
    {
        public class Ejecuta : IRequest{

        public Guid InstructorId { get; set; }
        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Titulo { get; set; }
        }


        public class EjecutaValida : AbstractValidator<Ejecuta>{
            public EjecutaValida()
            {
                 RuleFor(x => x.Nombre);
                 RuleFor(x => x.Apellidos);
                 RuleFor(x => x.Titulo);
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
             private readonly IInstructor _instructorRepository;

            public Manejador(IInstructor instructorRepository)
            {
                this._instructorRepository = instructorRepository;
            }

            public async  Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                  var instructor = new InstructorModel {
                    InstructorId = request.InstructorId,
                    Nombre = request.Nombre,
                    Apellidos = request.Apellidos,
                    Titulo = request.Titulo,
                };

                var result  = await _instructorRepository.Actualizar(instructor);

                if (result > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar la informacion del instructor");
            }
        }
    }
}