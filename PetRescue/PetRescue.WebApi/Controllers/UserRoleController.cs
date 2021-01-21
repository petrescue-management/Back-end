using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.Domains;
using PetRescue.Data.Extensions;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    [Route("/api/user-role/")]
    public class UserRoleController : BaseController
    {
        public UserRoleController(IUnitOfWork uow) : base(uow)
        {
        }
        #region Post
       
        #endregion
        #region Put
        //[Authorize(Roles ="manager")]
        //[HttpPut("switch-role")]
        //public IActionResult Update([FromBody] UserRoleUpdateModel model)
        //{
        //    try
        //    {
        //        var userRoleDomain = _uow.GetService<UserRoleDomain>();
        //        var userRole = userRoleDomain.SwitchActiveRole(model);
        //        if (userRole != null)
        //        {
        //            _uow.saveChanges();
        //            return Success(userRole.IsActived);
        //        }
        //        return Success("Not Found");
        //    }
        //    catch (Exception e)
        //    {
        //        return Error(e);
        //    }
        //}
        #endregion
        [HttpGet]
        public IActionResult Test(String htmlString)
        {                //MailArguments mailArguments = MailFormat.MailApproveRegistrationCenter("pjnochjo3095@gmail.com");
                //MailExtensions.Send(mailArguments, null, true, null);
            try
            {


                return Success("Done");
            }
            catch (Exception e) {
                return Error(e);
            }
        }
    }
}
