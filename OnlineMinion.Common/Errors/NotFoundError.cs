using FluentResults;

namespace OnlineMinion.Common.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string? message) : base(message ?? string.Empty) { }

    public NotFoundError(string modelName, object modelId) : base($"{modelName} with Id {modelId} not found")
    {
        WithMetadata(nameof(modelId), modelId);
    }
}
