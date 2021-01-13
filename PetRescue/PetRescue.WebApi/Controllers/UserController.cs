using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.Domains;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController : BaseController
    {
        public UserController(IUnitOfWork uow) : base(uow)
        {
        }
        [HttpPost]
        public IActionResult RegisterUser(UserCreateModel model)
        {
            try
            {
                var userDomain = _uow.GetService<UserDomain>();
                string id = userDomain.RegisterUser(model);
                return Success(id);
            }catch(Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetUserDetail()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var userDomain = _uow.GetService<UserDomain>();
                var result = userDomain.GetUserDetail(token.ToString().Split(" ")[1]);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }
    }
   
}
