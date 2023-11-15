using MediatR;
using Preon.Solver.Contracts.Abstractions;
using Preon.Solver.Contracts.Commands;

namespace Preon.Solver.Domain.Handlers;

public class SendPersonHandler : IRequestHandler<SendPersonCommand>
{
    private readonly IPersonRepository _repository;
    private readonly IWebApiClient _webApiClient;

    public SendPersonHandler(IPersonRepository repository, IWebApiClient webApiClient)
    {
        _repository = repository;
        _webApiClient = webApiClient;
    }

    public async Task Handle(SendPersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository.ById(request.PersonId, cancellationToken);
        await _webApiClient.SendPerson(person, cancellationToken);
    }
}