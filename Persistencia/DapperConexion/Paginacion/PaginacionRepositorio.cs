using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Persistencia.DapperConexion.Paginacion
{
    public class PaginacionRepositorio : IPaginacion
    {
            private readonly IFactoryConnection _factoryConnection;
            public PaginacionRepositorio(IFactoryConnection factoryConnection)
            {
                this._factoryConnection = factoryConnection;
            }


        public async Task<PaginacionModel> DevolverPaginancion(
            string storeProcedure,
            int numeroPagina,
            int cantidadElementos, 
            IDictionary<string, object> parametrosFiltro, 
            string ordenamientoColumna)
        {

                PaginacionModel paginacionModel = new PaginacionModel();

                List<IDictionary<string, object>> ListaReporte = null;

                int totalRecords = 0;
                int totalPaginas = 0;


                try
                {
                    var connection = _factoryConnection.GetConnection();

                    DynamicParameters parametros = new DynamicParameters();

                    foreach (var param in parametrosFiltro)
                    {
                        parametros.Add("@" + param.Key, param.Value);
                    }

                    parametros.Add("@NumeroPagina", numeroPagina);
                    parametros.Add("@CantidadElementos", cantidadElementos);
                    parametros.Add("@Ordenamiento", ordenamientoColumna);

                    parametros.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                    parametros.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);


                    var result = await connection.QueryAsync(storeProcedure, parametros, commandType: CommandType.StoredProcedure); 
                    ListaReporte = result.Select(x => (IDictionary<string, object>)x).ToList();

                    paginacionModel.ListaRecords = ListaReporte;
                    paginacionModel.NumeroPaginas = parametros.Get<int>("@TotalPaginas");
                    paginacionModel.TotalRecords = parametros.Get<int>("@TotalRecords");

                }
                catch (System.Exception)
                {
                    
                    throw new Exception("No se pudo ejecutar el procedimiento almacenado");
                } finally{
                    _factoryConnection.CloseConnection();
                } 



                return paginacionModel;
        }
    }
}