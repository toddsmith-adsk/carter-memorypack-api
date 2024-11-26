using Carter;
using Carter.Response;

namespace carter_memorypack_api
{
    public class Endpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (HttpResponse res) =>
            {
                var data = Enumerable.Range(0, 1000).Select(i => $"Item ${i}").ToList();

                return res.Negotiate(data);
            });

            app.MapGet("/result", async (HttpResponse res) =>
            {
                var data = Enumerable.Range(0, 1000).Select(i => $"Item ${i}").ToList();

                return Results.Extensions.Negotiated(res.Negotiate(data));
            });
        }
    }

    public static class NegotiatedResultExtensions
    {
        public static IResult Negotiated(this IResultExtensions _, Task obj)
        {
            return new NegotiatedResult(obj);
        }

        private class NegotiatedResult : IResult
        {
            private readonly Task _item;

            public NegotiatedResult(Task item)
            {
                _item = item;
            }

            public Task ExecuteAsync(HttpContext httpContext)
            {
                return _item;
            }
        }
    }
}
