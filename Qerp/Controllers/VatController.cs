using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VatController : Controller
    {
        public long _companyId;

        public VatController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await VatMV.SelectAll(_companyId);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await VatMV.SelectById(id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(VatMV vat)
        {
            if (_companyId != vat.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await vat.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(VatMV vat)
        {
            if (_companyId != vat.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await vat.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(VatMV vat)
        {
            if (_companyId != vat.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await vat.Delete();
        }
    }
}
