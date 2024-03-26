﻿namespace Microsoft.Extensions.Caching.Distributed;

public static class CacheOptions
{
    public static DistributedCacheEntryOptions DefaultExpiration =>
        new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20) };
}