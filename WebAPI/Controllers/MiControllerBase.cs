using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiControllerBase : ControllerBase
    {
        private  IMediator _Mediador;
        
        protected IMediator Mediador =>_Mediador ?? (_Mediador = HttpContext.RequestServices.GetService<IMediator>());

    }
}