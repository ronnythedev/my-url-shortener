using Microsoft.EntityFrameworkCore;
using shortid;
using shortid.Configuration;

namespace url_shortener_api.Services;

public class UrlShorteningService
{
    public const int NUMBER_OF_CODE_CHARACTERS = 7;
    public const bool USE_SPECIAL_CHARACTERS = false;

    private readonly ApplicationDbContext _dbContext;

    public UrlShorteningService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GenerateUniqueCode()
    {

        var options = new GenerationOptions(useSpecialCharacters: USE_SPECIAL_CHARACTERS, length: NUMBER_OF_CODE_CHARACTERS);

        while (true)
        {
            var code = ShortId.Generate(options).ToLower();

            if (!await _dbContext.ShortnedUrls.AnyAsync(u => u.Code == code))
            {
                return code;
            }
        }
    }
}
