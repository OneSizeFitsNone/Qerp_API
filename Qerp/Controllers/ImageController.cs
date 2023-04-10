using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Qerp.Interfaces;
using Qerp.ModelViews;
using Qerp.Services;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ImageController : Controller
    {
        public long _companyId;

        public ImageController(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            var currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = currentUser.GetCompanyByToken(token);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(ImageMV image)
        {
            if (image.Id != 0)
            {
                return await image.Update();
            }
            else
            {
                return await image.Insert();
            }
        }

        [HttpPost("UpdateList")]
        public async Task<ReturnResult> UpdateList(List<ImageMV> images)
        {
            return await ImageMV.UpdateList(images);
        }

        [HttpPost("Upload"), DisableRequestSizeLimit]
        public async Task<ReturnResult> Upload()
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            return await ImageMV.Upload(file, _companyId);
        }

        [HttpGet("GetByAppType")]
        public async Task<ReturnResult> GetByAppType(long apptypeId, long linktypeId)
        {
            return await ImageMV.SelectByAppTypeLinkedType(apptypeId, linktypeId);      
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(ImageMV image)
        {
            return await image.Delete();
        }
    }
}
