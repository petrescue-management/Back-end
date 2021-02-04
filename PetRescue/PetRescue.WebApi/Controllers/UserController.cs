using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Domains;
using PetRescue.Data.Models;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRescue.WebApi.Controllers
{
    [ApiController]
    [Route("/api/users/")]
    public class UserController : BaseController
    {
        public UserController(IUnitOfWork uow) : base(uow)
        {
        }
        #region get
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
        #endregion
        #region Post
        [HttpPost("{email}")]
        public IActionResult RegisterUser([FromQuery]String email)
        {
            try
            {
                var userDomain = _uow.GetService<UserDomain>();
                string id = userDomain.RegisterUser(email).UserId.ToString();
                _uow.saveChanges();
                return Success(id);
            }catch(Exception ex)
            {
                return Error(ex);
            }
        }
        [Authorize]
        [HttpPost("update-profile")]
        public IActionResult UpdateProfileForUser([FromBody] UserProfileUpdateModel model)
        {
            try
            {
                var userDomain = _uow.GetService<UserDomain>();
                UserProfile newUserProfile = userDomain.UpdateUserProfile(model);
                if(newUserProfile != null)
                {
                    _uow.saveChanges();
                    return Success(newUserProfile.UserId);
                }
                return Error("Can't update");
            }catch(Exception e)
            {
                return Error(e);
            }
        }
        //[Authorize(Roles = [RoleConstant.Manager, RoleConstant.Admin])]
        //[HttpPost("create-role-for-user")]
        //public IActionResult CreateRoleForUser(UserRoleUpdateModel model)
        //{
        //    try
        //    {
                
        //        var userDomain = _uow.GetService<UserDomain>();
        //        var tempUser = userDomain.AddRoleToUser(model);
        //        if(tempUser != null)
        //        {
        //            _uow.saveChanges();
        //            return Success(tempUser.IsBelongToCenter);
        //        }
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        return Error(e);
        //    }
        //}
        #endregion

    }

}
