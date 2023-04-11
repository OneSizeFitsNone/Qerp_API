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
    public class ContactroleController : Controller
    {
        public long _companyId;

        public ContactroleController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await ContactroleMV.SelectAll(_companyId);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ContactroleMV.SelectById(id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ContactroleMV contactrole)
        {
            if (contactrole.Id != 0)
            {
                if (_companyId != contactrole.CompanyId) { return new ReturnResult(false, "Access Denied", null); }
                return await contactrole.Update();
            }
            else
            {
                contactrole.CompanyId = _companyId;
                return await contactrole.Insert();
            }
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ContactroleMV contactrole)
        {
            if (_companyId != contactrole.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await contactrole.Delete();
        }
    }
}
