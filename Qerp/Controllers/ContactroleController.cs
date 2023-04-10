using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactroleController : Controller
    {
        public long _companyId;

        public ContactroleController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
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
            if (_companyId != contactrole.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await contactrole.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(ContactroleMV contactrole)
        {
            if (_companyId != contactrole.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await contactrole.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ContactroleMV contactrole)
        {
            if (_companyId != contactrole.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await contactrole.Delete();
        }
    }
}
