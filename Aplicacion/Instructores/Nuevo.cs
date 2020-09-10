using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Nuevo
    {
        public class Ejecuta : IRequest{

        public string Nombre { get; set; }

        public string Apellidos { get; set; }

        public string Titulo { get; set; }
        }

        public class EjecutaValida :  AbstractValidator<Ejecuta>{
            
            public EjecutaValida()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Titulo).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor _instructorRepository;

            public Manejador(IInstructor instructorRepository)
            {
                this._instructorRepository = instructorRepository;
            }
            public  async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructor = new InstructorModel {
                    Nombre = request.Nombre,
                    Apellidos = request.Apellidos,
                    Titulo = request.Titulo,
                };
                
                var resultado = await _instructorRepository.NuevoInstructor(instructor);

                if(resultado > 0 ){
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el nuevo instructor");

            }
        }
    }
}