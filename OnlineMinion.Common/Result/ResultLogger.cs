using FluentResults;
using Microsoft.Extensions.Logging;

namespace OnlineMinion.Common.Result;

public class ResultLogger : IResultLogger
{
    private readonly ILoggerFactory _loggerFactory;

    public ResultLogger(ILoggerFactory loggerFactory) => _loggerFactory = loggerFactory;

    public void Log(string context, string content, ResultBase result, LogLevel logLevel)
    {
        var logger = _loggerFactory.CreateLogger(context);
        Log(content, result, logLevel, logger);
    }

    public void Log<TContext>(string content, ResultBase result, LogLevel logLevel)
    {
        var logger = _loggerFactory.CreateLogger<TContext>();
        Log(content, result, logLevel, logger);
    }

    private static void Log(string content, IResultBase result, LogLevel logLevel, ILogger logger)
    {
        var reasons = result.Reasons.Select(reason => reason.Message);

        if (string.IsNullOrWhiteSpace(content))
        {
            logger.Log(logLevel, "Result: {Reasons}", reasons);
        }
        else
        {
            logger.Log(logLevel, "Result: {Reasons}, {MoreInfo}", reasons, content);
        }
    }
}
