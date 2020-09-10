using System;
using System.Net;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware {
    public class ManejadorErrorMiddleWare {
        public readonly RequestDelegate _next;

        public readonly ILogger<ManejadorErrorMiddleWare> _logger;
        public ManejadorErrorMiddleWare (RequestDelegate next, ILogger<ManejadorErrorMiddleWare> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke (HttpContext context) {
            try {
                await _next (context);
            } catch (Exception ex) {
                await ManejadorExcepcionAsync (context, ex, _logger);
            }

        }

        private async Task ManejadorExcepcionAsync (HttpContext context, Exception exception, ILogger<ManejadorErrorMiddleWare> logger) {
            object errores = null;
            switch (exception) {
                case ManejadorExcepcion me:
                    logger.LogError (exception, "Manejador Error");
                    errores = me._errores;
                    context.Response.StatusCode = (int) me._codigo;
                    break;
                case Exception ex:
                    logger.LogError (exception, "Error de Servidor");
                    errores = string.IsNullOrWhiteSpace (ex.Message) ? "Error" : ex.Message;
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.ContentType = "application/json";
            if (errores != null) {
                var resultados = JsonConvert.SerializeObject (new { errores });
                await context.Response.WriteAsync (resultados);
            }

        }
    }
}