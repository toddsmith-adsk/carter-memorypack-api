using Carter;
using carter_memorypack_api;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.ResponseCompression;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.WebHost.UseKestrel(options =>
{
    options.Limits.MaxResponseBufferSize = null;
});

builder.Services.AddCarter(configurator: c =>
{
    c.WithResponseNegotiator<MemoryPackNegotiator>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Optimal;
    //options.Level = System.IO.Compression.CompressionLevel.SmallestSize; // NOTE: this infinite loops on the HTTP response wtf?
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    //options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/x-memorypack" });
});
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(5);
    options.MaximumBodySize = Int32.MaxValue; // 2GB
    options.SizeLimit = Int32.MaxValue; // 2GB
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();

app.UseHttpsRedirection();

app.UseResponseCompression();
app.UseOutputCache();

app.Run();

