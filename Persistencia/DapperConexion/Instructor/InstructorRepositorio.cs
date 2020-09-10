using System.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection _factoryConnection;

        public InstructorRepositorio(IFactoryConnection factoryConnection)
        {
            this._factoryConnection = factoryConnection;
        }

        public async  Task<int> Actualizar(InstructorModel instructor)
        {
            var storeProcedure = "usp_instructor_editar";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var result = await connection.ExecuteAsync(storeProcedure, instructor, commandType:  CommandType.StoredProcedure);

                _factoryConnection.CloseConnection();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo editar el  instructor", e);
            }
        }

        public async Task<int> Elimina(Guid id)
        {

            var storeProcedure  = "usp_instructor_eliminar";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var result = await connection.ExecuteAsync(storeProcedure, new {InstructorId = id } ,  commandType:  CommandType.StoredProcedure);
                _factoryConnection.CloseConnection();

                return result;
            }
            catch (Exception e)
            {
                
                throw new Exception("No se pudo eliminar el instructor.", e);
            }
        }

        public async Task<int> NuevoInstructor(InstructorModel instructor)
        {
            try
            {
                var storeProcedure = "usp_instructor_nuevo";
                var connection = _factoryConnection.GetConnection();

                var NuevoInstructor = new InstructorModel {
                    InstructorId = Guid.NewGuid(),
                    Nombre = instructor.Nombre,
                    Apellidos = instructor.Apellidos,
                    Titulo = instructor.Titulo,
                };

                var result = await connection.ExecuteAsync(storeProcedure, NuevoInstructor, commandType:  CommandType.StoredProcedure);

                _factoryConnection.CloseConnection();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo guardar el nuevo instructor", e);
            }
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;

            string storeProcedure = "ups_Obtener_Instructores";

            try
            {
                var connection = _factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure, null, commandType : CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                
                throw new Exception("Error en la consulta de datos", e);
            }finally{
                    _factoryConnection.CloseConnection();
            }
            return instructorList;


        }

        public Task<InstructorModel> ObtenerPorId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}