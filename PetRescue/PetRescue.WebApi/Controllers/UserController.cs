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
        #region GET
        [Authorize]
        [HttpGet]
        public IActionResult GetUserDetail()
        {
            try
            {
                var token = Request.Headers["Authorization"];
                var _domain = _uow.GetService<UserDomain>();
                var result = _domain.GetUserDetail(token.ToString().Split(" ")[1]);
                return Success(result);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }
        #endregion
        #region POST
        [Authorize]
        [HttpPost("api/update-profile/")]
        public IActionResult UpdateProfileForUser([FromBody] UserProfileUpdateModel model)
        {
            try
            {
                var _domain = _uow.GetService<UserDomain>();
                var newUserProfile = _domain.UpdateUserProfile(model);
                if (newUserProfile != null)
                {
                    return Success(newUserProfile.UserId);
                }
                return BadRequest();
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
                var _domain = _uow.GetService<UserDomain>();
                var _currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var user = _domain.AddUserToCenter(new AddNewRoleModel 
                {
                    Email = email,
                    CenterId = Guid.Parse(_currentCenterId),
                    RoleName = RoleConstant.VOLUNTEER
                });
                return Success(_currentCenterId);
            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion

    }

}
