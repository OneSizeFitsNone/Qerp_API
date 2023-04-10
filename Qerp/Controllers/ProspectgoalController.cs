using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProspectgoalController : Controller
    {
        public long _companyId;

        public ProspectgoalController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await ProspectgoalMV.SelectAll();
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ProspectgoalMV.SelectById(id);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ProspectgoalMV prospectgoal)
        {
            return await prospectgoal.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(ProspectgoalMV prospectgoal)
        {
            return await prospectgoal.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ProspectgoalMV prospectgoal)
        {
            return await prospectgoal.Delete();
        }
    }
}
