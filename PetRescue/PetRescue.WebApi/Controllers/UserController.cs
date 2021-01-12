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
        [HttpPut]
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
    }
}
