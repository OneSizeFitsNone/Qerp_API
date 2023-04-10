using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProspectController : Controller
    {
        public long _companyId;

        public ProspectController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await ProspectMV.SelectAll();
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ProspectMV.SelectById(id);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ProspectMV prospect)
        {
            return await prospect.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(ProspectMV prospect)
        {
            return await prospect.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ProspectMV prospect)
        {
            return await prospect.Delete();
        }
    }
}
