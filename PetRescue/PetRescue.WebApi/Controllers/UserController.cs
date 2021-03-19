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
using System.Security.Claims;
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
        [HttpPost]
        [Route("/api/users/{email}")]
        public IActionResult RegisterUser(UserCreateByAppModel model)
        {
            try
            {

                var userDomain = _uow.GetService<UserDomain>();
                string id = userDomain.RegisterUser(model).UserId.ToString();
                _uow.saveChanges();
                return Success(id);
            } catch (Exception ex)
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
                if (newUserProfile != null)
                {
                    _uow.saveChanges();
                    return Success(newUserProfile.UserId);
                }
                return Error("Can't update");
            } catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPost("create-role-volunteer-for-user")]
        public IActionResult CreateRoleForUser([FromQuery]string email)
        {
            try
            {
                var userDomain = _uow.GetService<UserDomain>();
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var user = userDomain.AddUserToCenter(new AddNewRoleModel 
                {
                    Email = email,
                    CenterId = Guid.Parse(currentCenterId),
                    RoleName = RoleConstant.VOLUNTEER
                });
                return Success(user);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion

    }

}
