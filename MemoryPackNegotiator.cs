using Carter;
using MemoryPack;
using Microsoft.Net.Http.Headers;

namespace carter_memorypack_api
{
    public partial class MemoryPackNegotiator : IResponseNegotiator
    {
        private const string _memoryPackMediaType = "application/x-memorypack";

        private readonly ILogger _logger;

        public MemoryPackNegotiator(ILogger<MemoryPackNegotiator> logger)
        {
            _logger = logger;
        }

        public bool CanHandle(MediaTypeHeaderValue accept)
        {
            var matches = accept.MatchesMediaType(_memoryPackMediaType);
            return matches;
        }

        public Task Handle<T>(HttpRequest request, HttpResponse response, T data, CancellationToken cancellationToken)
        {
            Log.ExecutingHandler(_logger);

            response.ContentType = _memoryPackMediaType;

            var bin = MemoryPackSerializer.Serialize(data);
            using var ms = new MemoryStream(bin);

            Log.ExecutedHandler(_logger);
            return ms.CopyToAsync(response.Body);
        }

        private static partial class Log
        {
            [LoggerMessage(0, LogLevel.Information, "Executing handler 'MemoryPackNegotiator'", EventName = "ExecutingHandler")]
            public static partial void ExecutingHandler(ILogger logger);

            [LoggerMessage(1, LogLevel.Information, "Executed handler 'MemoryPackNegotiator'", EventName = "ExecutingHandler")]
            public static partial void ExecutedHandler(ILogger logger);
        }
    }
}
