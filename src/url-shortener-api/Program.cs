
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using url_shortener_api;
using url_shortener_api.Entities;
using url_shortener_api.Extentions;
using url_shortener_api.Models;
using url_shortener_api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Cache");
});

builder.Services.AddScoped<UrlShorteningService>();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("fixed", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromSeconds(10)
            }
        ));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigration();
}

app.MapPost("/shorten", async (ShortenUrlRequest request,
                            UrlShorteningService urlShorteningService,
                            ApplicationDbContext applicationDbContext,
                            HttpContext httpContext) =>
{
    // validate if the given URL is valid
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("Specified URL is invalid");
    }

    var code = await urlShorteningService.GenerateUniqueCode();

    var shortenedUrl = new ShortnedUrl
    {
        Id = Guid.NewGuid(),
        OriginalUrl = request.Url,
        Code = code,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{code}",
        CreatedAtUtc = DateTime.UtcNow
    };

    applicationDbContext.Add(shortenedUrl);

    await applicationDbContext.SaveChangesAsync();

    return Results.Ok(shortenedUrl.ShortUrl);

}).RequireRateLimiting("fixed");

app.MapGet("/{code}", async (string code, ApplicationDbContext applicationDbContext, IDistributedCache cache, CancellationToken ct) =>
{
    var shortenedUrl = await cache.GetAsync(code, async token =>
    {
        var shortenedUrl = await applicationDbContext.ShortnedUrls
            .AsNoTracking()
            .Where(x => x.Code == code)
            .FirstOrDefaultAsync(token);

        return shortenedUrl;

    }, CacheOptions.DefaultExpiration, ct);

    if (shortenedUrl == null)
    {
        return Results.NotFound();
    }

    return Results.Redirect(shortenedUrl.OriginalUrl);
});

app.UseHttpsRedirection();

app.UseRateLimiter();

app.Run();
