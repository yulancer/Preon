using MediatR;

namespace Preon.Solver.Contracts.Commands;

public class SendPersonCommand : IRequest
{
    public string PersonId { get; set; }
}