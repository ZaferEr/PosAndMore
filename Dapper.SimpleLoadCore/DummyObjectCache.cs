using Dapper.SimpleLoadCore;
using Microsoft.Extensions.Caching.Memory;

public class SimpleLoadMemoryCache : ObjectCache
{
    private readonly IMemoryCache _cache;

    public SimpleLoadMemoryCache(IMemoryCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public override string Name => "SimpleLoadMemoryCache";

    public override DefaultCacheCapabilities DefaultCacheCapabilities =>
        DefaultCacheCapabilities.InMemoryProvider |
        DefaultCacheCapabilities.AbsoluteExpirations |
        DefaultCacheCapabilities.SlidingExpirations;

    public override object this[string key]
    {
        get => Get(key);
        set => Set(key, value, new CacheItemPolicy());
    }

    public override object Get(string key, string regionName = null)
    {
        _cache.TryGetValue(key, out object value);
        return value;
    }

    public override CacheItem GetCacheItem(string key, string regionName = null)
    {
        if (_cache.TryGetValue(key, out object value))
            return new CacheItem(key, value);
        return null;
    }

    public override bool Contains(string key, string regionName = null) =>
        _cache.TryGetValue(key, out _);

    public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null) =>
        _cache.Set(key, value, absoluteExpiration);

    public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
    {
        var options = new MemoryCacheEntryOptions();

        if (policy.AbsoluteExpiration != DateTimeOffset.MaxValue)
            options.AbsoluteExpiration = policy.AbsoluteExpiration;

        if (policy.SlidingExpiration != TimeSpan.Zero)
            options.SlidingExpiration = policy.SlidingExpiration;

        options.Priority = policy.Priority switch
        {
            CacheItemPriority.High => CacheItemPriority.High,
            CacheItemPriority.Low => CacheItemPriority.Low,
            _ => CacheItemPriority.Normal
        };

        _cache.Set(key, value, options);
    }

    public override void Set(CacheItem item, CacheItemPolicy policy) =>
        Set(item.Key, item.Value, policy, item.RegionName);

    public override object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null) =>
        _cache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpiration = absoluteExpiration;
            return value;
        });

    public override object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null) =>
        _cache.GetOrCreate(key, entry =>
        {
            if (policy.AbsoluteExpiration != DateTimeOffset.MaxValue)
                entry.AbsoluteExpiration = policy.AbsoluteExpiration;

            if (policy.SlidingExpiration != TimeSpan.Zero)
                entry.SlidingExpiration = policy.SlidingExpiration;

            entry.Priority = policy.Priority switch
            {
                CacheItemPriority.High  => CacheItemPriority.High,
                CacheItemPriority.Low => CacheItemPriority.Low,
                _ => CacheItemPriority.Normal
            };

            return value;
        });

    public override CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy)
    {
        var result = AddOrGetExisting(value.Key, value.Value, policy);
        return result != null ? new CacheItem(value.Key, result) : null;
    }

    public override object Remove(string key, string regionName = null)
    {
        if (_cache.TryGetValue(key, out object value))
        {
            _cache.Remove(key);
            return value;
        }
        return null;
    }

    public override IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null)
    {
        var dict = new Dictionary<string, object>();
        foreach (var key in keys)
        {
            if (_cache.TryGetValue(key, out object val))
                dict[key] = val;
        }
        return dict;
    }

    public override long GetCount(string regionName = null) => 0; // IMemoryCache count vermiyor

    public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys, string regionName = null) => null;

    protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        yield break; // Enumeration desteklenmiyor
    }
}
