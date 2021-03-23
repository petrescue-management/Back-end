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
        public JwtController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            _env = environment;
        }
        [HttpGet]
        public  IActionResult GetToken([FromQuery]UserLoginModel model)
        {
            try
            {
                var _domain = _uow.GetService<JWTDomain>();
                if (ValidationExtensions.IsNotNullOrEmptyOrWhiteSpace(model.Token))
                {
                    var result = _domain.DecodeJwt(model);
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
                var jwtDomain = _uow.GetService<JWTDomain>();
                var result = jwtDomain.LoginBySysAdmin(model);
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
        public IActionResult LoginByVolunteer([FromBody] UserLoginModel model)
        {
            try
            {
                string path = _env.ContentRootPath;
                var result = _uow.GetService<JWTDomain>().LoginByVolunteer(model, path);
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
