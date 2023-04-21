using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.ModelViews;
using Qerp.Services;
using System.Threading.Tasks;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
        public long _companyId;

        public ProjectController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await ProjectMV.SelectAll(_companyId);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ProjectMV.SelectById(id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ProjectMV project)
        {
            if (project.Id != 0)
            {
                if (_companyId != project.CompanyId) { return new ReturnResult(false, "Access Denied", null); }
                return await project.Update();
            }
            else
            {
                project.CompanyId = _companyId;
                return await project.Insert();
            }
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ProjectMV project)
        {
            if (_companyId != project.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await project.Delete();
        }

        [HttpPost("search")]
        public async Task<ReturnResult> Search(ProjectMV project)
        {
            return await project.Search(_companyId);
        }
    }
}
