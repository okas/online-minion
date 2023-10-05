using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace OnlineMinion.Utilities.Mediatr.Behaviors;

public class ErrorOrLoggingBehavior<TRequest, TResponse>(ILogger<ErrorOrLoggingBehavior<TRequest, TResponse>> logger)
    : IErrorOrPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly string _requestTypeName = typeof(TRequest).Name;

    public async Task<TResponse> Handle(TRequest _, RequestHandlerDelegate<TResponse> next, CancellationToken __)
    {
        logger.LogInformation("Start handling {RequestType}", _requestTypeName);

        var result = await next().ConfigureAwait(false);

        if (result.IsError)
        {
            LogErrors(result.Errors!);
        }

        logger.LogInformation("End handling {RequestType}", _requestTypeName);

        return result;
    }

    private void LogErrors(List<Error> errors)
    {
        ref var firstItemRef = ref MemoryMarshal.GetReference(CollectionsMarshal.AsSpan(errors));

        for (var offset = 0; offset < errors.Count; offset++)
        {
            var error = Unsafe.Add(ref firstItemRef, offset);

            var logLevel = error.Type switch
            {
                ErrorType.Failure => LogLevel.Critical,
                ErrorType.Unexpected => LogLevel.Error,
                ErrorType.Validation => LogLevel.Warning,
                ErrorType.Conflict => LogLevel.Warning,
                ErrorType.NotFound => LogLevel.Warning,
                _ => LogLevel.Information,
            };

            logger.Log(
                logLevel,
                "Problem on handling {RequestType}: code `{Code}`, description `{Description}`",
                _requestTypeName,
                error.Code,
                error.Description
            );
        }
    }
}
