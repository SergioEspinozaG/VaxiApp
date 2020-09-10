using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class UsuarioController : MiControllerBase
    {
        [HttpPost("Login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta  paramentros){
            return await Mediador.Send(paramentros);
        }

        [HttpPost("Registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta registroUsuario){
            return await Mediador.Send(registroUsuario);
        }

        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario(){
            return await Mediador.Send(new UsuarioActual.Ejecutar());
        }
    }
}