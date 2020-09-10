using System.Runtime.CompilerServices;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class cursoUnico : IRequest<CursoDto>{
            public Guid Id {get; set;}
        }

        public class cursoUnicoValidacion : AbstractValidator<cursoUnico>{
            public cursoUnicoValidacion()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<cursoUnico, CursoDto>
        {
        protected readonly CursosOnlineContext Context;

        private readonly IMapper _mapper;

            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                this.Context = context;
                this._mapper = mapper;
            }

            public async Task<CursoDto> Handle(cursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await Context.curso.Include( x => x.ComentarioCurso)
                                                .Include( x => x.PrecioPromocion)
                                                .Include( x=> x.InstructoresLink)
                                                .ThenInclude(y => y.Instructor)
                                                .FirstOrDefaultAsync( a=> a.CursoId == request.Id);

                if(curso == null){
                     //throw new Exception("No se puede eliminar el curso");

                     throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {curso = "No se encontro el curso"});
                }

                 return _mapper.Map<Curso, CursoDto>(curso);
            }
        }
    }
}