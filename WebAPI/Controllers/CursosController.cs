using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : MiControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<CursoDto>>> Get(){
            return await Mediador.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDto>> Detalle(Guid id){
            return await Mediador.Send(new ConsultaId.cursoUnico{Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecutar data){
            return await Mediador.Send(data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta data){
            data.CursoId = id;
            return await Mediador.Send(data);
        }
    
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id){
            return await Mediador.Send(new Eliminar.Ejecuta{Id = id});
        }

    }
}