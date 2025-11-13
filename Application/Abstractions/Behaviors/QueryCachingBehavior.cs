using Application.Abstractions.Caching;
using Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Application.Abstractions.Behaviors;

internal sealed class QueryCachingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    where TResponse : Result
{
    private readonly HybridCache _cache;
    private readonly ILogger<QueryCachingBehavior<TRequest, TResponse>> _logger;

    public QueryCachingBehavior(
        HybridCache cacheService,
        ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cacheService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        return await _cache.GetOrCreateAsync(request.CacheKey, async ct =>
        {
            _logger.LogInformation("Cache {CacheKey} does not exist, forwarding request.", request.CacheKey);
            return await next(ct);
        }, cancellationToken: cancellationToken);
    }
}
