using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RedisNetCore.Models;
using RedisNetCore.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedisNetCore.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly IDistributedCache _distributedCache;
        private readonly ICountryService _countryService;

        public CountryController(ILogger<CountryController> logger, IDistributedCache distributedCache, ICountryService countryService)
        {
            _logger = logger;
            _distributedCache = distributedCache;
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IList<Country>> GetAsync()
        {
            var countryListFromCache = await _distributedCache.GetStringAsync("Country");

            if (countryListFromCache != null)
            {
                return JsonConvert.DeserializeObject<IList<Country>>(countryListFromCache);
            }

            var countryList = await _countryService.GetAsync().ConfigureAwait(false);

            await _distributedCache.SetStringAsync("Country", JsonConvert.SerializeObject(countryList),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(1),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                });

            return countryList;
        }
    }
}
