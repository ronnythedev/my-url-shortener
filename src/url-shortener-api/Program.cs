using Microsoft.AspNetCore.Http.HttpResults;
using url_shortener_api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/shorten", (ShortenUrlRequest request) =>
{
    // validate if the given URL is valid
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("Specified URL is invalid");
    }

    return Results.Ok("https://short.url/" + Guid.NewGuid().ToString());
});

app.UseHttpsRedirection();

app.Run();
