namespace url_shortener_api.Entities;

public class ShortnedUrl
{
    public Guid Id { get; set; }
    public string OriginalUrl { get; set; } = string.Empty;

    public string ShortenedUrl { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}
