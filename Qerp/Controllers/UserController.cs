using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Qerp.Interfaces;
using Qerp.Models;
using Qerp.ModelViews;
using Qerp.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Qerp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        public IConfiguration _configuration;
        public long _companyId;
        private CurrentUserMM _currentUser;

        public UserController(IConfiguration config, IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = config;
            _currentUser = new CurrentUserMM(memoryCache);
            var token = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            _companyId = _currentUser.GetCompanyByToken(token);
        }

        [HttpGet]
        public async Task<ReturnResult> GetAll()
        {
            return await UserMV.SelectAll(_companyId);
            
        }

        [HttpGet("{id}")]
        public async Task<ReturnResult> Get(long id)
        {
            return await UserMV.SelectById(id, _companyId);
        }

        [HttpPost]
        public async Task<ReturnResult> Post(UserMV User)
        {
            if (_companyId != User.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await User.Insert();
        }

        [HttpPut]
        public async Task<ReturnResult> Put(UserMV User)
        {
            if (_companyId != User.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await User.Update();
        }

        [HttpDelete]
        public async Task<ReturnResult> Delete(UserMV User)
        {
            if (_companyId != User.CompanyId) { new ReturnResult(false, "Access Denied", null); }
            return await User.Delete();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ReturnResult> Post(User _userData)
        {
            if (_userData != null && _userData.Username != null && _userData.Password != null)
            {
                ReturnResult result = await UserMV.Login(_userData.Username, _userData.Password);

                if (result.Success)
                {
                    UserMV? user = result.Object as UserMV;
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserName", user.Username),
                    };
                    
                    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    double expire;
                    if (!Double.TryParse(_configuration["Jwt:Expires"], out expire)) expire = 0;
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(expire),
                        signingCredentials: signIn);

                    string sToken = new JwtSecurityTokenHandler().WriteToken(token);
                    user.LastToken = sToken;

                    await user.Update();

                    Token oToken = new Token();
                    oToken.TokenString = sToken;

                    _currentUser.SetTokenCompany(sToken, user.CompanyId);

                    return new ReturnResult(true, "", user);
                }
                else
                {
                    return result;
                }
                
            }
            else
            {
                return new ReturnResult(false, "err.badrequest", null);
            }
        }


        //private async Task<User?> GetUser(string username, string password)
        //{
        //    try
        //    {
        //        using QerpContext db = new QerpContext();
        //        return await db.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        //    }
        //    catch (Exception ex)
        //    {
        //        var e = ex;
        //        return null;
        //    }

        //}
    }
}
