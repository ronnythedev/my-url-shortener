
using Microsoft.EntityFrameworkCore;
using url_shortener_api;
using url_shortener_api.Entities;
using url_shortener_api.Models;
using url_shortener_api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<UrlShorteningService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

    applicationDbContext.ShortnedUrls.Add(shortenedUrl);

    await applicationDbContext.SaveChangesAsync();

    return Results.Ok(shortenedUrl.ShortUrl);
});

app.UseHttpsRedirection();

app.Run();
