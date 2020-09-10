using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Eliminar
    {
        public class Ejecuta :IRequest {

        public Guid Id { get; set; }
        
        } 


        public class Manejador : IRequestHandler<Ejecuta> {

            private readonly IInstructor _instructorRepository;

            public Manejador(IInstructor instructorRepository)
            {
                this._instructorRepository = instructorRepository;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var result = await _instructorRepository.Elimina(request.Id);

                if(result > 0 ){
                    return Unit.Value;
                }

                throw new Exception("No se puede eliminar el instructor.");
            }
        }
    }
}