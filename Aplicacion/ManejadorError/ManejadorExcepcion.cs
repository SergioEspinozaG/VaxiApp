using System.Net;
using System;
namespace Aplicacion.ManejadorError
{
    public class ManejadorExcepcion : Exception
    {
        public HttpStatusCode _codigo { get; }

        public object _errores { get; }

        public ManejadorExcepcion(HttpStatusCode codigo, object errores = null)
        {
            _codigo = codigo;
            _errores = errores;
        }
    }
}