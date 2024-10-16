﻿using Microsoft.Extensions.Caching.Memory;
using Qerp.Interfaces;
using Qerp.Models;
using System;

namespace Qerp.Services
{
    public class CurrentUserMM 
    {
        private IMemoryCache _memoryCache;

        public CurrentUserMM(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public long GetUserByToken(string token)
        {
            if (token is null) return 0;

            token = token.Replace("Bearer ", "");
            var memory = _memoryCache.Get<CurrentUser>(token);

            if (memory != null)
            {
                return memory.UserId;
            }
            else
            {
                return 0;
            }
        }


        public long GetCompanyByToken(string token)
        {
            if(token is null) return 0;

            token = token.Replace("Bearer ", "");
            var memory = _memoryCache.Get<CurrentUser>(token);
            
            if (memory != null)
            {
                return memory.CompanyId;
            }
            else
            {
                return 0;
            }
        }

        public bool SetToken(string token, long companyId, long userId)
        {
            try
            {
                var memory = _memoryCache.Get<CurrentUser>(token);
                if (memory == null)
                {
                    // Set cache options
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromHours(8))
                        .SetAbsoluteExpiration(TimeSpan.FromDays(1))
                        .SetSize(1);

                    _memoryCache.Set(token, new CurrentUser { CompanyId = companyId, UserId = userId }, cacheOptions);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
