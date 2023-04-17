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
    public class ParameterController : Controller
    {

        public long _companyId;

        public ParameterController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> GetById(long id)
        {
            return await ParameterMV.GetById(_companyId, id);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ParameterMV parameter)
        {

            if (parameter.Id != 0)
            {
                if (_companyId != parameter.CompanyId) { return new ReturnResult(false, "Access Denied", null); }
                return await parameter.Update();
            }
            else
            {
                parameter.CompanyId = _companyId;
                return await parameter.Insert();
            }
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ParameterMV parameter)
        {
            if (_companyId != parameter.CompanyId) { return new ReturnResult(false, "Access Denied", null); }
            return await parameter.Delete();
        }

        [HttpGet("GetByGroupId/{id}")]
        public async Task<ReturnResult> GetByGroupId(long id)
        {
            return await ParameterMV.GetByGroupId(_companyId, id);
        }

        [HttpGet("GetGroups")]
        public async Task<ReturnResult> GetGroups()
        {
            return await ParametergroupMV.GetAll();
        }

    }
}
