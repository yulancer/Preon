using Preon.Solver.Contracts.Abstractions;
using Preon.Solver.Contracts.Enums;
using Preon.Solver.Contracts.Models;

namespace Preon.Solver.Domain.Services;

public class FakePersonRepository : IPersonRepository
{
    public async Task<PersonModel> ById(string personId, CancellationToken cancellationToken)
    {
        return new PersonModel
        {
            ScheduleType = ScheduleType.Schedule2x2,
            Name = "fake schedule",
            ExtraHolidays = Array.Empty<int>()
        };
    }
}