using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectcontactController : Controller
    {
        public long _companyId;

        public ProjectcontactController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await ProjectcontactMV.SelectAll();
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ProjectcontactMV.SelectById(id);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ProjectcontactMV projectcontact)
        {
            return await projectcontact.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(ProjectcontactMV projectcontact)
        {
            return await projectcontact.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ProjectcontactMV projectcontact)
        {
            return await projectcontact.Delete();
        }
    }
}
