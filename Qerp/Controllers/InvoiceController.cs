using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qerp.Interfaces;
using Qerp.ModelViews;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        public long _companyId;

        public InvoiceController(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _companyId = Convert.ToInt64(httpContextAccessor.HttpContext?.Session.GetString("companyId") ?? "0");
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await InvoiceMV.SelectAll(_companyId);
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await InvoiceMV.SelectById(id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(InvoiceMV invoice)
        {
            if (_companyId != invoice.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await invoice.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(InvoiceMV invoice)
        {
            if (_companyId != invoice.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await invoice.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(InvoiceMV invoice)
        {
            if (_companyId != invoice.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await invoice.Delete();
        }
    }
}
