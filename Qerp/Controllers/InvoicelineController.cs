using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicelineController : Controller
    {
        public long _companyId;

        public InvoicelineController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await InvoicelineMV.SelectAll(_companyId);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await InvoicelineMV.SelectById(id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(InvoicelineMV invoiceline)
        {
            if (_companyId != invoiceline.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await invoiceline.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(InvoicelineMV invoiceline)
        {
            if (_companyId != invoiceline.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await invoiceline.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(InvoicelineMV invoiceline)
        {
            if (_companyId != invoiceline.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await invoiceline.Delete();
        }
    }
}
