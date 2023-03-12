using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChairsBackend.Controllers
{
    public class CommonController : ControllerBase
    {

        private IMediator _mediator;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    }
}
