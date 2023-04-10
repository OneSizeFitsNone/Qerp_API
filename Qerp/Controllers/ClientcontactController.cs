using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientcontactController : Controller
    {
        public long _companyId;

        public ClientcontactController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await ClientcontactMV.SelectAll();
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await ClientcontactMV.SelectById(id);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ClientcontactMV clientcontact)
        {
            return await clientcontact.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(ClientcontactMV clientcontact)
        {
            return await clientcontact.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ClientcontactMV clientcontact)
        {
            return await clientcontact.Delete();
        }
    }
}
