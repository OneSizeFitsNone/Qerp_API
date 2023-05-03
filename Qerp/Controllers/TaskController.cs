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
    public class TaskController : Controller
    {
        public long _companyId;

        public TaskController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
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
            if (task.Id != 0)
            {
                if (_companyId != task.CompanyId) { return new ReturnResult(false, "Access Denied", null); }
                return await task.Update(); 
            }
            else
            {
                task.CompanyId = _companyId;
                return await task.Insert();
            }            
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(TaskMV task)
        {
            if (_companyId != task.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await task.Delete();
        }

        [HttpGet("SelectByApptype")]
        public async Task<ReturnResult> SelectByApptype(long apptypeId, long id)
        {
            return await TaskMV.SelectByApptype(_companyId, apptypeId, id);
        }

        [HttpPost("search")]
        public async Task<ReturnResult> Search(TaskMV task)
        {
            return await task.Search(_companyId);
        }
    }
}
