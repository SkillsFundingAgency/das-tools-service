using Microsoft.AspNetCore.DataProtection;
using SFA.DAS.ToolService.Core.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.ToolService.Web.AppStart
{
    public static class DistributedCacheConfigurationExtension
    {
        public static void AddDistributedCache(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
                services.AddDataProtection()
                    .SetApplicationName(ApplicationConstants.ApplicationName);
            }
            else
            {
                var redisConnectionString = configuration["RedisConnectionString"];
                var redis = ConnectionMultiplexer.Connect($"{redisConnectionString},DefaultDatabase=0");
                services.AddDataProtection()
                    .SetApplicationName(ApplicationConstants.ApplicationName)
                    .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
            }
        }
    }
}
