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
    public class ContactController : Controller
    {
        public long _companyId;

        public ContactController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await ContactMV.SelectAll(_companyId);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ContactMV.SelectById(id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ContactMV contact)
        {
            
            if (contact.Id != 0) {
                if (_companyId != contact.CompanyId) { return new ReturnResult(false, "Access Denied", null); }
                return await contact.Update();
            }
            else
            {
                contact.CompanyId = _companyId;
                return await contact.Insert();
            }
        }

        [HttpPut]
        public async Task<ReturnResult> Put(ContactMV contact)
        {
            if (_companyId != contact.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await contact.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ContactMV contact)
        {
            if (_companyId != contact.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await contact.Delete();
        }

        [HttpPost("search")]
        public async Task<ReturnResult> Search(ContactMV contact)
        {
            return await contact.Search(_companyId);
        }
    }
}
