using System.Collections.Generic;
using MediatR;
using Dominio;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using Microsoft.EntityFrameworkCore;
using Aplicacion.ManejadorError;
using System.Net;
using AutoMapper;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDto>> {

        }

        public class Manejador : IRequestHandler<ListaCursos, List<CursoDto>>
        {
            private readonly CursosOnlineContext Context;

            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                this.Context  = context;
                this._mapper = mapper;
            }


            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await Context.curso
                                            .Include( x => x.ComentarioCurso)
                                            .Include( x => x.PrecioPromocion)
                                            .Include(x => x.InstructoresLink)
                                            .ThenInclude(x => x.Instructor)
                                            .ToListAsync();

                if(cursos == null) {
                    throw new ManejadorExcepcion(HttpStatusCode.NoContent, new {cursos = "Error al listar los cursos, contacte al administrador del sistema"});
                }
                
                return _mapper.Map<List<Curso>, List<CursoDto>>(cursos);
            }
        }
    }
}