namespace Pepro.Presentation.Payloads;

public sealed record AssignmentDetailPagePayload
{
    public required int ProjectId { get; init; }

    public required string ProjectName { get; init; }
}
