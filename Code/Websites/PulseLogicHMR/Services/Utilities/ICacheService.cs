using System;
using System.Threading.Tasks;

namespace PulseLogicHMR.Services.Utilities
{
    public interface ICacheService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan? absoluteExpireTime = null);
        void Remove(string key);
    }
}
