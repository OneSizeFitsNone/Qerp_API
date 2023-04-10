using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        public long _companyId;

        public TaskController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await TaskMV.SelectAll(_companyId);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await TaskMV.SelectById(id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(TaskMV task)
        {
            if (_companyId != task.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await task.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(TaskMV task)
        {
            if (_companyId != task.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await task.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(TaskMV task)
        {
            if (_companyId != task.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await task.Delete();
        }
    }
}
