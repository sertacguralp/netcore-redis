using RedisNetCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedisNetCore.Services.Abstractions
{
    /// <summary>
    /// </summary>
    public interface ICountryService
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task<IList<Country>> GetAsync();
    }
}
