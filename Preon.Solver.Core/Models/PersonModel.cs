using Preon.Solver.Contracts.Enums;

namespace Preon.Solver.Contracts.Models;

public class PersonModel
{
    public string Name { get; set; }

    public ScheduleType ScheduleType { get; set; }

    public bool IsStrict { get; set; }

    public int Start { get; set; }

    public int End { get; set; }

    public int[] ExtraHolidays { get; set; }
}