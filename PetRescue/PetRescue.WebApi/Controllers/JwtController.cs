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
        public  IActionResult GetToken([FromQuery]string token)
        {
            try
            {
                var jwtDomain = _uow.GetService<JWTDomain>();
                if (ValidationExtensions.IsNotNullOrEmptyOrWhiteSpace(token))
                {
                    var returnToken = jwtDomain.DecodeJwt(token);
                    _uow.saveChanges();
                    return Success(returnToken);
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
                    _uow.saveChanges();
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
