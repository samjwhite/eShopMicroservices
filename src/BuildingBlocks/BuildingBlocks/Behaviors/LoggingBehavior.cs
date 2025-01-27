using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} - Response={Response}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var times = new Stopwatch();
        times.Start();

        var response = await next();

        times.Stop();
        var timeTaken = times.Elapsed;
        if (timeTaken.Seconds > 3)
        {
            logger.LogWarning("[SLOW] Handle request={Request} - Response={Response} - Time={TimeTaken}",
                typeof(TRequest).Name, typeof(TResponse).Name, timeTaken.Seconds);
        }

        logger.LogInformation("[END] Handle request={Request} - Response={Response}",
            typeof(TRequest).Name, typeof(TResponse).Name, response);
        return response;
    }
}
