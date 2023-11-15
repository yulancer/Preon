using MediatR;
using Microsoft.AspNetCore.Mvc;
using Preon.Solver.Contracts.Commands;

namespace Preon.Solver.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SendController : ControllerBase
    {
     
        private readonly IMediator _mediator;

        public SendController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{personId}")]
        public Task Person(string personId) =>
            _mediator.Send(new SendPersonCommand {PersonId = personId});
    }
}