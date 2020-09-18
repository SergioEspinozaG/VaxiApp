using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructor;
using static Aplicacion.Instructores.Editar;

namespace WebAPI.Controllers
{
    public class InstructorController : MiControllerBase{           
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InstructorModel>>> ObtenerInstructores(){
        return await Mediador.Send(new Consulta.Lista());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InstructorModel>> ObtenerInstructorPorId(Guid id){
        return await Mediador.Send(new ConsultaId.Ejecuta{Id = id});
    }

    [HttpPost]
    public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data){
        return await Mediador.Send(data);
    } 

    [HttpPut("{id}")]
    public async Task<ActionResult<Unit>> Actualizar(Guid id, Editar.Ejecuta data){
        data.InstructorId = id;
        return await Mediador.Send(data);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Unit>> Eliminar(Guid id){
        return await Mediador.Send(new Eliminar.Ejecuta{Id = id});
    }

    }

}