using System.Threading.Tasks;

namespace AlmVR.Server.Core.Providers
{
    public interface IConfigurationProvider
    {
        Task<T> GetConfigurationAsync<T>();
        Task SetConfigurationAsync<T>();
    }
}
