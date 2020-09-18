using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class RoleController : MiControllerBase
    {
        [HttpPost("crear")]
        public async Task<ActionResult<Unit>> Crear(RolNuevo.Ejecuta parametros){
            return await Mediador.Send(parametros);
        }
        
        [HttpDelete("eliminar")]
        public async Task<ActionResult<Unit>> Eliminar(RolEliminar.Ejecuta parametros){
            return await Mediador.Send(parametros);
        }

        [HttpGet("listar")]
        public async Task<ActionResult<List<IdentityRole>>> Lista(){
            return await Mediador.Send(new RoleLista.Ejecuta());
        }

        [HttpPost("AgregarRoleUsuario")]
        public async Task<ActionResult<Unit>> AgregarRoleUsuario(UsuarioRolAgregar.Ejecuta parametros){
            return await Mediador.Send(parametros); 
        }

        [HttpPost("eliminarRoleUsuario")]
        public async Task<ActionResult<Unit>> EliminarRoleUsuario(UsuarioRolEliminar.Ejecuta parametros){
            return await Mediador.Send(parametros);
        }
    }
}