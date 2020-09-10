using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public interface IInstructor
    {
         Task<IEnumerable<InstructorModel>> ObtenerLista();

         Task<InstructorModel>  ObtenerPorId(Guid id);

         Task<int> NuevoInstructor(InstructorModel instructor);

         Task<int> Actualizar(InstructorModel instructor);

         Task<int> Elimina(Guid id );
    }
}