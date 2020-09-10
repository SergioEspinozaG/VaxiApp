using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Aplicacion.Contratos
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UsuarioSesion(IHttpContextAccessor _httpContext){
                httpContextAccessor = _httpContext;
        }
        public string ObtenerUsuarioSesion()
        {
            var userName = httpContextAccessor.HttpContext.User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
            return userName;
        }
    }
}