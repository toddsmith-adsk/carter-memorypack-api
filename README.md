# carter-memorypack-api

.NET 9 / Carter / MemoryPack / IResponseNegotiator example

Open app.http and run the examples.

`GET /` returns a regular Carter negotiated result but will throw an exception.

`GET /result` will convert the Carter negotiated result to an IResult and avoid the exception.

Cause of the exception https://github.com/dotnet/aspnetcore/blob/1770dcf4e81872395cc4d3b3b3efbaef91f8020a/src/Shared/RouteHandlers/ExecuteHandlerHelper.cs#L27

