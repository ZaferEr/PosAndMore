using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Dapper.SimpleLoadCore
{
    // Minimal gerekli tipler (ObjectCache taklidi için)
    public class CacheItem
    {
        public CacheItem(string key, object value, string regionName = null)
        {
            Key = key;
            Value = value;
            RegionName = regionName;
        }

        public string Key { get; }
        public object Value { get; set; }
        public string RegionName { get; }
    }

    public class CacheItemPolicy
    {
        public DateTimeOffset AbsoluteExpiration { get; set; } = DateTimeOffset.MaxValue;
        public TimeSpan SlidingExpiration { get; set; } = TimeSpan.Zero;
        public CacheItemPriority Priority { get; set; } = CacheItemPriority.Normal;
        public List<ChangeMonitor> ChangeMonitors { get; } = new List<ChangeMonitor>();
    }

    public abstract class ChangeMonitor : IDisposable
    {
        protected virtual void Dispose(bool disposing) { }
        public void Dispose() => Dispose(true);
    }

    public abstract class CacheEntryChangeMonitor : ChangeMonitor { }

 

    [Flags]
    public enum DefaultCacheCapabilities
    {
        None = 0,
        InMemoryProvider = 1,
        CacheEntryUpdateCallback = 2,
        CacheEntryRemovalCallback = 4,
        AbsoluteExpirations = 8,
        SlidingExpirations = 16,
        CacheEntryChangeMonitors = 32,
        OutOfProcessProvider = 64
    }

    public abstract class ObjectCache
    {
        public abstract object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null);
        public abstract object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);
        public abstract CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy);
        public abstract bool Contains(string key, string regionName = null);
        public abstract CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys, string regionName = null);
        public abstract DefaultCacheCapabilities DefaultCacheCapabilities { get; }
        public abstract object Get(string key, string regionName = null);
        public abstract CacheItem GetCacheItem(string key, string regionName = null);
        public abstract long GetCount(string regionName = null);
        public abstract IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null);
        public abstract string Name { get; }
        public abstract object Remove(string key, string regionName = null);
        public abstract void Set(string key, object value, CacheItemPolicy policy, string regionName = null);
        public abstract void Set(CacheItem item, CacheItemPolicy policy);
        public abstract void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null);
        public abstract object this[string key] { get; set; }
        protected abstract IEnumerator<KeyValuePair<string, object>> GetEnumerator();
    }
}