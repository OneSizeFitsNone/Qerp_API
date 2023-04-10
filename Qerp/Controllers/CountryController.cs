using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qerp.Interfaces;
using Qerp.ModelViews;
using Qerp.Services;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await CountryMV.SelectAll();
        }


    }
}
