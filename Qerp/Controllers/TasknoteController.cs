using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasknoteController : Controller
    {
        public long _companyId;

        public TasknoteController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await TasknoteMV.SelectAll();
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await TasknoteMV.SelectById(id);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(TasknoteMV tasknote)
        {
            return await tasknote.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(TasknoteMV tasknote)
        {
            return await tasknote.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(TasknoteMV tasknote)
        {
            return await tasknote.Delete();
        }
    }
}
