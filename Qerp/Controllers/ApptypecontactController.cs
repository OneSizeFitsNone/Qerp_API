using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.ModelViews;
using Qerp.Services;
using System.Data.Common;
using System.Reflection.Metadata;

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
            return await ApptypecontactMV.SelectById(_companyId, id);
        }

        [HttpGet("SelectBySource")]
        public async Task<ReturnResult> SelectBySource(long apptypeId, long linkedId, long requestedType)
        {
            return await ApptypecontactMV.SelectBySource(this._companyId, apptypeId, linkedId, requestedType);
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
                if (_companyId != apptypecontact.CompanyId) { return new ReturnResult(false, "Access Denied", null); }
                return await apptypecontact.Update();
            }
            else
            {
                apptypecontact.CompanyId = _companyId;
                return await apptypecontact.Insert();
            }
        }


        [HttpDelete]
        public async Task<ReturnResult> Delete(ApptypecontactMV apptypecontact)
        {
            if (_companyId != apptypecontact.CompanyId) { return new ReturnResult(false, "Access Denied", null); }
            return await apptypecontact.Delete();
        }
    }
}
