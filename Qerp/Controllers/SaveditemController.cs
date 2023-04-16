using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qerp.Interfaces;
using Qerp.ModelViews;
using Qerp.Services;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SaveditemController : Controller
    {
        public long _userId;

        public SaveditemController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _userId = currentUser.GetUserByToken(token);
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await SaveditemMV.SelectAll(_userId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(SaveditemMV saveditem)
        {
            if (saveditem.Id != 0)
            {
                if (_userId != saveditem.UserId) { return new ReturnResult(false, "Access Denied", null); }
                return await saveditem.Update();
            }
            else
            {
                saveditem.UserId = _userId;
                return await saveditem.Insert();
            }
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(SaveditemMV saveditem)
        {
            if (_userId != saveditem.UserId) { new ReturnResult(false, "Access Denied", null); }
            return await saveditem.Delete();
        }

    }
}
