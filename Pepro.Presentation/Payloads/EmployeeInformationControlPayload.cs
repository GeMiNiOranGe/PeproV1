namespace Pepro.Presentation.Payloads;

public sealed record PersonalPagePayload
{
    public required int EmployeeId { get; init; }
}
