using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.Domains;
using PetRescue.Data.Uow;
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
                var returnToken = jwtDomain.DecodeJwt(token);
                _uow.saveChanges();
                return Success(returnToken);
            }catch(Exception e)
            {
                return Error(e);
            }
        }
    }
}
