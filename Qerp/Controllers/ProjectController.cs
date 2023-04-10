using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
        public long _companyId;

        public ProjectController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
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
            if (_companyId != project.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await project.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(ProjectMV project)
        {
            if (_companyId != project.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await project.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ProjectMV project)
        {
            if (_companyId != project.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await project.Delete();
        }
    }
}
