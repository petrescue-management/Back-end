using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _env;
        public UserController(IUnitOfWork uow, IHostingEnvironment environment) : base(uow)
        {
            this._env = environment;
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
        [Authorize(Roles =RoleConstant.MANAGER)]
        [HttpGet("get-list-volunteer-profile-of-center")]
        public IActionResult GetListVolunteerProfileOfCenter()
        {
            try
            {
                var currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _domain = _uow.GetService<UserDomain>();
                var result = _domain.GetListProfileOfVolunter(Guid.Parse(currentCenterId));
                return Success(result);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpGet("get-list-of-member-profiles")]
        public IActionResult GetListOfMemberProfile([FromQuery] int page = 0, [FromQuery] int limit = -1)
        {
            try
            {
                var _domain = _uow.GetService<UserDomain>();
                var result = _domain.GetListProfileMember(page, limit);
                if(result != null)
                {
                    return Success(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [Authorize(Roles = RoleConstant.ADMIN)]
        [HttpGet("get-working-history-by-userId")]
        public IActionResult GetListOfMemberProfile([FromQuery]Guid userId)
        {
            try
            {
                var _domain = _uow.GetService<WorkingHistoryDomain>();
                var result = _domain.GetListWorkingHistoryById(userId);
                return Success(result);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HttpGet("get-profile-by-userid")]
        public IActionResult GetProfileByUserId (Guid userId)
        {
            try
            {
                var _domain = _uow.GetService<UserDomain>();
                var result = _domain.GetProfileByUserId(userId);
                if (result != null)
                {
                    return Success(result);
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
        #endregion
        #region POST
        [Authorize]
        [HttpPost("update-profile")]
        public IActionResult UpdateProfileForUser([FromBody] UserProfileUpdateModel model)
        {
            try
            {
                var _domain = _uow.GetService<UserDomain>();
                var newUserProfile = _domain.UpdateUserProfile(model);
                if (newUserProfile == 1)
                {
                    return Success(newUserProfile);
                }
                return BadRequest();
            } catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpPost("create-role-volunteer-for-user")]
        public IActionResult CreateRoleForUser([FromBody] CreateVolunteerModel model)
        {
            try
            {
                var _domain = _uow.GetService<UserDomain>();
                var _currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _domain.AddVolunteerToCenter(new AddNewRoleModel
                {
                    Email = model.Email,
                    CenterId = Guid.Parse(_currentCenterId),
                    RoleName = RoleConstant.VOLUNTEER,
                    InsertBy = Guid.Parse(_currentUserId),
                    DoB = model.Dob,
                    FirstName = model.FirstName,
                    Gender = model.Gender,
                    LastName = model.LastName,
                    Phone = model.Phone,
                });
                if (!result.Equals(""))
                {
                    return Success(result);
                }
                return BadRequest(result);

            }
            catch (Exception e)
            {
                return Error(e.Message);
            }
        }
        #endregion
        #region DELETE
        [Authorize(Roles = RoleConstant.MANAGER)]
        [HttpDelete("remove-role-volunteer-for-user")]
        public IActionResult RemoveRoleForUser ([FromQuery] RemoveRoleVolunteerModel model)
        {
            try
            {
                var _domain = _uow.GetService<UserDomain>();
                var _currentCenterId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("centerId")).Value;
                var _currentUserId = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Actor)).Value;
                var result = _domain.RemoveVolunteerOfCenter(new RemoveVolunteerRoleModel
                {
                    CenterId = Guid.Parse(_currentCenterId),
                    InsertBy = Guid.Parse(_currentUserId),
                    UserId = model.UserId,
                    Description = model.Description
                });
                if (result.Equals(""))
                {
                    return Success("");
                }
                return BadRequest(result);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
    }

}
