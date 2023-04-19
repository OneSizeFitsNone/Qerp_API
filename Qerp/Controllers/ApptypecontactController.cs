using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.ModelViews;
using Qerp.Services;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApptypecontactController : Controller
    {
        public long _companyId;

        public ApptypecontactController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ApptypecontactMV.SelectById(id);
        }

        [HttpGet("SelectByApptypeLinkedId")]
        public async Task<ReturnResult> SelectByApptypeLinkedId(long apptypeId, long linkedId)
        {
            return await ApptypecontactMV.SelectByApptypeLinkedId(this._companyId, apptypeId, linkedId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ApptypecontactMV apptypecontact)
        {
            if (apptypecontact.Id != 0)
            {
                return await apptypecontact.Update();
            }
            else
            {
                return await apptypecontact.Insert();
            }
        }


        [HttpDelete]
        public async Task<ReturnResult> Delete(ApptypecontactMV apptypecontact)
        {
            return await apptypecontact.Delete();
        }
    }
}
