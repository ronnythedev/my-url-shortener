using shortid;
using shortid.Configuration;

namespace url_shortener_api.Services;

public class UrlShorteningService
{
    public const int NUMBER_OF_CODE_CHARACTERS = 7;
    public const bool USE_SPECIAL_CHARACTERS = false;

    public string GenerateUniqueCode()
    {

        var options = new GenerationOptions(useSpecialCharacters: USE_SPECIAL_CHARACTERS, length: NUMBER_OF_CODE_CHARACTERS);
        var code = ShortId.Generate(options);

        return code.ToString();
    }
}
