using Preon.Solver.Contracts.Models;

namespace Preon.Solver.Contracts.Abstractions;

public interface IPersonRepository
{
    Task<PersonModel> ById(string personId, CancellationToken cancellationToken);
}