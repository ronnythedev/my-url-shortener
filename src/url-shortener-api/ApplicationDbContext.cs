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
        modelBuilder.Entity<ShortnedUrl>().HasKey(e => e.Id);
        modelBuilder.Entity<ShortnedUrl>().Property(e => e.OriginalUrl).IsRequired();
        modelBuilder.Entity<ShortnedUrl>().Property(e => e.ShortenedUrl).IsRequired();
        modelBuilder.Entity<ShortnedUrl>().Property(e => e.Code).IsRequired().HasMaxLength(UrlShorteningService.NUMBER_OF_CODE_CHARACTERS);
        modelBuilder.Entity<ShortnedUrl>().HasIndex(e => e.Code).IsUnique();
        modelBuilder.Entity<ShortnedUrl>().Property(e => e.CreatedAtUtc).IsRequired();
    }
}
