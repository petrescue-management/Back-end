using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.Domains;
using PetRescue.Data.Extensions;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    [Route("/jwt")]
    public class JwtController : BaseController
    {
        private readonly IHostingEnvironment _env;
        private readonly JWTDomain _jwtDomain;
        public JwtController(IUnitOfWork uow, IHostingEnvironment environment, JWTDomain jwtDomain) : base(uow)
        {
            _env = environment;
            this._jwtDomain = jwtDomain;
        }
        [HttpGet]
        public  IActionResult GetToken([FromQuery]UserLoginModel model)
        {
            try
            {
                if (ValidationExtensions.IsNotNullOrEmptyOrWhiteSpace(model.Token))
                {
                    var result = _jwtDomain.DecodeJwt(model);
                    if (result != null)
                    {
                        return Success(result.Jwt);
                    }
                }
                return BadRequest();
            }catch(Exception e)
            {
                return Error(e.Message);
            }
        }
        [HttpPost("login-by-sysadmin")]
        public IActionResult LoginBySystemAdmin([FromBody] UserLoginBySysadminModel model)
        {
            try
            {
                var result = _jwtDomain.LoginBySysAdmin(model);
                if(result != null)
                {
                    return Success(result);
                }
                return BadRequest("");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpPost("login-by-volunteer")]
        public async Task<IActionResult> LoginByVolunteer([FromBody] UserLoginModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var result = await _jwtDomain.LoginByVolunteer(model,path);
                if(result != null)
                {
                    return Success(result);
                }
                return BadRequest(result);
            }catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
