using System.Text.Json.Serialization;
using Preon.Solver.Contracts.Enums;

namespace Preon.Solver.Integration.Contracts;

public class PersonContract
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    public ScheduleType ScheduleType { get; set; }

    [JsonPropertyName("isStrict")]
    public bool IsStrict { get; set; }

    [JsonPropertyName("rtart")]
    public int Start { get; set; }

    [JsonPropertyName("end")]
    public int End { get; set; }

    [JsonPropertyName("extraHolidays")]
    public int[] ExtraHolidays { get; set; }
}