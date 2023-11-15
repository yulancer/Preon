using Preon.Solver.Contracts.Models;

namespace Preon.Solver.Contracts.Abstractions;

public interface IWebApiClient
{
    Task SendPerson(PersonModel personModel, CancellationToken token);
}