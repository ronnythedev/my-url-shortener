using Microsoft.EntityFrameworkCore;
using url_shortener_api.Entities;
using url_shortener_api.Services;

namespace url_shortener_api;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ShortnedUrl> ShortnedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortnedUrl>(builder =>
        {
            builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NUMBER_OF_CODE_CHARACTERS);

            builder.HasIndex(s => s.Code).IsUnique();
        });
    }
}
