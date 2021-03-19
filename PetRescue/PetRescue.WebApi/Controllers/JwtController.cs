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
        public JwtController(IUnitOfWork uow) : base(uow)
        {
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
    }
}
